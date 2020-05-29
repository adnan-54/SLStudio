using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class ScriptClassCONSRpkRefEntry : ScriptClassCONSConstantEntry
  {
    protected static readonly int rpkNameIndexOffset = 4;
    protected static readonly int typeIDOffset = 8;

    public int RPKnameIndexInConstantTable
    {
      get
      {
        return GetIntFromFile(rpkNameIndexOffset, true);
      }
      set
      {
        SetIntInFile(value, rpkNameIndexOffset, true);
      }
    }
    public int TypeIdInRPK
    {
      get
      {
        return GetIntFromFile(typeIDOffset, true);
      }
      set
      {
        SetIntInFile(value, typeIDOffset, true);
      }
    }
    public string RPKNameString
    {
      get
      {
        return (DataAsIndexLookup[0] as ScriptClassCONSNameEntry).DataAsString;
      }
      set
      {
        parentForIndexLookup.OverwriteEntry(RPKnameIndexInConstantTable,new ScriptClassCONSNameEntry(value, parentForIndexLookup));
      }
    }

    public ScriptClassCONSRpkRefEntry(FileCacheHolder file, int offset, ScriptClassCONSChunk parent = null, bool cache = false)
    : base(file, offset, parent, cache)
    {
    }

    public override string ToString()
    {
      var ret = DataAsIndexLookup[0].ToString();
      return ret + ":0x" + TypeIdInRPK.ToString("X8");
    }
    public override void Save(BinaryWriter bw)
    {
      bw.Write(ID);
      bw.Write(Data);
    }
  }
}
