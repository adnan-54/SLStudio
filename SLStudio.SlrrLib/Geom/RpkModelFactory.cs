using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class RpkModelFactory : IModelFactory
    {
        private string rpkFnam;
        private string slrrRoot;
        private SlrrLib.Model.HighLevel.GameFileManager lazygManag;
        private SlrrLib.Model.HighLevel.RpkManager rpk;

        public RpkModelFactory(string rpkFnam, string slrrRoot)
        {
            lazygManag = null;
            this.rpkFnam = rpkFnam;
            this.slrrRoot = slrrRoot;
            rpk = new SlrrLib.Model.HighLevel.RpkManager(rpkFnam, SlrrLib.Model.HighLevel.GameFileManager.GetSLRRRoot(rpkFnam));
        }

        public IEnumerable<NamedModel> GenModels(string modelGroup)
        {
            int typeID = typeIDFromGroupString(modelGroup);
            var cfg = rpk.GetCfgFromTypeID(typeID);
            List<NamedModel> ret = new List<NamedModel>();
            //bodyLines
            foreach (var bodyLine in cfg.BodyLines)
            {
                if (!File.Exists(slrrRoot + "\\" + bodyLine.BodyModel))
                {
                    SlrrLib.Model.MessageLog.AddError("Scx file not found: " + bodyLine.BodyModel + " refed in cfg: " + cfg.CfgFileName + " as a body-line");
                    continue;
                }
                Vector3D bodyPosAsVec3 = new Vector3D(bodyLine.LineX, bodyLine.LineY, bodyLine.LineZ);
                ret.AddRange(namedModelsFromFnamAndNamePrefix(slrrRoot + "\\" + bodyLine.BodyModel,
                             Path.GetFileNameWithoutExtension(cfg.CfgFileName)
                             + ":" + Path.GetFileNameWithoutExtension(bodyLine.BodyModel),
                             bodyLine, NamedModelSource.BodyLine, bodyPosAsVec3, cfg));
            }
            //mesh
            int meshTypeID = cfg.GetTypeIDRefLineTypeIDFromName("mesh");
            int refPart = SlrrLib.Model.HighLevel.RpkManager.TypeIDExternalRefPart(meshTypeID);
            int IDPart = SlrrLib.Model.HighLevel.RpkManager.TypeIDExternalIDPart(meshTypeID);
            var extRpk = requestGManag().GetRpkFromRefString(rpk.GetRpkExternalRefIndAsRPKRefString(refPart));
            if (extRpk == null)
            {
                SlrrLib.Model.MessageLog.AddError("TypeID(mesh): 0x" + IDPart.ToString("X8") + " points to unkown external rpk in cfg: "
                                                  + cfg.CfgFileName);
            }
            else
            {
                var res = extRpk.GetResEntry(IDPart);
                if (extRpk == null)
                {
                    SlrrLib.Model.MessageLog.AddError("TypeID(mesh: 0x" + IDPart.ToString("X8") + " not found refed in cfg: "
                                                      + cfg.CfgFileName + " as a mesh looked up in rpk: " + extRpk.RpkFileName);
                }
                else
                {
                    var mesRes = requestGManag().GetFullSCXPathFromTypeByteResEntryWithLineKeySCXLine(res, extRpk, 5, "sourcefile");
                    string toadMesh = "";
                    if (mesRes != null)
                        toadMesh = mesRes.ScxFullFnam;
                    if (!File.Exists(toadMesh))
                    {
                        SlrrLib.Model.MessageLog.AddError("Scx file not found: " + toadMesh + " refed in cfg: " + cfg.CfgFileName + " as a mesh with ID: 0x" + meshTypeID.ToString("X8"));
                    }
                    else
                    {
                        ret.AddRange(namedModelsFromFnamAndNamePrefix(toadMesh,
                                     Path.GetFileNameWithoutExtension(cfg.CfgFileName)
                                     + ":" + Path.GetFileNameWithoutExtension(toadMesh),
                                     null, NamedModelSource.BodyLine, new Vector3D(0, 0, 0), null));
                    }
                }
            }
            return ret;
            //thats it... (click and deformable probably would be nice)
        }

        public IEnumerable<string> GetModelGroups()
        {
            if (modelGroupsCache != null)
                return modelGroupsCache;
            List<string> groups = new List<string>();
            foreach (var res in rpk.resKeyToResEntry)
            {
                var cfg = rpk.GetCfgFromTypeID(res.Key, false);
                if (cfg == null)
                    continue;
                var cls = rpk.GetClassPairFromTypeID(res.Key);
                if (cls == null)
                    continue;
                groups.Add("0x" + res.Key.ToString("X8") + " | " + Path.GetFileNameWithoutExtension(cls.GetClassFileName()) + " : " + cfg.CfgFileName);
            }
            modelGroupsCache = groups;
            return groups;
        }

        private SlrrLib.Model.HighLevel.GameFileManager requestGManag()
        {
            if (lazygManag != null)
                return lazygManag;
            lazygManag = new SlrrLib.Model.HighLevel.GameFileManager(SlrrLib.Model.HighLevel.GameFileManager.GetSLRRRoot(rpk.RpkFileName));
            lazygManag.BuildRpkDict();
            lazygManag.ManualAddRPK(rpk, rpk.RpkFileName);
            return lazygManag;
        }

        private int typeIDFromGroupString(string groupName)
        {
            return int.Parse(groupName.Substring(2, 8), System.Globalization.NumberStyles.HexNumber);
        }

        private IEnumerable<NamedModel> namedModelsFromFnamAndNamePrefix(string fnam, string name,
            SlrrLib.Model.CfgBodyLine bodyLine, NamedModelSource source,
            Vector3D Translate, SlrrLib.Model.Cfg cfg)
        {
            List<NamedModel> ret = new List<NamedModel>();
            SlrrLib.Model.Scx scxMeshBase = SlrrLib.Model.Scx.ConstructScx(fnam);
            int curInd = 0;
            if (scxMeshBase.Version == 4)
            {
                SlrrLib.Model.BinaryScxV4 scxMesh = new SlrrLib.Model.BinaryScxV4(fnam, true);
                ScxV4Geometry geom = new ScxV4Geometry(scxMesh);
                foreach (var partMEsh in geom.WpfModels(1))
                {
                    var translate = NamedCfgInducedModel.GetTranslateVec3FromBodyLinePos(bodyLine);
                    Transform3DGroup transform3DGroup = new Transform3DGroup();
                    transform3DGroup.Children.Add(new TranslateTransform3D(translate));
                    partMEsh.Transform = transform3DGroup;
                    ret.Add(new NamedCfgInducedModel
                    {
                        BodyLine = bodyLine,
                        Name = name + "_" + curInd.ToString(),
                        SourceOfModel = source,
                        ModelGeom = partMEsh,
                        Translate = Translate,
                        Cfg = cfg
                    });
                    curInd++;
                }
            }
            else if (scxMeshBase.Version == 3)
            {
                SlrrLib.Model.BinaryScxV3 scxMesh = new SlrrLib.Model.BinaryScxV3(fnam, true);
                ScxV3Geometry geom = new ScxV3Geometry(scxMesh);
                foreach (var partMEsh in geom.WpfModels(1, -1))
                {
                    var translate = NamedCfgInducedModel.GetTranslateVec3FromBodyLinePos(bodyLine);
                    Transform3DGroup transform3DGroup = new Transform3DGroup();
                    transform3DGroup.Children.Add(new TranslateTransform3D(translate));
                    partMEsh.Transform = transform3DGroup;
                    ret.Add(new NamedCfgInducedModel
                    {
                        BodyLine = bodyLine,
                        Name = name + "_" + curInd.ToString(),
                        SourceOfModel = source,
                        ModelGeom = partMEsh,
                        Translate = Translate,
                        Cfg = cfg
                    });
                    curInd++;
                }
            }
            return ret;
        }

        private IEnumerable<string> modelGroupsCache = null;
    }
}