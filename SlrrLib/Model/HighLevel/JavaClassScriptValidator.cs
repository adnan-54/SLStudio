using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlrrLib.Model.HighLevel
{
    public class JavaClassScriptValidator
    {
        private ClassJavaPairCache cache;
        private string slrrRoot;
        private bool cacheBuilt = false;

        public JavaClassScriptValidator(string slrrRoot)
        {
            cache = new ClassJavaPairCache(slrrRoot);
            this.slrrRoot = slrrRoot;
        }

        public void CheckForOverwritingMultiplyDefinedClasses()
        {
            buildFullClassCache();
            Dictionary<string, List<Tuple<string, ScriptClassTUFAChunk>>> classNameToTUFA = new Dictionary<string, List<Tuple<string, ScriptClassTUFAChunk>>>();
            int cur = 0;
            int count = cache.GetAllCached().Count();
            foreach (var cls in cache.GetAllCached())
            {
                if (!cls.IsClassPresent)
                    continue;
                foreach (var tufa in cls.BinaryClassTUFAEntries)
                {
                    cur++;
                    MessageLog.AddMessage(cur.ToString() + "\\" + count.ToString());
                    if (cls.IsClassPresent)
                    {
                        foreach (var conClas in cls.BinaryClassTUFAEntries)
                        {
                            string fullClassName = cls.PackageName + "." + conClas.ConsEntry.ShortClassName;

                            if (classNameToTUFA.ContainsKey(fullClassName))
                            {
                                if (classNameToTUFA[fullClassName].Any(x => !signaturedDataFullMatch(x.Item2.GetWithSignature("TREE"), conClas.GetWithSignature("TREE"))))
                                {
                                    MessageLog.AddError("Class: " + fullClassName + " already defined in: " + classNameToTUFA[fullClassName].Select(x => x.Item1).Aggregate((x, y) => x + ", " + y));
                                    MessageLog.AddError("Redefinition at Class: " + cls.GetClassFileName());
                                    classNameToTUFA[fullClassName].Add(new Tuple<string, ScriptClassTUFAChunk>(cls.GetClassFileName(), tufa));
                                }
                            }
                            else
                            {
                                classNameToTUFA[fullClassName] = new List<Tuple<string, ScriptClassTUFAChunk>>
                {
                  new Tuple<string, ScriptClassTUFAChunk>(cls.GetClassFileName(), tufa)
                };
                            }
                        }
                    }
                }
            }
        }

        public void CheckForMultiplyDefinedClasses()
        {
            buildFullClassCache();
            Dictionary<string, ClassJavaPair> classNameToClassFile = new Dictionary<string, ClassJavaPair>();
            int cur = 0;
            int count = cache.GetAllCached().Count();
            foreach (var cls in cache.GetAllCached())
            {
                cur++;
                MessageLog.AddMessage(cur.ToString() + "\\" + count.ToString());
                if (cls.IsClassPresent)
                {
                    foreach (var conClas in cls.BinaryClassTUFAEntries.Select(x => new ScriptClass(x.ConsEntry, null)))
                    {
                        string fullClassName = cls.PackageName + "." + conClas.ShortClassName;
                        if (classNameToClassFile.ContainsKey(fullClassName))
                        {
                            if (classNameToClassFile[fullClassName].IsJavaPresent)
                                MessageLog.AddError("Class: " + fullClassName + " already defined in: " + classNameToClassFile[fullClassName].GetJavaFileName());
                            else
                                MessageLog.AddError("Class: " + fullClassName + " already defined in: " + classNameToClassFile[fullClassName].GetClassFileName());
                            if (cls.IsJavaPresent)
                                MessageLog.AddError("Redefinition at Java: " + cls.GetJavaFileName());
                            else
                                MessageLog.AddError("Redefinition at Class: " + cls.GetClassFileName());
                        }
                        else
                        {
                            classNameToClassFile[fullClassName] = cls;
                        }
                    }
                }
            }
        }

        public void CheckForRpkRefsNotFound()
        {
            buildFullClassCache();
            Dictionary<string, ClassJavaPair> classNameToClassFile = new Dictionary<string, ClassJavaPair>();
            int cur = 0;
            int count = cache.GetAllCached().Count();
            foreach (var cls in cache.GetAllCached())
            {
                cur++;
                MessageLog.AddMessage(cur.ToString() + "\\" + count.ToString());
                if (cls.IsClassPresent)
                {
                    foreach (var conClas in cls.BinaryClassTUFAEntries)
                    {
                        foreach (var rpkRef in conClas.ConsEntry.RpkRefs)
                        {
                            if (!File.Exists(slrrRoot + "\\" + rpkRef.RPKNameString))
                            {
                                MessageLog.AddError("Rpk ref " + rpkRef.RPKNameString + " not found refed in cls: " + cls.GetClassFileName() + " in TUFA " + conClas.ConsEntry.FullClassName);
                            }
                        }
                        foreach (var str in conClas.ConsEntry.Constants.Where(x => x is ScriptClassCONSNameEntry)
                                 .Select(x => x as ScriptClassCONSNameEntry))
                        {
                            if (str.DataAsString.ToLower().EndsWith(".rpk") && !File.Exists(slrrRoot + "\\" + str.DataAsString))
                            {
                                MessageLog.AddError("WEAKRpk ref " + str.DataAsString + " not found refed in cls: " + cls.GetClassFileName() + " in TUFA " + conClas.ConsEntry.FullClassName);
                            }
                        }
                    }
                }
            }
        }

        public void ReplaceWeakRpkRefsInClasses(string newRpkRef, bool evenIfJavaIsPresent = false)
        {
            if (!File.Exists(slrrRoot + "\\" + newRpkRef))
            {
                MessageLog.AddError("Weak rpk refs will be replaced with a still non existent rpk: " + newRpkRef);
            }
            buildFullClassCache();
            Dictionary<string, ClassJavaPair> classNameToClassFile = new Dictionary<string, ClassJavaPair>();
            int cur = 0;
            int count = cache.GetAllCached().Count();
            foreach (var cls in cache.GetAllCached())
            {
                cur++;
                MessageLog.AddMessage(cur.ToString() + "\\" + count.ToString());
                bool save = false;
                if (cls.IsClassPresent && (!cls.IsJavaPresent || evenIfJavaIsPresent))
                {
                    foreach (var conClas in cls.BinaryClassTUFAEntries)
                    {
                        foreach (var str in conClas.ConsEntry.Constants.Where(x => x is ScriptClassCONSNameEntry)
                                 .Select(x => x as ScriptClassCONSNameEntry))
                        {
                            if (str.DataAsString.ToLower().EndsWith(".rpk") && !File.Exists(slrrRoot + "\\" + str.DataAsString))
                            {
                                save = true;
                                MessageLog.AddError("WEAKRpk ref " + str.DataAsString + " not found refed in cls: " + cls.GetClassFileName() + " in TUFA " + conClas.ConsEntry.FullClassName);
                                str.ReplaceString(newRpkRef);
                            }
                        }
                    }
                    if (save)
                        cls.SaveClassFile(true, "_BAK_WEAKREF_");
                }
            }
        }

        public void ReportClassContainingEntryWithString(string toFind)
        {
            buildFullClassCache();
            int cur = 0;
            int count = cache.GetAllCached().Count();
            MessageLog.AddMessage("ReportClassContainingEntryWithString toFind: " + toFind);
            foreach (var cls in cache.GetAllCached())
            {
                cur++;
                MessageLog.AddMessage(cur.ToString() + "\\" + count.ToString());
                foreach (var tufa in cls.BinaryClassTUFAEntries)
                {
                    foreach (var str in tufa.ConsEntry.Constants)
                    {
                        if (str is ScriptClassCONSNameEntry)
                        {
                            if ((str as ScriptClassCONSNameEntry).DataAsString.Contains(toFind))
                            {
                                MessageLog.AddMessage("toFind: " + toFind + " found in class: " + cls.GetClassFileName() + " at classDefined with name: " + tufa.ConsEntry.FullClassName);
                            }
                        }
                    }
                }
            }
        }

        private void buildFullClassCache()
        {
            if (cacheBuilt)
                return;
            cacheBuilt = true;
            var allClasses = Directory.EnumerateFiles(slrrRoot, "*.class", SearchOption.AllDirectories).Where(x => GameFileManager.BlackListWordsForFiles.All(y => !x.Contains(y))).ToList();
            MessageLog.AddMessage("Building Script cache from allClasses.count: " + allClasses.Count.ToString());
            int cur = 0;
            foreach (var cls in allClasses)
            {
                cur++;
                cache.GetPairFromClassFileName(cls);
                MessageLog.AddMessage(cur.ToString() + "\\" + allClasses.Count.ToString());
            }
        }

        private bool signaturedDataFullMatch(ScriptClassChunk b1, ScriptClassChunk b2)
        {
            if (b1 == null && b2 == null)
                return true;
            if (b1 == null || b2 == null)
                return false;
            return byteDataFullMatch(b1.RawDataOfEntry, b2.RawDataOfEntry);
        }

        private bool byteDataFullMatch(byte[] b1, byte[] b2)
        {
            if (b1 == null && b2 == null)
                return true;
            if (b1.Length != b2.Length)
                return false;
            for (int i = 0; i != b1.Length; i++)
            {
                if (b1[i] != b2[i])
                    return false;
            }
            return true;
        }
    }
}