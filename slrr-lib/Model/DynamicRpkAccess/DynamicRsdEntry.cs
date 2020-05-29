using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicRsdEntry
  {
    public int Size
    {
      get
      {
        int ret = 0;
        foreach(var entr in InnerEntries)
        {
          ret += entr.GetSize();
        }
        return ret;
      }
    }
    public List<DynamicRSDInnerEntryBase> InnerEntries
    {
      get;
      set;
    } = new List<DynamicRSDInnerEntryBase>();
    public List<DynamicRSDInnerEntryBase> HiddenEntries
    {
      get;
      set;
    } = new List<DynamicRSDInnerEntryBase>();

    public DynamicRsdEntry()
    :this(null)
    {

    }
    public DynamicRsdEntry(BinaryRsdEntry from)
    {
      if (from == null)
        return;
      foreach(var entry in from.InnerEntries)
      {
        if (entry is BinaryEXTPInnerEntry)
        {
          InnerEntries.Add(new DynamicEXTPInnerEntry(entry as BinaryEXTPInnerEntry));
        }
        else if (entry is BinaryXCFGInnerEntry)
        {
          InnerEntries.Add(new DynamicXCFGInnerEntry(entry as BinaryXCFGInnerEntry));
        }
        else if (entry is BinaryICFGInnerEntry)
        {
          InnerEntries.Add(new DynamicICFGInnerEntry(entry as BinaryICFGInnerEntry));
        }
        else if (entry is BinarySpatialNode)
        {
          InnerEntries.Add(new DynamicSpatialNode(entry as BinarySpatialNode));
        }
        else if (entry is BinaryRSDInnerEntry)
        {
          InnerEntries.Add(new DynamicRSDInnerEntry(entry as BinaryRSDInnerEntry));
        }
        else if (entry is BinaryStringInnerEntry)
        {
          InnerEntries.Add(new DynamicStringInnerEntry(entry as BinaryStringInnerEntry));
        }
      }
    }

    public int GetSizeIncludingHiddenEntries()
    {
      return Size + GetFlatHiddenEntries().Sum(x => x.GetSize());
    }
    public List<DynamicRSDInnerEntryBase> GetFlatHiddenEntries()
    {
      List<DynamicRSDInnerEntryBase> flatHiddenEntries = new List<DynamicRSDInnerEntryBase>();
      foreach (var innerEntry in HiddenEntries.Union(getRootOwnedEntries()))
      {
        flatHiddenEntries.Add(innerEntry);
        fillFlatHiddenEntries(flatHiddenEntries, innerEntry);
      }
      return flatHiddenEntries;
    }
    public void Save(BinaryWriter bw)
    {
      //hidden entries need precalculated offsets...
      //flatten the hierarchy
      List<DynamicRSDInnerEntryBase> flatHiddenEntries = GetFlatHiddenEntries();/*.OrderBy(
              x =>
        {
          if (x is UnkownNamedDataArray)
            return (x as UnkownNamedDataArray).OriginalOffset;
          if(x is InnerPolyEntry)
            return (x as InnerPolyEntry).OriginalOffset;
          if (x is InnerPhysEntry)
            return (x as InnerPhysEntry).OriginalOffset;
          if (x is RSDInnerEntry)
            return (x as RSDInnerEntry).OriginalOffset;
          return int.MaxValue;
        }).ToList();*/
      int currentOffset = (int)bw.BaseStream.Position+InnerEntries.Sum(x => x.GetSize());
      foreach(var hiddenEntry in flatHiddenEntries)
      {
        if(hiddenEntry is DynamicSpatialNode)
        {
          var hiddenUnkEntry = hiddenEntry as DynamicSpatialNode;
          hiddenUnkEntry.PrecalculatedOffset = currentOffset;
        }
        currentOffset += hiddenEntry.GetSize();
      }
      foreach(var innerEntry in InnerEntries)
      {
        innerEntry.Save(bw);
      }
      foreach (var innerEntry in flatHiddenEntries)
      {
        innerEntry.Save(bw);
      }
    }

    private void fillFlatHiddenEntries(List<DynamicRSDInnerEntryBase> flatHiddenEntries,DynamicRSDInnerEntryBase currentEntry)
    {
      if(currentEntry is DynamicSpatialNode)
      {
        var currentUnkEntry = currentEntry as DynamicSpatialNode;
        foreach(var dat in currentUnkEntry.DataArray)
        {
          foreach(var ownedEntry in dat.OwnedEntries)
          {
            flatHiddenEntries.Add(ownedEntry);
          }
        }
        foreach (var dat in currentUnkEntry.DataArray)
        {
          foreach (var ownedEntry in dat.OwnedEntries)
          {
            fillFlatHiddenEntries(flatHiddenEntries, ownedEntry);
          }
        }
      }
    }
    private IEnumerable<DynamicRSDInnerEntryBase> getRootOwnedEntries()
    {
      List<DynamicRSDInnerEntryBase> ret = new List<DynamicRSDInnerEntryBase>();
      foreach(var innerEntry in InnerEntries)
      {
        if(innerEntry is DynamicSpatialNode)
        {
          var innerUnkEntry = innerEntry as DynamicSpatialNode;
          foreach(var dat in innerUnkEntry.DataArray)
          {
            ret.AddRange(dat.OwnedEntries);
          }
        }
      }
      return ret;
    }
  }
}
