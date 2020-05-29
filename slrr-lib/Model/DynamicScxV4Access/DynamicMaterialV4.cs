using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicMaterialV4
  {
    public static readonly int TypeDefer = 0;

    public int UnkownZero
    {
      get;
      set;
    }
    public int Size
    {
      get
      {
        return 20/*headerSize*/+ Entries.Sum(x => x.Size);
      }
    }
    public int Flags
    {
      get;
      set;
    }
    public int MaterialOpacityRGBA
    {
      get;
      set;
    }
    public int EntriesCount
    {
      get
      {
        return Entries.Count;
      }
    }
    public List<DynamicMaterialV4Entry> Entries
    {
      get;
      set;
    } = new List<DynamicMaterialV4Entry>();

    public DynamicMaterialV4(BinaryMaterialV4 constructFrom = null)
    {
      if (constructFrom == null)
        return;

      UnkownZero = constructFrom.UnkownZero1;
      MaterialOpacityRGBA = constructFrom.UnkownZero2;
      Flags = constructFrom.Flags;
      DynamicMaterialV4MapDefinition mapDef = null;
      foreach(var entry in constructFrom.Entries)
      {
        switch (entry.Type)
        {
          case 2048:
            Entries.Add(new DynamicMaterialV4StringEntry(entry));
            mapDef = null;
            break;
          case 0:
            Entries.Add(new DynamicMaterialV4RGBAEntry(entry));
            mapDef = null;
            break;
          case 256:
            if(mapDef == null)
              Entries.Add(new DynamicMaterialV4FloatEntry(entry));
            mapDef = null;
            break;
          case 1536:
            mapDef = new DynamicMaterialV4MapDefinition(entry as BinaryMaterialV4MapDefinition);
            Entries.Add(mapDef);
            if ((entry as BinaryMaterialV4MapDefinition).FloatWeight == null)
              mapDef = null;
            break;
          case 1280:
            Entries.Add(new DynamicMaterialV4IntEntry(entry));
            mapDef = null;
            break;
          default:
            MessageLog.AddError("UNKOWN MATERIAL ENTRY");
            break;
        }
      }

      if (constructFrom.Size != Size)
        throw new Exception("HeaderWill Mismatch");
    }

    public void SetReflectionStrength(float percent)
    {
      float realPercent = Math.Min(Math.Max(0.0f, percent), 1.0f);
      foreach (var materialEntry in Entries)
      {
        if (materialEntry.TypeEnumed == DynamicMaterialEntryPrettyType.ReflectionMap)
        {
          if ((materialEntry as DynamicMaterialV4MapDefinition).FloatWeight != null)
          {
            (materialEntry as DynamicMaterialV4MapDefinition).FloatWeight.Data = realPercent;
          }
        }
      }
    }
    public bool HasReflection()
    {
      foreach (var materialEntry in Entries)
      {
        if (materialEntry.TypeEnumed == DynamicMaterialEntryPrettyType.ReflectionMap)
        {
          if ((materialEntry as DynamicMaterialV4MapDefinition).FloatWeight != null)
          {
            return true;
          }
        }
      }
      return false;
    }
    public float ReflectionStrength()
    {
      foreach (var materialEntry in Entries)
      {
        if (materialEntry.TypeEnumed == DynamicMaterialEntryPrettyType.ReflectionMap)
        {
          if ((materialEntry as DynamicMaterialV4MapDefinition).FloatWeight != null)
          {
            return (materialEntry as DynamicMaterialV4MapDefinition).FloatWeight.Data;
          }
        }
      }
      return 0;
    }
    public void Save(BinaryWriter bw)
    {
      bw.Write(UnkownZero);
      bw.Write(Size);
      bw.Write(Flags);
      bw.Write(MaterialOpacityRGBA);
      bw.Write((int)Entries.Count + Entries.Count(x => x is DynamicMaterialV4MapDefinition && (x as DynamicMaterialV4MapDefinition).FloatWeight != null));
      foreach(var entry in Entries)
      {
        entry.Save(bw);
      }
    }

    public override string ToString()
    {
      string ret = "";
      foreach (var entry in Entries)
      {
        if (entry.TypeEnumed == DynamicMaterialEntryPrettyType.MaterialName)
          ret += (entry as DynamicMaterialV4StringEntry).Text.Trim();
      }
      if (ret == "")
      {
        foreach (var entry in Entries)
        {
          ret += entry.PrettyName + "|";
        }
      }
      return ret;
    }
  }
}
