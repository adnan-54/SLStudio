using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace SlrrLib.Model.HighLevel
{
    public class CarScalerNode : Comparer<CarScalerNode>
    {
        private GameFileManager gameManager;
        private Vector3D scaleCenter;
        private float scaleFactor;
        private int? requestedHashCode;

        public RpkManager Rpk
        {
            get;
            private set;
        }

        public int TypeID
        {
            get;
            set;
        }

        public HashSet<RpkSCXFile> ScxsToProcess
        {
            get;
            private set;
        }

        public CarScalerNode(RpkManager rpk, int typeID, Vector3D ScaleCenter, float ScaleFactor, GameFileManager gameManager)
        {
            this.Rpk = rpk;
            this.TypeID = typeID;
            this.scaleFactor = ScaleFactor;
            this.scaleCenter = ScaleCenter;
            this.gameManager = gameManager;
            ScxsToProcess = new HashSet<RpkSCXFile>(
              gameManager.GetAllFullFnamOfUsedSCXsInCfg(rpk, typeID).Where(x => rpk.IsFullFnamProbableForLocalPath(x.ScxFullFnam))
            );
            foreach (var scxs in ScxsToProcess)
                scxs.ScxFullFnam = scxs.ScxFullFnam.ToLower();
        }

        public void ScalePosLinesInCfg()
        {
            var cfg = Rpk.GetCfgFromTypeID(TypeID);
            if (cfg == null)
                return;
            foreach (var posLine in cfg.LinesWithPositionData)
            {
                Vector3D curPosOfLine = new Vector3D(posLine.LineX, posLine.LineY, posLine.LineZ);
                curPosOfLine -= scaleCenter;
                curPosOfLine *= scaleFactor;
                curPosOfLine += scaleCenter;
                posLine.LineX = (float)curPosOfLine.X;
                posLine.LineY = (float)curPosOfLine.Y;
                posLine.LineZ = (float)curPosOfLine.Z;
            }
            cfg.Save();
        }

        public void ScaleSCXs()
        {
            foreach (var scx in ScxsToProcess)
            {
                MessageLog.AddMessage("Scaling " + scx.ScxFullFnam);
                if (scx.Rpk.RpkFileName != Rpk.RpkFileName)
                    continue;
                var model = Scx.ConstructScx(scx.ScxFullFnam);
                model.CahceData();
                if (model is BinaryScxV3)
                {
                    var scxv3 = new DynamicScxV3(model as BinaryScxV3);
                    foreach (var mesh in scxv3.Meshes)
                    {
                        foreach (var vert in mesh.VertexDatas)
                        {
                            vert.VertexCoordX -= (float)scaleCenter.X;
                            vert.VertexCoordY -= (float)scaleCenter.Y;
                            vert.VertexCoordZ -= (float)scaleCenter.Z;
                            vert.VertexCoordX *= scaleFactor;
                            vert.VertexCoordY *= scaleFactor;
                            vert.VertexCoordZ *= scaleFactor;
                            vert.VertexCoordX += (float)scaleCenter.X;
                            vert.VertexCoordY += (float)scaleCenter.Y;
                            vert.VertexCoordZ += (float)scaleCenter.Z;
                        }
                    }
                    scxv3.SaveAs(model.FileName, true);
                }
                else if (model is BinaryScxV4)
                {
                    var scxv4 = new DynamicScxV4(model as BinaryScxV4);
                    foreach (var mesh in scxv4.Meshes)
                    {
                        foreach (var vert in mesh.VertexData.VertexDataList)
                        {
                            vert.Position = new BasicXYZ
                            {
                                X = ((vert.Position.X - (float)scaleCenter.X) * scaleFactor) + (float)scaleCenter.X,
                                Y = ((vert.Position.Y - (float)scaleCenter.Y) * scaleFactor) + (float)scaleCenter.Y,
                                Z = ((vert.Position.Z - (float)scaleCenter.Z) * scaleFactor) + (float)scaleCenter.Z
                            };
                        }
                    }
                    scxv4.SaveAs(model.FileName, true);
                }
            }
        }

        public HashSet<CarScalerNode> GetChildrenBySlots()
        {
            var cfg = Rpk.GetCfgFromTypeID(TypeID);
            if (cfg == null)
                return new HashSet<CarScalerNode>();
            var ret = new HashSet<CarScalerNode>();
            foreach (var slot in cfg.Slots)
            {
                var allAttachable = gameManager.GetLocalAttachableReferencesForSlot(Rpk, TypeID, slot.SlotID);
                foreach (var attachabel in allAttachable)
                {
                    ret.Add(new CarScalerNode(Rpk, attachabel.SlotRef.TypeID, new Vector3D(0, 0, 0), scaleFactor, gameManager));
                }
            }
            return ret;
        }

        public override string ToString()
        {
            return Rpk.RpkFileName + ":" + TypeID.ToString();
        }

        public override int Compare(CarScalerNode x, CarScalerNode y)
        {
            return string.Compare(x.ToString(), y.ToString());
        }

        public override int GetHashCode()
        {
            if (!requestedHashCode.HasValue)
            {
                requestedHashCode = StringComparer.InvariantCulture.GetHashCode(this.ToString());
            }
            return requestedHashCode.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is CarScalerNode)
            {
                return Compare(obj as CarScalerNode, this) == 0;
            }
            return base.Equals(obj);
        }
    }
}