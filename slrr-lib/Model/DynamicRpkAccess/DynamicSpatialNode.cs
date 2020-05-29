using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicSpatialNode : DynamicRSDInnerEntryBase
  {
    public int Signature
    {
      get;
      set;
    }
    public int PrecalculatedOffset
    {
      get;
      set;
    }
    public int OriginalOffset
    {
      get;
      private set;
    }
    public List<DynamicNamedSpatialData> DataArray
    {
      get;
      set;
    } = new List<DynamicNamedSpatialData>();

    public DynamicSpatialNode()
    :this(null)
    {
      Signature = 0;
    }
    public DynamicSpatialNode(BinarySpatialNode from = null)
    {
      if (from == null)
        return;
      Signature = from.Signature;
      foreach(var dat in from.DataArray)
      {
        DataArray.Add(new DynamicNamedSpatialData(dat));
      }
      OriginalOffset = from.Offset;
    }

    public IEnumerable<DynamicNamedSpatialData> FlatAllDecendantNamedDatas()
    {
      List<DynamicNamedSpatialData> ret = new List<DynamicNamedSpatialData>();
      ret.AddRange(DataArray);
      List<DynamicSpatialNode> toProc = new List<DynamicSpatialNode>();
      toProc.AddRange(DataArray.SelectMany(x => x.OwnedEntries.Where(y => y is DynamicSpatialNode).Select(y => y as DynamicSpatialNode)));
      while (toProc.Any())
      {
        var currentArray = toProc.Last();
        toProc.RemoveAt(toProc.Count - 1);
        ret.AddRange(currentArray.DataArray);
        toProc.AddRange(currentArray.DataArray.SelectMany(x => x.OwnedEntries.Where(y => y is DynamicSpatialNode).Select(y => y as DynamicSpatialNode)));
      }
      return ret;
    }
    //first argument of the function will be the parent if applicable null otherwise
    public void VisitAllDecendantNamedDatas(Action<DynamicNamedSpatialData, DynamicNamedSpatialData> visiter,DynamicNamedSpatialData explicitFirstParent = null)
    {
      foreach (var namedDat in DataArray.ToList())
      {
        visiter(explicitFirstParent, namedDat);
        namedDat.VisitAllDecendantNamedDatas(visiter);
      }
    }
    //first argument of the function will be the parent if applicable null otherwise int is depth
    public void VisitAllDecendantNamedDatas(Action<DynamicNamedSpatialData, DynamicNamedSpatialData,int> visiter,DynamicNamedSpatialData explicitFirstParent = null, int startDepth = 0)
    {
      foreach (var namedDat in DataArray.ToList())
      {
        visiter(explicitFirstParent, namedDat,startDepth);
        namedDat.VisitAllDecendantNamedDatas(visiter,startDepth+1);
      }
    }
    public void VisitAllDecendantNamedDatas(Func<DynamicNamedSpatialData, DynamicNamedSpatialData,bool> visiter,DynamicNamedSpatialData explicitFirstParent = null)
    {
      foreach (var namedDat in DataArray.ToList())
      {
        if (!visiter(explicitFirstParent, namedDat))
          break;
        namedDat.VisitAllDecendantNamedDatas(visiter);
      }
    }
    public DynamicNamedSpatialData GetClosestContainingBound(DynamicNamedSpatialData dat, bool OnlyNamelessParentAllowed = true)
    {
      DynamicNamedSpatialData ClosestBound = null;
      VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData current) =>
      {
        if (current.UnkownShort1 == 257 && current.OwnedEntries.Count == 1 && current.OwnedEntries.First() is DynamicSpatialNode)
        {
          if (!current.IsOtherContainedWithinBounds(dat))
            return;
          if (current.Name != "" && OnlyNamelessParentAllowed)
            return;
          if (ClosestBound == null)
            ClosestBound = current;
          else if (ClosestBound.VolumeOfBound() > current.VolumeOfBound())
            ClosestBound = current;
        }
      });
      return ClosestBound;
    }
    public DynamicNamedSpatialData GetClosestContainingPosition(DynamicNamedSpatialData dat, bool OnlyNamelessParentAllowed = true)
    {
      DynamicNamedSpatialData ClosestBound = null;
      VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData current) =>
      {
        if (current.UnkownShort1 == 257 && current.OwnedEntries.Count == 1 && current.OwnedEntries.First() is DynamicSpatialNode)
        {
          if (!current.IsOtherPositionInBound(dat) || current.Name != "")
            return;
          if (current.Name != "" && OnlyNamelessParentAllowed)
            return;
          if (ClosestBound == null)
            ClosestBound = current;
          else if (ClosestBound.VolumeOfBound() > current.VolumeOfBound())
            ClosestBound = current;
        }
      });
      return ClosestBound;
    }
    public DynamicNamedSpatialData GetDeepestContainingPosition(DynamicNamedSpatialData dat, bool OnlyNamelessParentAllowed = true)
    {
      DynamicNamedSpatialData ClosestBound = null;
      int winnerDepth = 0;
      VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData current, int depth) =>
      {
        if (current.UnkownShort1 == 257 && current.OwnedEntries.Count == 1 && current.OwnedEntries.First() is DynamicSpatialNode)
        {
          if (!current.IsOtherPositionInBound(dat) || current.Name != "")
            return;
          if (current.Name != "" && OnlyNamelessParentAllowed)
            return;
          if (ClosestBound == null)
            ClosestBound = current;
          else if (winnerDepth < depth)
          {
            ClosestBound = current;
            winnerDepth = depth;
          }
        }
      });
      return ClosestBound;
    }
    public DynamicNamedSpatialData GetDeepestContainingBound(DynamicNamedSpatialData dat, bool OnlyNamelessParentAllowed = true)
    {
      DynamicNamedSpatialData ClosestBound = null;
      int winnerDepth = 0;
      VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData current, int depth) =>
      {
        if (current.UnkownShort1 == 257 && current.OwnedEntries.Count == 1 && current.OwnedEntries.First() is DynamicSpatialNode)
        {
          if (!current.IsOtherContainedWithinBounds(dat) || current.Name != "")
            return;
          if (current.Name != "" && OnlyNamelessParentAllowed)
            return;
          if (ClosestBound == null)
            ClosestBound = current;
          else if (winnerDepth < depth)
          {
            ClosestBound = current;
            winnerDepth = depth;
          }
        }
      });
      return ClosestBound;
    }
    public DynamicNamedSpatialData PushDownNameEntryWithPosition(DynamicNamedSpatialData dat)
    {
      DynamicNamedSpatialData ClosestBound = GetClosestContainingPosition(dat);
      if (ClosestBound != null)
      {
        ClosestBound.GetFirstNamedDataArray().DataArray.Add(dat);
        dat.SuperID = ClosestBound.TypeID;
      }
      else
      {
        DataArray.Add(dat);
      }
      return ClosestBound;
    }
    public DynamicNamedSpatialData PushDownNameEntryWithBound(DynamicNamedSpatialData dat)
    {
      DynamicNamedSpatialData ClosestBound = GetClosestContainingBound(dat);
      if (ClosestBound != null)
      {
        ClosestBound.GetFirstNamedDataArray().DataArray.Add(dat);
        dat.SuperID = ClosestBound.TypeID;
      }
      else
      {
        DataArray.Add(dat);
      }
      return ClosestBound;
    }
    public DynamicNamedSpatialData PushDownDeepNameEntryWithPosition(DynamicNamedSpatialData dat)
    {
      DynamicNamedSpatialData ClosestBound = GetDeepestContainingPosition(dat);
      if (ClosestBound != null)
      {
        ClosestBound.GetFirstNamedDataArray().DataArray.Add(dat);
        dat.SuperID = ClosestBound.TypeID;
      }
      else
      {
        DataArray.Add(dat);
      }
      return ClosestBound;
    }
    public DynamicNamedSpatialData PushDownDeepNameEntryWithBound(DynamicNamedSpatialData dat)
    {
      DynamicNamedSpatialData ClosestBound = GetDeepestContainingBound(dat);
      if (ClosestBound != null)
      {
        ClosestBound.GetFirstNamedDataArray().DataArray.Add(dat);
        dat.SuperID = ClosestBound.TypeID;
      }
      else
      {
        DataArray.Add(dat);
      }
      return ClosestBound;
    }
    public void RemoveDescendantEntry(DynamicNamedSpatialData dat)
    {
      DataArray.Remove(dat);
      VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData current) =>
      {
        if (dat == current)
        {
          if (parent != null)
            parent.RemoveDirectChild(dat);
        }
      });
    }
    public void RemoveDirectChild(DynamicNamedSpatialData dat)
    {
      DataArray.Remove(dat);
    }

    public override int GetSize()
    {
      return DataArray.Sum(x => x.GetSize()) + 8;
    }
    public override void Save(BinaryWriter bw)
    {
      bw.Write(Signature);
      bw.Write((int)DataArray.Count);
      foreach(var dat in DataArray)
      {
        dat.Save(bw);
      }
    }
  }
}
