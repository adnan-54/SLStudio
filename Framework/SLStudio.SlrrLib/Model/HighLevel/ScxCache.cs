using System.Collections.Generic;
using System.IO;

namespace SlrrLib.Model.HighLevel
{
    public class ScxCache
    {
        private string slrrRoot;
        private Dictionary<string, Scx> scxCache = new Dictionary<string, Scx>();

        public ScxCache(string slrrRoot)
        {
            this.slrrRoot = slrrRoot;
        }

        public Scx GetSCXFromFileName(string fnam)
        {
            string realFnam = fnam;
            if (!File.Exists(realFnam))
            {
                realFnam = slrrRoot + "\\" + fnam;
            }
            if (!File.Exists(realFnam))
                return null;
            realFnam = realFnam.ToLower();
            if (!scxCache.ContainsKey(realFnam))
            {
                scxCache[realFnam] = Scx.ConstructScx(realFnam);
                scxCache[realFnam].CahceData();
            }
            return scxCache[realFnam];
        }

        public void ClearCache()
        {
            scxCache = new Dictionary<string, Scx>();
        }
    }
}