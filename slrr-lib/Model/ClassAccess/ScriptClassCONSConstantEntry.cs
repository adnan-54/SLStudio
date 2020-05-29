using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class ScriptClassCONSConstantEntry : FileEntry
  {
    protected static readonly int idOffset = 0;
    protected static readonly int dataOffset = 4;

    protected ScriptClassCONSChunk parentForIndexLookup = null;

    public int ID
    {
      get
      {
        return GetIntFromFile(idOffset, true);
      }
      set
      {
        SetIntInFile(value, idOffset, true);
      }
    }
    public byte[] Data
    {
      get
      {
        return GetFixLengthChunk(dataOffset, GetDataSizeFromID(),true);
      }
    }
    public int[] DataAsInt
    {
      get
      {
        if (GetDataSizeFromID() == 8)
          return new int[] { GetIntFromFile(dataOffset, true), GetIntFromFile(dataOffset + 4, true) };
        else if (GetDataSizeFromID() == 4)
          return new int[] { GetIntFromFile(dataOffset, true) };
        else
          return new int[] { };
      }
    }
    public ScriptClassCONSConstantEntry[] DataAsIndexLookup
    {
      get
      {
        if(parentForIndexLookup == null)
          return new ScriptClassCONSConstantEntry[] { };
        if (GetDataSizeFromID() == 8)
        {
          ScriptClassCONSConstantEntry left = null;
          ScriptClassCONSConstantEntry right = null;
          int left_i = GetIntFromFile(dataOffset, true);
          int right_i = GetIntFromFile(dataOffset + 4, true);
          if (parentForIndexLookup.Constants.Count() > left_i && left_i >= 0)
            left = parentForIndexLookup.Constants.ElementAt(left_i);
          if (parentForIndexLookup.Constants.Count() > right_i && right_i >= 0)
            right = parentForIndexLookup.Constants.ElementAt(right_i);
          return new ScriptClassCONSConstantEntry[]
          {
            left,
            right
          };
        }
        else if (GetDataSizeFromID() == 4)
        {
          ScriptClassCONSConstantEntry left = null;
          int left_i = GetIntFromFile(dataOffset, true);
          if (parentForIndexLookup.Constants.Count() > left_i && left_i >= 0)
            left = parentForIndexLookup.Constants.ElementAt(left_i);
          return new ScriptClassCONSConstantEntry[]
          {
            left
          };
        }
        else
          return new ScriptClassCONSConstantEntry[] { };
      }
    }
    public string FlatOutIndexLookupsTillStringEntries
    {
      get
      {
        List<ScriptClassCONSConstantEntry> flatting = new List<ScriptClassCONSConstantEntry>();
        flatting.AddRange(DataAsIndexLookup);
        List<ScriptClassCONSConstantEntry> flattingOther = new List<ScriptClassCONSConstantEntry>();
        while (!flatting.All(x => x == null || x is ScriptClassCONSNameEntry || x is ScriptClassCONSRpkRefEntry))
        {
          flattingOther = new List<ScriptClassCONSConstantEntry>();
          foreach (var entry in flatting)
          {
            if (entry == null)
            {
              flattingOther.Add(null);
            }
            else
            {
              if (!(entry is ScriptClassCONSNameEntry || entry is ScriptClassCONSRpkRefEntry))
                flattingOther.AddRange(entry.DataAsIndexLookup);
              else
                flattingOther.Add(entry);
            }
          }
          flatting = flattingOther;
        }
        flattingOther = new List<ScriptClassCONSConstantEntry>();
        string ret = "";
        foreach (var entry in flatting)
        {
          if (entry == null)
          {
            ret += "ERROR_IN_CONST_STRUCTURE_INVALID_INDEX | ";
          }
          else
          {
            if (entry is ScriptClassCONSNameEntry)
              ret += (entry as ScriptClassCONSNameEntry).DataAsString + " | ";
            if (entry is ScriptClassCONSRpkRefEntry)
            {
              ret += (parentForIndexLookup.Constants.ElementAt((entry as ScriptClassCONSRpkRefEntry).RPKnameIndexInConstantTable) as ScriptClassCONSNameEntry).DataAsString
                     + ":0x" + (entry as ScriptClassCONSRpkRefEntry).TypeIdInRPK.ToString("X");
            }
          }
        }
        return ret;
      }
    }

    public ScriptClassCONSConstantEntry(FileCacheHolder file,int offset,ScriptClassCONSChunk parent = null,bool cache = false)
    :base(file,offset,cache)
    {
      parentForIndexLookup = parent;
    }

    public int GetDataSizeFromID()
    {
      switch(ID)
      {
        case 0:
          return GetIntFromFile(dataOffset,true)+4;
        case 4:
          return 4;
        case 7:
        case 3:
        case 5:
          return 8;
        case 2:
          return 0;
      }
      throw new Exception("Unkown const entry in file " + Cache.FileName);
    }
    public virtual int GetSizeInFile()
    {
      return 4 + GetDataSizeFromID();
    }
    public virtual void Save(BinaryWriter bw)
    {
      bw.Write(ID);
      bw.Write(Data);
    }

    public override string ToString()
    {
      var ret = FlatOutIndexLookupsTillStringEntries;
      return "["+ret.Remove(ret.LastIndexOf(" | "))+"]";
    }
  }
}
