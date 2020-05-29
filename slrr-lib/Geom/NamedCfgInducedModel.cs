using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
  public class NamedCfgInducedModel : NamedScxModel
  {
    public SlrrLib.Model.CfgBodyLine BodyLine;
    public SlrrLib.Model.Cfg Cfg;
    public int SourceTypeID = -1;

    public NamedCfgInducedModel(NamedScxModel b)
    :base(b)
    {

    }
    public NamedCfgInducedModel()
    :base()
    {

    }

    public static NamedCfgInducedModel ConstructMovableModelFromSCX(string fnam,string name = "<unnamed>")
    {
      if(!File.Exists(fnam))
        return null;
      var model = new NamedCfgInducedModel();
      SlrrLib.Model.CfgBodyLine line = new SlrrLib.Model.CfgBodyLine(new SlrrLib.Model.CfgLine("body 0.0 0.0 0.0 0.000 0.000 0.000 0.0 none"));
      model.BodyLine = line;
      model.Cfg = null;
      model.Name = name;
      model.SourceOfModel = Geom.NamedModelSource.BodyLine;
      model.Translate = new Vector3D(0,0,0);
      if(new SlrrLib.Model.Scx(fnam).Version == 4)
      {
        SlrrLib.Model.BinaryScxV4 scxMesh = new SlrrLib.Model.BinaryScxV4(fnam, true);
        SlrrLib.Model.DynamicScxV4 scxData = new SlrrLib.Model.DynamicScxV4(scxMesh);
        SlrrLib.Geom.ScxV4Geometry geom = new SlrrLib.Geom.ScxV4Geometry(scxMesh);
        model.ModelGeom = geom.WpfModels(1).First();
      }
      else
      {
        SlrrLib.Model.BinaryScxV3 scxMesh = new SlrrLib.Model.BinaryScxV3(fnam, true);
        SlrrLib.Model.DynamicScxV3 scxData = new SlrrLib.Model.DynamicScxV3(scxMesh);
        SlrrLib.Geom.ScxV3Geometry geom = new SlrrLib.Geom.ScxV3Geometry(scxMesh);
        model.ModelGeom = geom.WpfModels(1).First();
      }
      return model;
    }
    public static Vector3D GetTranslateVec3FromBodyLinePos(SlrrLib.Model.CfgBodyLine bodyLine)
    {
      if (bodyLine == null)
        return new Vector3D(0, 0, 0);
      return new Vector3D(100 * bodyLine.LineX, 100 * bodyLine.LineY, 100 * bodyLine.LineZ);
    }
    public static Vector3D GetYprFromBodyLinePos(SlrrLib.Model.CfgBodyLine bodyLine)
    {
      if (bodyLine == null)
        return new Vector3D(0, 0, 0);
      return new Vector3D(bodyLine.LineRotX, bodyLine.LineRotY, bodyLine.LineRotZ);
    }
  }
}
