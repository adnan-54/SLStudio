using SlrrLib.Model;

namespace SlrrLib.Geom
{
  public class NativeGeometryCfgContext
  {
    public DynamicResEntry res;
    public Cfg cfg;
    public DynamicRpk rpk;

    public NativeGeometryCfgContext(Cfg cfg, DynamicRpk rpk,DynamicResEntry res)
    {
      this.cfg = cfg;
      this.rpk = rpk;
      this.res = res;
    }
  }
}
