using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model.HighLevel
{
  public class SlotReBaser
  {
    private GameFileManager gManag;
    private RpkManager rpk;

    public SlotReBaser(GameFileManager gManag, RpkManager rpk)
    {
      this.gManag = gManag;
      this.rpk = rpk;
    }

    public void GeatherMovementToAllSlotsOfTypeID(int typeID)
    {
      var cfg = rpk.GetCfgFromTypeID(typeID);
      if(cfg == null)
      {
        return;
      }
      HashSet<int> toSave = new HashSet<int>();
      foreach(var slotID in cfg.Slots)
      {
        toSave.UnionWith(geatherMovementToSlotReturnEditedTypeIDs(typeID, slotID.SlotID));
      }
      foreach (var s in toSave)
      {
        var cfgSave = rpk.GetCfgFromTypeID(s);
        if (cfgSave == null)
          continue;
        cfgSave.Save();
      }
    }
    public float GetAvgMovementToSlotGathering(int typeID)
    {
      var cfg = rpk.GetCfgFromTypeID(typeID);
      if (cfg == null)
      {
        return 0;
      }
      float avg = 0;
      float c = 0;
      foreach (var slotID in cfg.Slots)
      {
        float toad = getAvgMovementToSlotGatheringAtSlotID(typeID, slotID.SlotID);
        if(toad != 0)
        {
          c++;
          avg += toad;
        }
      }
      if (c == 0)
        return 0;
      avg /= c;
      return avg;
    }
    public void GeatherMovementToSlot(int typeID,int slotID)
    {
      var toSave = geatherMovementToSlotReturnEditedTypeIDs(typeID, slotID);
      foreach(var s in toSave)
      {
        var cfg = rpk.GetCfgFromTypeID(s);
        if (cfg == null)
          continue;
        cfg.Save();
      }
    }

    private float getAvgMovementToSlotGatheringAtSlotID(int typeID, int slotID)
    {
      var baseSlot = rpk.GetSlotFromTypeIDAndSlotID(typeID, slotID, true);
      if (baseSlot == null)
        return 0;
      if (baseSlot.IsWheelSlot)
      {
        //MessageLog.AddMessage("Won't rebase slot part of a wheel definition would produce unwanted result.");
        return 0;
      }
      var asSlotRef = new RpkSlotReference(rpk, typeID, slotID);
      HashSet<RpkSlotReference> allSlots = new HashSet<RpkSlotReference>(gManag.GetLocalAttachableReferencesForSlot(asSlotRef.Rpk, asSlotRef.SlotRef.TypeID, asSlotRef.SlotRef.SlotID));
      allSlots.RemoveWhere(x => rpk.GetSlotFromTypeIDAndSlotID(x.SlotRef.TypeID, x.SlotRef.SlotID, false) == null);
      if (!allSlots.Any())
      {
        //MessageLog.AddMessage("Did not find any attachable slots locally for typeID: 0x" + typeID.ToString("X8") + " slotID: " + slotID.ToString() + " cfg: " + rpk.GetCfgFromTypeID(typeID).CfgFileName);
        return 0;
      }
      if (allSlots.Any(x => x.SlotRef.TypeID == typeID))
      {
        MessageLog.AddError("Circular cfg slot ref found starting from cfg: "
                            + rpk.GetCfgFromTypeID(typeID).CfgFileName
                            + " with slotID: " + slotID.ToString() + " typeID: 0x" + typeID.ToString("X8"));
        MessageLog.AddError("Aborting!");
        return 0;
      }
      double avgX = 0;
      double avgY = 0;
      double avgZ = 0;
      double c = allSlots.Count;
      foreach (var slotRef in allSlots)
      {
        var slotRefSlot = rpk.GetSlotFromTypeIDAndSlotID(slotRef.SlotRef.TypeID, slotRef.SlotRef.SlotID, true);
        if (slotRefSlot == null)
          continue;
        avgX += slotRefSlot.LineX;
        avgY += slotRefSlot.LineY;
        avgZ += slotRefSlot.LineZ;
      }
      avgX /= c;
      avgY /= c;
      avgZ /= c;
      return (float)Math.Sqrt(avgX * avgX + avgY * avgY + avgZ * avgZ);
    }
    private HashSet<int> geatherMovementToSlotReturnEditedTypeIDs(int typeID, int slotID)
    {
      var baseSlot = rpk.GetSlotFromTypeIDAndSlotID(typeID, slotID, true);
      if (baseSlot == null)
        return new HashSet<int>();
      if(baseSlot.IsWheelSlot)
      {
        MessageLog.AddMessage("Won't rebase slot part of a wheel definition would produce unwanted result.");
        return new HashSet<int>();
      }
      var asSlotRef = new RpkSlotReference(rpk, typeID, slotID);
      HashSet<RpkSlotReference> allSlots = new HashSet<RpkSlotReference>(gManag.GetLocalAttachableReferencesForSlot(asSlotRef.Rpk, asSlotRef.SlotRef.TypeID, asSlotRef.SlotRef.SlotID));
      allSlots.RemoveWhere(x => rpk.GetSlotFromTypeIDAndSlotID(x.SlotRef.TypeID, x.SlotRef.SlotID, true) == null);
      if (!allSlots.Any())
      {
        MessageLog.AddMessage("Did not find any attachable slots locally for typeID: 0x" + typeID.ToString("X8") + " slotID: " + slotID.ToString() + " cfg: " + rpk.GetCfgFromTypeID(
                                typeID).CfgFileName);
        return new HashSet<int>();
      }
      if (allSlots.Any(x => x.SlotRef.TypeID == typeID))
      {
        MessageLog.AddError("Circular cfg slot ref found starting from cfg: "
                            + rpk.GetCfgFromTypeID(typeID).CfgFileName
                            + " with slotID: " + slotID.ToString() + " typeID: 0x" + typeID.ToString("X8"));
        MessageLog.AddError("Aborting!");
        return new HashSet<int>();
      }
      double avgX = 0;
      double avgY = 0;
      double avgZ = 0;
      double c = allSlots.Count;
      foreach (var slotRef in allSlots)
      {
        var slotRefSlot = rpk.GetSlotFromTypeIDAndSlotID(slotRef.SlotRef.TypeID, slotRef.SlotRef.SlotID, true);
        if (slotRefSlot == null)
          continue;
        avgX += slotRefSlot.LineX;
        avgY += slotRefSlot.LineY;
        avgZ += slotRefSlot.LineZ;
      }
      avgX /= c;
      avgY /= c;
      avgZ /= c;
      foreach (var slotRef in allSlots)
      {
        var slotRefSlot = rpk.GetSlotFromTypeIDAndSlotID(slotRef.SlotRef.TypeID, slotRef.SlotRef.SlotID, true);
        if (slotRefSlot == null)
          continue;
        slotRefSlot.LineX -= (float)avgX;
        slotRefSlot.LineY -= (float)avgY;
        slotRefSlot.LineZ -= (float)avgZ;
      }
      var ret = new HashSet<int>();
      foreach (var slotRef in allSlots)
      {
        var cfg = rpk.GetCfgFromTypeID(slotRef.SlotRef.TypeID);
        if (cfg == null)
          continue;
        ret.Add(slotRef.SlotRef.TypeID);
      }
      baseSlot.LineX -= (float)avgX;
      baseSlot.LineY -= (float)avgY;
      baseSlot.LineZ -= (float)avgZ;
      ret.Add(typeID);
      return ret;
    }
  }
}
