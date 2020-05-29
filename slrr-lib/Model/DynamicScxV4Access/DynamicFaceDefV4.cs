using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicFaceDefV4
  {
    public static readonly int TypeDefer = 5;

    public List<ushort> Indices
    {
      get;
      set;
    } = new List<ushort>();
    public int Size
    {
      get
      {
        return Indices.Count * sizeof(short) + 12;
      }
    }

    public DynamicFaceDefV4(BinaryFaceDefV4 constructFrom = null)
    {
      if (constructFrom == null)
        return;
      Indices.AddRange(constructFrom.GetShorts);

      if (constructFrom.Size != Size)
        throw new Exception("HeaderWill Mismatch");
    }

    public void FixNumberOfIndices()
    {
      while (Indices.Count % 3 != 0)
        Indices.RemoveAt(Indices.Count - 1);
    }
    public void Save(BinaryWriter bw)
    {
      bw.Write(TypeDefer);
      bw.Write(Size);
      bw.Write((int)Indices.Count);
      foreach(var index in Indices)
      {
        bw.Write(index);
      }
    }
  }
}
