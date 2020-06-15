using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class SCXModelFactor : IModelFactory
    {
        private IEnumerable<string> modelGroupsCache = null;

        public IEnumerable<NamedModel> GenModels(string fnam)
        {
            return GetModels(fnam, new Vector3D(0, 0, 0), new Vector3D(0, 0, 0));
        }

        public IEnumerable<NamedModel> GetModels(string fnam, Vector3D translate, Vector3D ypr)
        {
            List<NamedScxModel> ret = new List<NamedScxModel>();
            SlrrLib.Model.Scx scxMeshBase = SlrrLib.Model.Scx.ConstructScx(fnam);
            int curInd = 0;
            string name = Path.GetFileNameWithoutExtension(fnam);
            if (scxMeshBase.Version == 4)
            {
                SlrrLib.Model.BinaryScxV4 scxMesh = new SlrrLib.Model.BinaryScxV4(fnam, true);
                SlrrLib.Model.DynamicScxV4 scxV4 = new SlrrLib.Model.DynamicScxV4(scxMesh);
                ScxV4Geometry geom = new ScxV4Geometry(scxMesh);
                int ind = 0;
                foreach (var partMEsh in geom.WpfModels(1))
                {
                    Transform3DGroup transform3DGroup = new Transform3DGroup();
                    transform3DGroup.Children.Add(new YprRotation3D(ypr).TransformValues);
                    transform3DGroup.Children.Add(new TranslateTransform3D(translate));
                    partMEsh.Transform = transform3DGroup;
                    ret.Add(new NamedScxModel
                    {
                        Name = name + "_" + curInd.ToString() + "(v4)",
                        SourceOfModel = NamedModelSource.Mesh,
                        ModelGeom = partMEsh,
                        Translate = translate,
                        Ypr = ypr,
                        Scxv4Source = scxV4,
                        MeshIndex = ind,
                        ScxFnam = fnam
                    });
                    curInd++;
                    ind++;
                }
            }
            else if (scxMeshBase.Version == 3)
            {
                SlrrLib.Model.BinaryScxV3 scxMesh = new SlrrLib.Model.BinaryScxV3(fnam, true);
                SlrrLib.Model.DynamicScxV3 scxV3 = new SlrrLib.Model.DynamicScxV3(scxMesh);
                ScxV3Geometry geom = new ScxV3Geometry(scxMesh);
                int ind = 0;
                foreach (var partMEsh in geom.WpfModels(1, -1))
                {
                    Transform3DGroup transform3DGroup = new Transform3DGroup();
                    transform3DGroup.Children.Add(new YprRotation3D(ypr).TransformValues);
                    transform3DGroup.Children.Add(new TranslateTransform3D(translate));
                    partMEsh.Transform = transform3DGroup;
                    ret.Add(new NamedScxModel
                    {
                        Name = name + "_" + curInd.ToString() + "(v3)",
                        SourceOfModel = NamedModelSource.Mesh,
                        ModelGeom = partMEsh,
                        Translate = translate,
                        Ypr = ypr,
                        Scxv3Source = scxV3,
                        MeshIndex = ind,
                        ScxFnam = fnam
                    });
                    curInd++;
                    ind++;
                }
            }
            return ret;
        }

        public IEnumerable<string> GetModelGroups()
        {
            return modelGroupsCache;
        }
    }
}