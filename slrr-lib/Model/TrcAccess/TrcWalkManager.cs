using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vec3 = System.Windows.Media.Media3D.Vector3D;

namespace SlrrLib.Model
{
  public class TrcWalkManager
  {
    public List<TrcWalk> Walks
    {
      get;
      private set;
    }
    public DynamicRpk Rpk
    {
      get;
      set;
    }

    public static bool RoughEqual(Vec3 p1, Vec3 p2, double epsilon = 0.1)
    {
      return (p1 - p2).LengthSquared < epsilon * epsilon;
    }
    public static TrcWalk GetNextStepAdjacent(TrcWalk adjacent,TrcWalk source)
    {
      var left = adjacent.GetFirstAdjacent();
      var right = adjacent.GetSecondAdjacent();
      if (left == null || left == source)
      {
        //right must win
        if (right == null || right == source)
        {
          //no winner
          MessageLog.AddError("Probably should have been able to find a proper nontriangle adjacent walk");
        }
        else
        {
          return right;
        }
      }
      else if (right == null || right == source)
      {
        //left must win
        if (left == null || left == source)
        {
          //no winner
          MessageLog.AddError("Probably should have been able to find a proper nontriangle adjacent walk");
        }
        else
        {
          return left;
        }
      }
      return null;
    }

    public TrcWalkManager()
    {
      Rpk = new DynamicRpk();
      Walks = new List<TrcWalk>();
    }
    public TrcWalkManager(DynamicRpk rpk)
    {
      this.Rpk = rpk;
      FillWalksFromRpk();
    }

