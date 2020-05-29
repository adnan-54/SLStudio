using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicMaterialV4IntEntry : DynamicMaterialV4Entry
  {
    public int Data
    {
      get;
      set;
    }

    public DynamicMaterialV4IntEntry(BinaryMaterialV4Entry constructFrom = null)
    : base(constructFrom)
    {
      base.type = 1280;
      if (constructFrom == null)
        return;
      UnknownFlag = constructFrom.UnknownFlag;
      type = constructFrom.Type;
      Data = constructFrom.DataAsInt;

      if (constructFrom.Size != Size)
        throw new Exception("HeaderWill Mismatch");
    }

    public override void Save(BinaryWriter bw)
    {
      bw.Write(UnknownFlag);
      bw.Write(this.type);
      bw.Write(Data);
    }
  }
}
