using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicBoneListV4
  {
    private static readonly int typeDeferNormal = 3;
    private static readonly int typeDeferSparse = 2; //2 bones defined (0,1) in the mesh there was no boneindref and 1 boneweight defed, used in springlike deformable objects
    private int type = 0;

    public int Size
    {
      get
      {
        return 4*3 + BoneIndices.Count*4;
      }
    }
    public int UnkownNull
    {
      get;
      set;
    }
    public int Type
    {
      get
      {
        return type;
      }
      set
      {
        if (value != typeDeferNormal && value != typeDeferSparse)
          return;
        type = value;
      }
    }
    public List<int> BoneIndices
    {
      get;
      set;
    } = new List<int>();

    public DynamicBoneListV4(BinaryBoneListV4 constructFrom = null)
    {
      if (constructFrom == null)
        return;
      BoneIndices.AddRange(constructFrom.Indices);
      UnkownNull = constructFrom.UnkownNull;
      Type = constructFrom.Type;

      if (constructFrom.Size != Size)
        throw new Exception("HeaderWill Mismatch");
    }

    public void Save(BinaryWriter bw)
    {
      bw.Write(Type);
      bw.Write(Size);
      bw.Write(UnkownNull);
      foreach(var boneInd in BoneIndices)
      {
        bw.Write(boneInd);
      }
    }
  }
}
