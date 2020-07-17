using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace SlrrLib.Model.HighLevel
{
    public class CarScaler
    {
        public HashSet<CarScalerNode> Nodes
        {
            get;
            set;
        } = new HashSet<CarScalerNode>();

        public void GetNodesFor(RpkManager rpk, GameFileManager gameFiles, float scaleFactor, int typeID)
        {
            HashSet<CarScalerNode> toad = new HashSet<CarScalerNode>
      {
        new CarScalerNode(rpk, typeID, new Vector3D(0, 0, 0), scaleFactor, gameFiles)
      };
            while (toad.Any())
            {
                HashSet<CarScalerNode> toadNext = new HashSet<CarScalerNode>();
                foreach (var carCh in toad)
                {
                    MessageLog.AddMessage("nodes: " + carCh.TypeID.ToString("X8"));
                    toadNext.UnionWith(carCh.GetChildrenBySlots());
                }
                Nodes.UnionWith(toad);
                toadNext.ExceptWith(Nodes);
                toad = toadNext;
            }
        }

        public void ProcessNodes()
        {
            HashSet<RpkSCXFile> processedSCXs = new HashSet<RpkSCXFile>();
            foreach (var node in Nodes)
            {
                MessageLog.AddMessage("Scaling node: " + node.TypeID.ToString("X8"));
                var intersect = processedSCXs.Intersect(node.ScxsToProcess);
                if (intersect.Any())
                {
                    foreach (var collision in intersect)
                    {
                        MessageLog.AddError("Multiple ResEntries reference the scx: " + collision.ScxFullFnam);
                        foreach (var refing in Nodes.Where(x => x.ScxsToProcess.Contains(collision)))
                        {
                            var cfg = refing.Rpk.GetCfgFromTypeID(refing.TypeID, false);
                            MessageLog.AddError("Cfg: " + cfg.CfgFileName + " TypeID: 0x" + refing.TypeID.ToString("X8") + " refs scx: " + collision.ScxFullFnam);
                        }
                    }
                    node.ScxsToProcess.ExceptWith(processedSCXs);
                }
                node.ScalePosLinesInCfg();
                node.ScaleSCXs();
                processedSCXs.UnionWith(node.ScxsToProcess);
            }
        }
    }
}