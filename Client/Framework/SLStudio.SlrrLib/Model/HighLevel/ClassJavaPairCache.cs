using System.Collections.Generic;
using System.IO;

namespace SlrrLib.Model.HighLevel
{
    public class ClassJavaPairCache
    {
        private string slrrRoot;
        private Dictionary<string, ClassJavaPair> classCache = new Dictionary<string, ClassJavaPair>();

        public ClassJavaPairCache(string slrrRoot)
        {
            this.slrrRoot = slrrRoot;
        }

        public IEnumerable<ClassJavaPair> GetAllCached()
        {
            return classCache.Values;
        }

        public ClassJavaPair GetPairFromClassFileName(string fnam)
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
                classCache[realFnam] = new ClassJavaPair(realFnam);
            return classCache[realFnam];
        }

        public void ClearCache()
        {
            classCache = new Dictionary<string, ClassJavaPair>();
        }
    }
}