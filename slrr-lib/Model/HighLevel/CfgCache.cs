using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model.HighLevel
{
  public class CfgCache
  {
    private string slrrRoot;
    Dictionary<string, Cfg> cfgCache = new Dictionary<string, Cfg>();

    public CfgCache(string slrrRoot)
    {
      this.slrrRoot = slrrRoot;
    }
    public IEnumerable<Cfg> CachedCfgs
    {
      get
      {
        return cfgCache.Values;
      }
    }
    public Cfg GetCFGFromFileName(string fnam)
    {
      string realFnam = fnam;
      if (!File.Exists(realFnam))
      {
        realFnam = slrrRoot + "\\" + fnam;
      }
      if (!File.Exists(realFnam))
        return null;
      realFnam = realFnam.ToLower();
      if (!cfgCache.ContainsKey(realFnam))
        cfgCache[realFnam] = new Cfg(realFnam);
      return cfgCache[realFnam];
    }
    public void ClearCache()
    {
      cfgCache = new Dictionary<string, Cfg>();
    }
  }
}
