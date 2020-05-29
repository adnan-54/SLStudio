using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicHardSurfaceDefV4
  {
    public static readonly int TypeDefer = 1;

    public int Size
    {
      get
      {
        return 12;//size,type,null
      }
    }
    public int UnkownNull
    {
      get;
      set;
    }

    public DynamicHardSurfaceDefV4(BinaryHardSurfaceDefV4 constructFrom = null)
    {
      if (constructFrom == null)
        return;
      this.UnkownNull = constructFrom.UnkownNull;

      if (constructFrom.Size != Size)
        throw new Exception("HeaderWill Mismatch");
    }

    public void Save(BinaryWriter bw)
    {
      bw.Write(TypeDefer);
      bw.Write(Size);
      bw.Write(UnkownNull);
    }
  }
}