    public void FillWalksFromRpk()
    {
      DynamicResEntry walkNode = null;
      foreach (var res in Rpk.Entries)
      {
        if (res.TypeOfEntry == 8 && res.RSD.InnerEntries.Count == 1 && res.RSD.InnerEntries.First() is DynamicStringInnerEntry)
        {
          var strRsd = res.RSD.InnerEntries.First() as DynamicStringInnerEntry;
          var spl = strRsd.StringData.Split(new char[] { '\r', '\n', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
          if (spl.Count >= 2)
          {
            if (spl[0].ToLower() == "native" && spl[1].ToLower() == "walk")
            {
              walkNode = res;
              break;
            }
          }
        }
      }
      if (walkNode == null)
        return;
      Dictionary<int, TrcWalk> tempDict = new Dictionary<int, TrcWalk>();
      foreach (var spatial in Rpk.SpatialStructs())
      {
        spatial.VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData child, int depth) =>
        {
          if (child.GetFirstRSDEntry() != null)
          {
            if (child.GetIntHexValueFromLineNameInFirstRSD("gametype") == walkNode.TypeID)
            {
              var spl = child.GetStringValueFromLineNameInFirstRSD("params").Split(new char[] { ';', '\r', '\n', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
              int ind = int.Parse(spl[0]);
              tempDict[ind] = new TrcWalk(spl[1]);
              tempDict[ind].SpatialPosition = new Vec3(child.BoundingBoxX, child.BoundingBoxY, child.BoundingBoxZ);
            }
          }
        });
      }
      Walks = tempDict.OrderBy(x => x.Key).Select(x => x.Value).ToList();
      //Fill objects from indices
      if (!Walks.Any())
        return;
      foreach (var walk in Walks)
      {
        foreach (var frst in walk.AdjacencyDirections)
        {
          if (frst.Direction.OtherWalkIndex > -1)
            frst.Direction.OtherWalk = Walks[frst.Direction.OtherWalkIndex - 1];
        }
        foreach (var related in walk.RelatedWalkIndices)
        {
          if (related > -1)
            walk.RelatedWalks.Add(Walks[related - 1]);
        }
        walk.RelatedWalks.RemoveAt(walk.RelatedWalks.Count - 1);//last one is self
      }
    }
    public void UpdateAdjacencyRefsOfWalk(TrcWalk walk)
    {
      foreach (var seg in walk.AdjacencyDirections)
      {
        if (seg.Type == 23)
          continue;
        if (seg.Direction.OtherWalk == null)
          continue;
        foreach (var otherSeg in seg.Direction.OtherWalk.AdjacencyDirections)
        {
          if (otherSeg.Direction.OtherWalk == walk)
            otherSeg.Direction.OtherWalk = null;
        }
      }
      foreach (var seg in walk.AdjacencyDirections)
      {
        if (seg.Type == 23)
          continue;
        foreach (var other in Walks)
        {
          if (other == walk)
            continue;
          bool found = false;
          foreach (var otherSeg in other.AdjacencyDirections)
          {
            if (otherSeg.Type == 23)
              continue;
            if ((RoughEqual(walk.GetGlobalSourcePosition(seg), other.GetGlobalSourcePosition(otherSeg))
                 && RoughEqual(walk.GetGlobalTargetPosition(seg), other.GetGlobalTargetPosition(otherSeg)))
                ||
                (RoughEqual(walk.GetGlobalTargetPosition(seg), other.GetGlobalSourcePosition(otherSeg))
                 && RoughEqual(walk.GetGlobalSourcePosition(seg), other.GetGlobalTargetPosition(otherSeg))))
            {
              found = true;
              seg.Direction.OtherWalk = other;
              otherSeg.Direction.OtherWalk = walk;
              break;
            }
          }
          if (found)
            break;
        }
      }
    }
    public void FillIndicesFromWalks()
    {
      foreach (var walk in Walks)
      {
        foreach (var frst in walk.AdjacencyDirections)
        {
          frst.Direction.OtherWalkIndex = Walks.IndexOf(frst.Direction.OtherWalk)+1;
          if (frst.Direction.OtherWalkIndex == 0)
            frst.Direction.OtherWalkIndex = -1;
        }
        walk.RelatedWalkIndices.Clear();
        foreach (var related in walk.RelatedWalks)
        {
          walk.RelatedWalkIndices.Add(Walks.IndexOf(related)+1);
        }
        walk.RelatedWalkIndices.Add(Walks.IndexOf(walk)+1);
      }
    }
    public void FillFillableValues()
    {
      foreach (var wlk in Walks)
      {
        foreach (var adj in wlk.AdjacencyDirections)
        {
          if (adj.Type == 23)
          {
            adj.Direction.X1 = (float)adj.ControlPoints.First().Position.X;
            adj.Direction.Y1 = (float)adj.ControlPoints.First().Position.Y;
            adj.Direction.X2 = (float)adj.ControlPoints.Last().Position.X;
            adj.Direction.Y2 = (float)adj.ControlPoints.Last().Position.Y;
          }
          if (adj.Type == 21 && adj.Direction.OtherWalk != null)
            adj.Type = 22;
          if (adj.Type == 22 && adj.Direction.OtherWalk == null)
            adj.Type = 21;
        }
      }
      foreach (var wlk in Walks)
      {
        List<TrcWalk> newRelatedNodes = new List<TrcWalk>();
        var left = wlk.GetSecondAdjacent();
        TrcWalk source = wlk;
        if (left != null)
        {
          newRelatedNodes.Add(left);
          while (left.IsTriangleLike(1))
          {
            var tmp = GetNextStepAdjacent(left,source);
            source = left;
            left = tmp;
            if (left == null)
              break;
            newRelatedNodes.Add(left);
          }
        }
        var right = wlk.GetFirstAdjacent();
        source = wlk;
        if (right != null)
        {
          newRelatedNodes.Add(right);
          while (right.IsTriangleLike(1))
          {
            var tmp = GetNextStepAdjacent(right,source);
            source = right;
            right = tmp;
            if (right == null)
              break;
            newRelatedNodes.Add(right);
          }
        }
        if (wlk.RelatedWalks.Count != newRelatedNodes.Count)
          MessageLog.AddMessage("wlk.RelatedWalks.Count != newRelatedNodes.Count");
        wlk.RelatedWalks = newRelatedNodes;

      }
    }
    public void ReBakeWalks(string rpkFnam, float spatialMargin = 5)
    {
      var nativeWalk = getWalkDefFromRpk();
      foreach (var res in Rpk.Entries)
      {
        foreach (var rsd in res.RSD.InnerEntries)
        {
          if (rsd is SlrrLib.Model.DynamicSpatialNode)
          {
            var nameArr = rsd as SlrrLib.Model.DynamicSpatialNode;
            nameArr.VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData child) =>
            {
              int gameType = child.GetIntHexValueFromLineNameInFirstRSD("gametype");
              if (gameType == nativeWalk.TypeID)
              {
                if (parent != null)
                  parent.RemoveDirectChild(child);
                else
                  nameArr.RemoveDirectChild(child);
              }
            });
          }
        }
      }
      var topSpatialStruct = Rpk.SpatialStructs().First();
      var spatialRes = Rpk.Entries.First(x => x.RSD.InnerEntries.Any(y => y is SlrrLib.Model.DynamicSpatialNode));
      if (spatialMargin < 0.1f)
        spatialMargin = 0.1f;

      FillFillableValues();
      FillIndicesFromWalks();
      MessageLog.AddMessage("Walk indices filled");
      var TypeIDs = Rpk.GetFreeTypeIDsIncludingHiddenEntries(Walks.Count);
      int currTypeID = 0;
      Random rnd = new Random();
      foreach(var walk in Walks)
      {
        MessageLog.AddMessage("Baking walk "+currTypeID.ToString());
        DynamicNamedSpatialData topush = new DynamicNamedSpatialData();
        walk.RecalcPosition();
        if (!walk.HasType23Adjacency())
        {
          topush.BoundingBoxX = (float)walk.GlobalPavementPos.X;
          topush.BoundingBoxY = (float)walk.GlobalPavementPos.Z+1.0f;
          topush.BoundingBoxZ = (float)walk.GlobalPavementPos.Y;
        }
        if (Math.Abs(walk.Slope1 - 0.0f) < 0.00000001f)
          walk.Slope1 = 0;
        if (Math.Abs(walk.Slope2 - 0.0f) < 0.00000001f)
          walk.Slope2 = 0;
        topush.BoundingBoxX = (float)walk.SpatialPosition.X;
        topush.BoundingBoxY = (float)walk.SpatialPosition.Y;
        topush.BoundingBoxZ = (float)walk.SpatialPosition.Z;
        topush.BoundingBoxHalfWidthX = 5.0f;
        topush.BoundingBoxHalfWidthY = 5.0f;
        topush.BoundingBoxHalfWidthZ = 5.0f;
        topush.TypeID = TypeIDs[currTypeID];
        currTypeID++;
        topush.Name = "walk" + currTypeID.ToString();
        DynamicSpatialNode emptyArr = new DynamicSpatialNode();
        DynamicRSDInnerEntry rsd = new DynamicRSDInnerEntry();
        topush.OwnedEntries.Add(emptyArr);
        topush.OwnedEntries.Add(rsd);
        topush.UnkownShort1 = 0x101;
        topush.UnkownIsParentCompatible = 0.0f;
        rsd.StringData = "gametype 0x" + nativeWalk.TypeID.ToString("X8") + "\r\n"
                         + "params " + currTypeID.ToString() + ";" + walk.AsEncoded()+"\r\n";
        topush.SuperID = spatialRes.TypeID;
        topSpatialStruct.PushDownDeepNameEntryWithBound(topush);
      }
      Rpk.SaveAs(rpkFnam, true, "_BAK_WALK_");
    }

