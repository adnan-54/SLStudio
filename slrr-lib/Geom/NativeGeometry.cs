using SlrrLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
  public class NativeGeometry : RpkSpatialGeometry
  {
    private float scaleMitigation = 100.0f;
    private int walkSmooth = 0;
    private Vector3D walkNormal = new Vector3D(1, 0, 0);
    private List<DynamicNamedSpatialData> dirtyList = new List<DynamicNamedSpatialData>();
    private Dictionary<string, DynamicRpk> cachedRpks = new Dictionary<string, DynamicRpk>();
    private Dictionary<int, List<NativeGeometryScxContext>> dynamicNamedSpatialDataTypeIDToScxFilename = new Dictionary<int, List<NativeGeometryScxContext>>();
    private List<DynamicNamedSpatialData> highLightedDynamicNamedSpatialDatas = new List<DynamicNamedSpatialData>();
    private const byte nonSelectedAlpha = 60;
    private const byte nonSelectedAlpha_Race = 100;

    public SolidColorBrush UnresolvedColor
    {
      get;
      private set;
    } = new SolidColorBrush(Color.FromArgb(nonSelectedAlpha, Colors.DarkRed.R, Colors.DarkRed.G, Colors.DarkRed.B));
    public SolidColorBrush ResolvedColor
    {
      get;
      private set;
    } = new SolidColorBrush(Color.FromArgb(nonSelectedAlpha, Colors.DarkGreen.R, Colors.DarkGreen.G, Colors.DarkGreen.B));
    public SolidColorBrush UnresolvedColorHighLight
    {
      get;
      private set;
    } = Brushes.Red;
    public SolidColorBrush ResolvedColorHighLight
    {
      get;
      private set;
    } = Brushes.Green;
    public DiffuseMaterial UnresolvedHighLight
    {
      get;
      private set;
    } = new DiffuseMaterial();
    public DiffuseMaterial ResolvedHighLight
    {
      get;
      private set;
    } = new DiffuseMaterial();
    public DiffuseMaterial UnresolvedMaterial
    {
      get;
      private set;
    } = new DiffuseMaterial();
    public DiffuseMaterial ResolvedMaterial
    {
      get;
      private set;
    } = new DiffuseMaterial();
    public DiffuseMaterial TopHighLightMaterial
    {
      get;
      private set;
    } = new DiffuseMaterial();
    public Dictionary<DynamicNamedSpatialData, GeometryModel3D> DynamicNamedSpatialDataToGeom
    {
      get;
      private set;
    } = new Dictionary<DynamicNamedSpatialData, GeometryModel3D>();
    public double AddingBoundMargin
    {
      get;
      set;
    } = 0.2;
    public bool AutoResolve
    {
      get;
      set;
    } = false;

    public NativeGeometry(DynamicRpk rpkDat,string rpkFilename)
    :base(rpkDat,rpkFilename)
    {
      UnresolvedHighLight.Brush = UnresolvedColorHighLight;
      ResolvedHighLight.Brush = ResolvedColorHighLight;
      UnresolvedMaterial.Brush = UnresolvedColor;
      ResolvedMaterial.Brush = ResolvedColor;
      TopHighLightMaterial.Brush = Brushes.Magenta;
    }
    public NativeGeometry(string rpkFilename)
    :base(rpkFilename)
    {
      UnresolvedHighLight.Brush = UnresolvedColorHighLight;
      ResolvedHighLight.Brush = ResolvedColorHighLight;
      UnresolvedMaterial.Brush = UnresolvedColor;
      ResolvedMaterial.Brush = ResolvedColor;
      TopHighLightMaterial.Brush = Brushes.Magenta;
    }

    public void RemoveNamedDataFromRpk(DynamicNamedSpatialData data)
    {
      DynamicNamedSpatialDataToGeom.Remove(data);
      if (dirtyList.Contains(data))
      {
        dirtyList.Remove(data);
        return;
      }
      foreach (var nameArr in Rpk.SpatialStructs())
      {
        nameArr.VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData child) =>
        {
          if (child == data)
          {
            if (parent != null)
              SlrrLib.Model.MessageLog.AddMessage(child.TypeID.ToString("X8") + " V: " + parent.VolumeOfBound().ToString("F4"));
            else
              SlrrLib.Model.MessageLog.AddMessage(child.TypeID.ToString("X8"));
            if (parent != null)
              parent.RemoveDirectChild(child);
            else
              nameArr.RemoveDirectChild(child);
          }
        });
      }
    }
    public DynamicNamedSpatialData AddNamedDataFromDef(NativeGeometryObjResEntryContext datDef,Vector3D pos,Vector3D rot)
    {
      DynamicNamedSpatialData toad = new DynamicNamedSpatialData();
      toad.TypeID = 0;
      toad.Name = datDef.res.Alias;
      toad.SuperID = 0;
      toad.OwnedEntries = new List<DynamicRSDInnerEntryBase>();
      toad.OwnedEntries.Add(new DynamicSpatialNode());
      var rsdEntry = new DynamicRSDInnerEntry();
      rsdEntry.StringData = "gametype 0x" + datDef.asExternalTypeID.ToString("x8") + "\r\nparams "
                            + pos.X.ToString("F3") + "," + pos.Y.ToString("F3") + "," + pos.Z.ToString("F3") + ","
                            + rot.X.ToString("F3") + "," + rot.Y.ToString("F3") + "," + rot.Z.ToString("F3") + "\r\n";
      toad.OwnedEntries.Add(rsdEntry);
      toad.UnkownShort1 = 0x00002101;
      toad.BoundingBoxX = (float)pos.X;
      toad.BoundingBoxY = (float)pos.Y;
      toad.BoundingBoxZ = (float)pos.Z;
      toad.UnkownIsParentCompatible = 1.0f;
      if (!dynamicNamedSpatialDataTypeIDToScxFilename.ContainsKey(datDef.asExternalTypeID))
      {
        dynamicNamedSpatialDataTypeIDToScxFilename[datDef.asExternalTypeID]
          = allHighLodScxContexts(CfgOfTypeID(datDef.asExternalTypeID, SlrrRoot, Rpk), SlrrRoot);
      }

      var geom = new GeometryModel3D();
      var scxContexts = dynamicNamedSpatialDataTypeIDToScxFilename[datDef.asExternalTypeID];
      MeshGeometry3D mesh = new MeshGeometry3D();
      foreach (var scxCon in scxContexts)
        addScxContextToMesh(mesh, scxCon);
      geom.Geometry = mesh;
      geom.Material = ResolvedMaterial;
      DynamicNamedSpatialDataToGeom[toad] = geom;
      toad.BoundingBoxHalfWidthX = (float)((geom.Bounds.SizeX / 2.0) + AddingBoundMargin);
      toad.BoundingBoxHalfWidthY = (float)((geom.Bounds.SizeY / 2.0) + AddingBoundMargin);
      toad.BoundingBoxHalfWidthZ = (float)((geom.Bounds.SizeZ / 2.0) + AddingBoundMargin);
      dirtyList.Add(toad);
      SetTransformsOf(toad);
      return toad;
    }
    public void SetNewPositionForData(DynamicNamedSpatialData datDef, Vector3D pos, Vector3D rot)
    {
      if (!dirtyList.Contains(datDef))
      {
        foreach (var nameArr in Rpk.SpatialStructs())
        {
          nameArr.VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData child) =>
          {
            if (child == datDef)
            {
              if (parent != null)
                SlrrLib.Model.MessageLog.AddMessage(child.TypeID.ToString("X8") + " V: " + parent.VolumeOfBound().ToString("F4"));
              else
                SlrrLib.Model.MessageLog.AddMessage(child.TypeID.ToString("X8"));
              if (parent != null)
                parent.RemoveDirectChild(child);
              else
                nameArr.RemoveDirectChild(child);
            }
          });
        }
        dirtyList.Add(datDef);
      }
      datDef.SetPositionFromParamsLineFromFirstRSD(pos);
      datDef.SetYawPitchRollFromParamsLineFromFirstRSD(rot);
      SetTransformsOf(datDef);
    }
    public void SetTypeIDDirty(int ExternalTypeID)
    {
      foreach (var named in DynamicNamedSpatialDataToGeom.Keys)
      {
        if (named.GetIntHexValueFromLineNameInFirstRSD("gametype") == ExternalTypeID)
          SetNewPositionForData(named, named.GetPositionFromParamsLineFromFirstRSD(), named.GetYawPitchRollFromParamsLineFromFirstRSD());
      }
    }
    public void SaveRpk(string rpkFnam)
    {
      //rebakeDirtyList
      var spatial = Rpk.SpatialStructs().First();
      foreach (var dirty in dirtyList)
      {
        ResolveVisualsOf(dirty);
        bool unkownModel = (dynamicNamedSpatialDataTypeIDToScxFilename[dirty.GetIntHexValueFromLineNameInFirstRSD("gametype")].All(x => !System.IO.File.Exists(SlrrRoot + "\\" + x.scxFnam)));
        var resolved = DynamicNamedSpatialDataToGeom[dirty];
        YprRotation3D ypr = new YprRotation3D(dirty.GetYawPitchRollFromParamsLineFromFirstRSD(), false);
        ypr.yAxis = new Vector3D(0, 1, 0);
        ypr.pAxis = new Vector3D(1, 0, 0);
        var rotateBounds = ypr.TransformValues.TransformBounds(resolved.Bounds);
        if (unkownModel)
        {
          rotateBounds.SizeX += 20.0;
          rotateBounds.SizeY += 20.0;
          rotateBounds.SizeZ += 20.0;
        }
        dirty.BoundingBoxHalfWidthX = (float)(rotateBounds.SizeX/2.0 + AddingBoundMargin);
        dirty.BoundingBoxHalfWidthY = (float)(rotateBounds.SizeY/2.0 + AddingBoundMargin);
        dirty.BoundingBoxHalfWidthZ = (float)(rotateBounds.SizeZ/2.0 + AddingBoundMargin);
        var pos = dirty.GetPositionFromParamsLineFromFirstRSD();
        dirty.BoundingBoxX = (float)pos.X;
        dirty.BoundingBoxY = (float)pos.Y+(dirty.BoundingBoxHalfWidthY-(float)AddingBoundMargin);
        dirty.BoundingBoxZ = (float)pos.Z;
        dirty.TypeID = Rpk.GetFirstFreeTypeIDIncludingHiddenEntries();
        spatial.PushDownDeepNameEntryWithBound(dirty);
      }
      Rpk.FixBounds();
      dirtyList.Clear();
      Rpk.SaveAs(rpkFnam);
    }
    public void ClearHighLight()
    {
      foreach (var named in highLightedDynamicNamedSpatialDatas)
      {
        if (!DynamicNamedSpatialDataToGeom.ContainsKey(named))
          continue;
        if (dynamicNamedSpatialDataTypeIDToScxFilename.ContainsKey(named.GetIntHexValueFromLineNameInFirstRSD("gametype")))
        {
          DynamicNamedSpatialDataToGeom[named].Material = ResolvedMaterial;
          DynamicNamedSpatialDataToGeom[named].BackMaterial = ResolvedMaterial;
        }
        else
        {
          DynamicNamedSpatialDataToGeom[named].Material = UnresolvedMaterial;
          DynamicNamedSpatialDataToGeom[named].BackMaterial = UnresolvedMaterial;
        }
      }
      highLightedDynamicNamedSpatialDatas.Clear();
    }
    public void TopHighLightDynamicNamedSpatialData(DynamicNamedSpatialData named)
    {
      if (named == null)
        return;
      if (AutoResolve)
        ResolveVisualsOf(named);
      highLihgtSingleNamed(named);
      DynamicNamedSpatialDataToGeom[named].BackMaterial = TopHighLightMaterial;
      DynamicNamedSpatialDataToGeom[named].Material = TopHighLightMaterial;
    }
    public void ResolveVisualsOf(DynamicNamedSpatialData obj)
    {
      if (obj == null)
        return;
      var geom = DynamicNamedSpatialDataToGeom[obj];
      if (obj.UnkownShort1 == 0x0101 && (obj.Name.Contains("walk")))
      {
        var newGeom = geomFromWalkString(obj.GetStringValueFromLineNameInFirstRSD("params").Split(';').Last().Trim());
        geom.Geometry = newGeom.Geometry;
        SetTransformsOf(obj);
        return;
      }
      if (!dynamicNamedSpatialDataTypeIDToScxFilename.ContainsKey(obj.GetIntHexValueFromLineNameInFirstRSD("gametype")))
      {
        dynamicNamedSpatialDataTypeIDToScxFilename[obj.GetIntHexValueFromLineNameInFirstRSD("gametype")]
          = allHighLodScxContexts(CfgOfTypeID(obj.GetIntHexValueFromLineNameInFirstRSD("gametype"), SlrrRoot, Rpk), SlrrRoot);
      }
      var scxContexts = dynamicNamedSpatialDataTypeIDToScxFilename[obj.GetIntHexValueFromLineNameInFirstRSD("gametype")];

      MeshGeometry3D mesh = new MeshGeometry3D();
      foreach (var scxCon in scxContexts)
        addScxContextToMesh(mesh, scxCon);

      geom.Geometry = mesh;
      SetTransformsOf(obj);
    }
    public List<NativeGeometryObjResEntryContext> AllPossibleObjects()
    {
      List<NativeGeometryObjResEntryContext> ret = new List<NativeGeometryObjResEntryContext>();
      addPossibleObjects(ret, Rpk, System.IO.Path.GetFileNameWithoutExtension(rpkFilename) + " ",0);
      int extrefPart = 1;
      foreach (var extref in Rpk.ExternalReferences)
      {
        var rpkFnam = SlrrRoot.ToLower() + "\\" + extref.ToLower();
        if (!System.IO.File.Exists(rpkFnam))
          continue;
        if (!cachedRpks.ContainsKey(rpkFnam))
        {
          cachedRpks[rpkFnam] = new DynamicRpk(new BinaryRpk(rpkFnam, true));
        }
        addPossibleObjects(ret, cachedRpks[rpkFnam], System.IO.Path.GetFileNameWithoutExtension(rpkFnam) + " ", extrefPart << 16);
        extrefPart++;
      }
      return ret;
    }
    public NativeGeometryCfgContext CfgOfTypeID(int typeID,string slrrRootDir,DynamicRpk localRpk)
    {
      if (typeID == -1)
        return null;
      if ((typeID & 0xFFFF0000) != 0)
      {
        var rpkFnam = slrrRootDir.ToLower() + "\\" + localRpk.ExternalReferences[(typeID >> 16)-1].ToLower();
        if (!cachedRpks.ContainsKey(rpkFnam))
        {
          cachedRpks[rpkFnam] = new DynamicRpk(new BinaryRpk(rpkFnam, true));
        }
        return CfgOfTypeID(typeID & 0x0000FFFF, slrrRootDir, cachedRpks[rpkFnam]);
      }
      var res = localRpk.Entries.FirstOrDefault(x => x.TypeID == typeID);
      if (res == null)
        return null;
      if (res.TypeOfEntry == 8)
      {
        var rsd = res.RSD.InnerEntries.First();
        if (rsd is DynamicStringInnerEntry)
        {
          var strRsd = rsd as DynamicStringInnerEntry;
          var spl = strRsd.StringData.Split(new char[] { '\r', '\n', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
          var fnam = spl.FirstOrDefault(x => x.ToLower().EndsWith(".cfg"));
          if (fnam == null)
            return null;
          return new NativeGeometryCfgContext(new Cfg(slrrRootDir + "\\" + fnam), localRpk,res);
        }
        if (rsd is DynamicICFGInnerEntry)
        {
          var icfgRsd = rsd as DynamicICFGInnerEntry;
          if (icfgRsd.DataList.Count < 2)
            return null;
          return new NativeGeometryCfgContext(new Cfg("", icfgRsd.DataList[1]),localRpk,res);
        }
        if (rsd is DynamicXCFGInnerEntry)
        {
          var xcfgRsd = rsd as DynamicXCFGInnerEntry;
          return new NativeGeometryCfgContext(new Cfg(slrrRootDir + "\\" + xcfgRsd.CfgReferenceValue),localRpk,res);
        }
      }
      return null;
    }
    public void UnresolveGeneratedVisualsOf(DynamicNamedSpatialData obj)
    {
      if (DynamicNamedSpatialDataToGeom.ContainsKey(obj))
      {
        DynamicNamedSpatialDataToGeom[obj].Geometry = unresolvedBoxMeshAtPos();
        SetTransformsOf(obj);
        ReenforceHighlights();
      }
    }
    public override void GenerateVisuals()
    {
      DynamicNamedSpatialDataToGeom.Clear();
      foreach (var spatial in Rpk.SpatialStructs())
      {
        spatial.VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData child) =>
        {
          if (child.UnkownShort1 == 0x2101)
          {
            DynamicNamedSpatialDataToGeom[child] = unresolvedBoxAtPos();
            SetTransformsOf(child);
          }
        });
      }
    }
    public void SetTransformsOf(DynamicNamedSpatialData named)
    {
      Transform3DGroup transform3DGroup = new Transform3DGroup();
      var yprTrans = new YprRotation3D(named.GetYawPitchRollFromParamsLineFromFirstRSD(), false)
      {
        yAxis = new Vector3D(0, 1, 0),
        pAxis = new Vector3D(1, 0, 0),
        rAxis = new Vector3D(0, 0, 1)
      };
      foreach (var trns in yprTrans.TransformValues.Children)
      {
        transform3DGroup.Children.Add(trns);
      }
      transform3DGroup.Children.Add(new TranslateTransform3D(named.GetPositionFromParamsLineFromFirstRSD()));
      DynamicNamedSpatialDataToGeom[named].Transform = transform3DGroup;
    }
    public List<GeometryModel3D> CloseVisuals(float distance, Vector3D pos)
    {
      List<GeometryModel3D> ret = new List<GeometryModel3D>();
      foreach (var entry in DynamicNamedSpatialDataToGeom)
      {
        if ((entry.Key.GetPositionFromParamsLineFromFirstRSD() - pos).Length < distance)
          ret.Add(entry.Value);
      }
      return ret;
    }
    public DynamicNamedSpatialData GetClosestEntity(Vector3D pos)
    {
      float minDist = float.MaxValue;
      DynamicNamedSpatialData ret = null;
      float dist = float.MaxValue;
      foreach (var objLst in DynamicNamedSpatialDataToGeom)
      {
        var named = objLst.Key;
        dist = (float)(named.GetPositionFromParamsLineFromFirstRSD() - pos).Length;
        if (dist < minDist)
        {
          minDist = dist;
          ret = objLst.Key;
        }
      }
      return ret;
    }
    public void ReenforceHighlights()
    {
      var lstCpy = highLightedDynamicNamedSpatialDatas.ToList();
      highLightedDynamicNamedSpatialDatas.Clear();
      foreach (var obj in lstCpy)
      {
        highLihgtSingleNamed(obj);
      }
    }

    private void highLihgtSingleNamed(DynamicNamedSpatialData named)
    {
      highLightedDynamicNamedSpatialDatas.Add(named);
      if (dynamicNamedSpatialDataTypeIDToScxFilename.ContainsKey(named.GetIntHexValueFromLineNameInFirstRSD("gametype")))
      {
        DynamicNamedSpatialDataToGeom[named].Material = ResolvedHighLight;
        DynamicNamedSpatialDataToGeom[named].BackMaterial = ResolvedHighLight;
      }
      else
      {
        DynamicNamedSpatialDataToGeom[named].Material = UnresolvedHighLight;
        DynamicNamedSpatialDataToGeom[named].BackMaterial = UnresolvedHighLight;
      }
    }
    private GeometryModel3D unresolvedBoxAtPos()
    {
      GeometryModel3D ret = new GeometryModel3D();
      MeshGeometry3D mesh = new MeshGeometry3D();
      addUShape(mesh, -boxDiff, boxDiff, boxWidth, boxWidth, (float)boxWidth);
      ret.Material = UnresolvedMaterial;
      ret.Geometry = mesh;
      return ret;
    }
    private MeshGeometry3D unresolvedBoxMeshAtPos()
    {
      MeshGeometry3D mesh = new MeshGeometry3D();
      addUShape(mesh, -boxDiff, boxDiff, boxWidth, boxWidth, (float)boxWidth);
      return mesh;
    }
    private void addPossibleObjects(List<NativeGeometryObjResEntryContext> lst, DynamicRpk rpk,string prefix,int externalRefPart)
    {
      foreach (var res in rpk.Entries)
      {
        if (res.TypeOfEntry == 8)
        {
          NativeGeometryObjResEntryContext toad = new NativeGeometryObjResEntryContext(res,rpk,new Vector3D(),new Vector3D());
          toad.prefix = prefix;
          toad.asExternalTypeID = externalRefPart | res.TypeID;
          lst.Add(toad);
        }
      }
    }
    private NativeGeometryObjResEntryContext resAndResolve(int typeID, DynamicRpk rpk, string slrrRoot,Vector3D pos,Vector3D ypr)
    {
      if (typeID == -1)
      {
        return new NativeGeometryObjResEntryContext(null,rpk,pos,ypr);
      }
      if ((typeID & 0xFFFF0000) != 0)
      {
        var rpkFnam = slrrRoot.ToLower() + "\\" + rpk.ExternalReferences[(typeID >> 16)-1].ToLower();
        if (!cachedRpks.ContainsKey(rpkFnam))
        {
          cachedRpks[rpkFnam] = new DynamicRpk(new BinaryRpk(rpkFnam, true));
        }
        return resAndResolve(typeID & 0x0000FFFF, cachedRpks[rpkFnam],slrrRoot,pos,ypr);
      }
      return new NativeGeometryObjResEntryContext(rpk.Entries.First(x => x.TypeID == typeID),rpk,pos,ypr);
    }
    private List<NativeGeometryScxContext> allHighLodScxContexts(NativeGeometryCfgContext objCfg,string slrrRoot)
    {
      List<NativeGeometryObjTypeIDContext> RenderTypeIDs = new List<NativeGeometryObjTypeIDContext>();
      if (objCfg.cfg.LinesWithName("deformable") != null)
      {
        foreach (var ln in objCfg.cfg.LinesWithName("deformable"))
        {
          if (ln.Tokens.Count > 1)
            RenderTypeIDs.Add(new NativeGeometryObjTypeIDContext(ln.Tokens[1].ValueAsHexInt, new Vector3D(), new Vector3D()));
        }
      }
      int maxLod = 0;
      if(objCfg.cfg.Lines.Any(x => x.NameStr == "lod"))
        maxLod = objCfg.cfg.Lines.Where(x => x.NameStr == "lod").Max(x => x.Tokens[1].ValueAsInt);
      var allRenders = objCfg.cfg.GetMultiLineStructs("render", new string[] { "rendertypes", "colors", "flags", "lod" });
      foreach (var render in allRenders)
      {
        var lodLine = render.FirstOrDefault(x => x.NameStr == "lod");
        if (lodLine == null || lodLine.Tokens[1].ValueAsInt == maxLod)
        {
          var renderLine = render.First(x => x.NameStr == "render");
          if (renderLine.Tokens.Count > 5 && renderLine.Tokens[5].IsValueFloat)
          {
            RenderTypeIDs.Add(new NativeGeometryObjTypeIDContext(renderLine.Tokens[1].ValueAsHexInt,
                              new Vector3D(renderLine.Tokens[2].ValueAsFloat, renderLine.Tokens[3].ValueAsFloat, renderLine.Tokens[4].ValueAsFloat),
                              new Vector3D(renderLine.Tokens[5].ValueAsFloat, renderLine.Tokens[6].ValueAsFloat, renderLine.Tokens[7].ValueAsFloat)));
          }
          else if (renderLine.Tokens.Count > 2 && renderLine.Tokens[2].IsValueFloat)
          {
            RenderTypeIDs.Add(new NativeGeometryObjTypeIDContext(renderLine.Tokens[1].ValueAsHexInt,
                              new Vector3D(renderLine.Tokens[2].ValueAsFloat, renderLine.Tokens[3].ValueAsFloat, renderLine.Tokens[4].ValueAsFloat),
                              new Vector3D()));
          }
          else
          {
            RenderTypeIDs.Add(new NativeGeometryObjTypeIDContext(renderLine.Tokens[1].ValueAsHexInt,new Vector3D(),new Vector3D()));
          }
        }
      }
      var renderRess = RenderTypeIDs.Select(x => resAndResolve(x.typeID, objCfg.rpk, slrrRoot, x.pos, x.ypr));
      var meshRess = renderRess.Select(x => resAndResolve(x.GetMehsTypeID(), x.rpk, slrrRoot, x.pos, x.ypr));
      return meshRess.Select(x => new NativeGeometryScxContext(x.GetSourcefile(), x.pos, x.ypr)).ToList();
    }
    private void addScxContextToMesh(MeshGeometry3D mesh,NativeGeometryScxContext scxCon)
    {
      Scx scx = null;
      if (System.IO.File.Exists(SlrrRoot + "\\" + scxCon.scxFnam))
        scx = Scx.ConstructScx(SlrrRoot + "\\" + scxCon.scxFnam,true);
      else
        scx = Scx.ConstructScx("QuestionMark.SCX",true);
      YprRotation3D ypr = new YprRotation3D(scxCon.ypr, false);
      ypr.yAxis = new Vector3D(0, 1, 0);
      ypr.pAxis = new Vector3D(1, 0, 0);
      int indicesOffset = mesh.Positions.Count;
      if (scx is BinaryScxV3)
      {
        var scx3 = scx as BinaryScxV3;
        foreach (var v3mesh in scx3.Models)
        {
          indicesOffset = mesh.Positions.Count;
          foreach (var vert in v3mesh.VertexDatas)
          {
            var vertPos = new Vector3D(vert.VertexCoordX, vert.VertexCoordY, vert.VertexCoordZ);
            vertPos = ypr.TransformValues.Transform(vertPos);
            mesh.Positions.Add(new Point3D((vertPos.X / scaleMitigation) + scxCon.pos.X,
                                           (vertPos.Y / scaleMitigation) + scxCon.pos.Y,
                                           (vertPos.Z / scaleMitigation) + scxCon.pos.Z));
            mesh.Normals.Add(new Vector3D(vert.VertexNormalX, vert.VertexNormalY, vert.VertexNormalZ));
            mesh.TextureCoordinates.Add(new Point(vert.UVChannel1X, vert.UVChannel1Y));
          }
          foreach (var ind in v3mesh.VertexIndices)
          {
            mesh.TriangleIndices.Add(indicesOffset + ind);
          }
        }
      }
      else if (scx is BinaryScxV4)
      {
        var scx4 = new DynamicScxV4(scx as BinaryScxV4);
        foreach (var v4mesh in scx4.Meshes)
        {
          indicesOffset = mesh.Positions.Count;
          foreach (var vert in v4mesh.VertexData.VertexDataList)
          {
            var vertPos = new Vector3D(vert.Position.X, vert.Position.Y, vert.Position.Z);
            vertPos = ypr.TransformValues.Transform(vertPos);
            mesh.Positions.Add(new Point3D((vertPos.X / scaleMitigation) + scxCon.pos.X,
                                           (vertPos.Y / scaleMitigation) + scxCon.pos.Y,
                                           (vertPos.Z / scaleMitigation) + scxCon.pos.Z));
            mesh.Normals.Add(new Vector3D(vert.Normal.X, vert.Normal.Y, vert.Normal.Z));
            mesh.TextureCoordinates.Add(new Point(vert.Uv1.U, vert.Uv1.V));
          }
          foreach (var ind in v4mesh.FaceDef.Indices)
          {
            mesh.TriangleIndices.Add(indicesOffset + ind);
          }
        }
      }
    }
    private GeometryModel3D geomFromWalkString(string walk)
    {
      GeometryModel3D ret = new GeometryModel3D();
      DiffuseMaterial materail = new DiffuseMaterial();
      materail.Brush = Brushes.Red;
      materail.AmbientColor = Colors.White;
      MeshGeometry3D mesh = new MeshGeometry3D();
      Vector3D first = new Vector3D();
      Vector3D normal = walkNormal;
      string pattern = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      foreach (char c in walk)
      {
        int turn = pattern.IndexOf(c);
        AxisAngleRotation3D rotat = new AxisAngleRotation3D(new Vector3D(0, 1, 0), (360.0 / (double)(pattern.Length+walkSmooth)) * (double)turn);
        RotateTransform3D trans = new RotateTransform3D(rotat);
        normal = trans.Transform(normal);
        Vector3D second = first + normal;
        addSegment(mesh, new Point3D(first.X,first.Y,first.Z), new Point3D(second.X,second.Y,second.Z), new Vector3D(0,1,0));
        first = second;
      }
      ret.Geometry = mesh;
      ret.BackMaterial = materail;
      ret.Material = materail;
      return ret;
    }
  }
}
