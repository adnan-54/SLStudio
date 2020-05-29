using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model.HighLevel
{
  public class CarMeshPivotNormalizer
  {
    private ScxCache scxCache;
    private GameFileManager gManag;
    private RpkManager rpk;
    private List<PivotNormalizingNode> operations = new List<PivotNormalizingNode>();

    public CarMeshPivotNormalizer(GameFileManager gManag,RpkManager rpk)
    {
      this.gManag = gManag;
      this.rpk = rpk;
      scxCache = new ScxCache(gManag.SlrrRoot);
    }

    public void GenerateOperation()
    {
      Dictionary<string, PivotNormalizingNode> scxFnamToOperation = new Dictionary<string, PivotNormalizingNode>();
      foreach(var res in rpk.PartDefTypeIDs())
      {
        var cfg = rpk.GetCfgFromTypeID(res, false);
        if (cfg == null)
        {
          MessageLog.AddError("TypeID: 0x" + res.ToString("X8") + " does not define proper cfg as native part in rpk: " + rpk.RpkFileName);
          continue;
        }
        int meshID = cfg.GetTypeIDRefLineTypeIDFromName("mesh");
        if(meshID == -1)
        {
          MessageLog.AddError("TypeID: 0x" + res.ToString("X8") + " defines native part cfg: " + cfg.CfgFileName + " but this cfg does not define a proper mesh typeid");
          continue;
        }
        var scxFile = gManag.GetFullSXCFnamOfTypeID(rpk, meshID);
        if (scxFile == null)
        {
          MessageLog.AddError("TypeID: 0x" + res.ToString("X8") + " defines native part cfg: " + cfg.CfgFileName + " but this cfg does not refer to a proper mesh typeid(0x" + meshID.ToString("X8")
                              +")");
          continue;
        }
        string scxFnam = scxFile.ScxFullFnam.ToLower();
        PivotNormalizingNode toad = null;
        if(scxFnamToOperation.ContainsKey(scxFnam))
        {
          toad = scxFnamToOperation[scxFnam];
        }
        else
        {
          toad = new PivotNormalizingNode(scxCache,scxFnam);
          operations.Add(toad);
          scxFnamToOperation[scxFnam] = toad;
        }
        toad.Cfgs.Add(cfg);
      }
    }
    public float GetAvgPivotDifference()
    {
      if (operations.Count == 0)
        GenerateOperation();
      float ret = 0;
      foreach(var op in operations)
      {
        ret += (float)op.GetMedianPivot().Length / (float)operations.Count;
      }
      return ret;
    }
    public void ProcessGenedOperation()
    {
      foreach(var operation in operations)
      {
        operation.TranslateSCXsAndCorrespondingSlot();
      }
    }
  }
}
