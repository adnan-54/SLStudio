using System.Collections.Generic;
using System.IO;

namespace SlrrLib.Model.HighLevel
{
    public class BinaryRpkDataCache
    {
        private string slrrRoot;
        private Dictionary<string, DynamicRpk> classCache = new Dictionary<string, DynamicRpk>();

        public BinaryRpkDataCache(string slrrRoot)
        {
            this.slrrRoot = slrrRoot;
        }

        public IEnumerable<DynamicRpk> GetAllCached()
        {
            return classCache.Values;
        }

        public IEnumerable<KeyValuePair<string, DynamicRpk>> GetAllCachedWithSourceFileNames()
        {
            return classCache;
        }

        public DynamicRpk GetRPKFromClassFileName(string fnam)
        {
            string realFnam = fnam;
            if (!File.Exists(realFnam) && !File.Exists(ClassJavaPair.JavaFnamFromClassFnam(fnam)))
            {
                realFnam = slrrRoot + "\\" + fnam;
            }
            if (!File.Exists(realFnam) && !File.Exists(ClassJavaPair.JavaFnamFromClassFnam(fnam)))
                return null;
            realFnam = realFnam.ToLower();
            if (!classCache.ContainsKey(realFnam))
                classCache[realFnam] = new DynamicRpk(new BinaryRpk(fnam));
            return classCache[realFnam];
        }

        public void ClearCache()
        {
            classCache = new Dictionary<string, DynamicRpk>();
        }
    }
}