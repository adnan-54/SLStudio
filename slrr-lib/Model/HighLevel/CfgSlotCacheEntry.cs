using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model.HighLevel
{
  public class CfgSlotCacheEntry
  {
    public int TypeIDOfSlotOwner
    {
      get;
      private set;
    }
    public int SlotID
    {
      get;
      private set;
    }
    public List<CfgSlotReference> AttachLines
    {
      get;
      private set;
    } = new List<CfgSlotReference>();
    public List<CfgSlotReference> CompatibleLines
    {
      get;
      private set;
    } = new List<CfgSlotReference>();

    public CfgSlotCacheEntry(int typeID,int slotID)
    {
      TypeIDOfSlotOwner = typeID;
      SlotID = slotID;
    }

    public override string ToString()
    {
      return "(0x" + TypeIDOfSlotOwner.ToString("X8") + "-" + SlotID.ToString() + ")"+AttachLines.Count.ToString()+", "+CompatibleLines.Count.ToString();
    }
  }
}