    private bool matchingNativeWalk(string rsdStr)
    {
      var spl = rsdStr.Split(new char[] { '\r', '\n', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
      if (spl.Length >= 2)
      {
        return spl[0].ToLower() == "native" && spl[1].ToLower() == "walk";
      }
      return false;
    }
    private DynamicResEntry getWalkDefFromRpk()
    {
      var walkDef = Rpk.Entries.FirstOrDefault(x => x.RSD.InnerEntries.Count == 1
                    && x.RSD.InnerEntries.First() is SlrrLib.Model.DynamicStringInnerEntry
                    && matchingNativeWalk((x.RSD.InnerEntries.First() as SlrrLib.Model.DynamicStringInnerEntry).StringData));
      if (walkDef == null && Rpk != null)
      {
        walkDef = new SlrrLib.Model.DynamicResEntry();
        int freeTypeID = Rpk.GetFirstFreeTypeIDIncludingHiddenEntries();
        if (freeTypeID == -1)
          throw new Exception("There is no more room in the rpk to add the needed native walk ref");
        walkDef.SuperID = Rpk.GetOrAddExternalRefIndexOfRPK("system.rpk") << 16 | 0x00000015;
        walkDef.TypeID = freeTypeID;
        walkDef.Alias = "walk";
        walkDef.AdditionalType = 0;
        walkDef.IsParentCompatible = 1.0f;
        walkDef.TypeOfEntry = 8;
        walkDef.RSD = new SlrrLib.Model.DynamicRsdEntry();
        walkDef.RSD.InnerEntries.Add(new SlrrLib.Model.DynamicStringInnerEntry("native walk"));
        Rpk.Entries.Add(walkDef);
      }
      return walkDef;
    }
  }
}
