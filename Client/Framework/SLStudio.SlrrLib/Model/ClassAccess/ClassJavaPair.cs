using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlrrLib.Model
{
    public class ClassJavaPair
    {
        private string classFnam;
        private string javaFnam;
        private List<ScriptClassTUFAChunk> binaryClassTUFAEntries_cache = null;
        private List<ScriptClass> consistentClasses = null;
        private ScriptJava javaFile = null;

        public bool ForceHideClassFileFromPair
        {
            get;
            set;
        } = false;

        public bool IsClassPresent
        {
            get
            {
                if (ForceHideClassFileFromPair)
                    return false;
                return File.Exists(classFnam);
            }
        }

        public bool IsJavaPresent
        {
            get
            {
                return File.Exists(javaFnam);
            }
        }

        public string PackageName
        {
            get
            {
                if (IsJavaPresent)
                    return javaFile.PackageNameToken.Token;
                return BinaryClassTUFAEntries[0].ConsEntry.PackageName;
            }
            set
            {
                if (IsJavaPresent)
                {
                    javaFile.PackageNameToken.Token = value;
                }
                if (IsClassPresent)
                {
                    foreach (var tufa in BinaryClassTUFAEntries)
                    {
                        tufa.ConsEntry.PackageName = value;
                    }
                }
            }
        }

        public List<ScriptClassTUFAChunk> BinaryClassTUFAEntries
        {
            get
            {
                if (binaryClassTUFAEntries_cache == null)
                    binaryClassTUFAEntries_cache = loadBinaryClassTUFAEntries();
                return binaryClassTUFAEntries_cache;
            }
            set
            {
                binaryClassTUFAEntries_cache = value;
            }
        }

        public ClassJavaPair(string Classfnam)
        {
            this.classFnam = Classfnam;
            getJavaFromClass();
            if (IsJavaPresent)
                javaFile = new ScriptJava(javaFnam);
        }

        public string GetJavaFileName()
        {
            return javaFnam;
        }

        public string GetClassFileName()
        {
            return classFnam;
        }

        public bool CheckZerosAre32s()
        {
            if (!IsClassPresent)
                return false;
            return File.ReadAllBytes(classFnam)[5] == 32;
        }

        public void FixZerosAre32s()
        {
            if (CheckZerosAre32s())
            {
                var byts = File.ReadAllBytes(classFnam);
                for (int byt_i = 0; byt_i != byts.Length; ++byt_i)
                {
                    if (byts[byt_i] == 32)
                        byts[byt_i] = 0;
                }
                int fix_i = 0;
                while (File.Exists(classFnam + "_zerosFix" + fix_i.ToString()))
                    fix_i++;
                File.Copy(classFnam, classFnam + "_zerosFix" + fix_i.ToString(), true);
                File.WriteAllBytes(classFnam, byts);
            }
        }

        public int FixClassAndSuperClassNames()
        {
            if (!IsClassPresent)
                return -1;
            if (classFnam.EndsWith("GameState.class"))
                return 0;
            int ret = 0;
            var bytes = File.ReadAllBytes(classFnam).Skip(32).TakeWhile(x => x != 0 && x != 4).ToArray();
            int oldSize = BitConverter.ToInt32(File.ReadAllBytes(classFnam).Skip(28).Take(4).ToArray(), 0);
            int realSize = bytes.Length;
            if (bytes.Length != oldSize)
            {
                SlrrLib.Model.MessageLog.AddMessage("Incorrect In Class Name: " + classFnam);
                ret = 1;
            }
            int minusOneForNoNullEndingOfClassName = File.ReadAllBytes(classFnam)[32 + realSize] == 4 ? 1 : 0;
            if (minusOneForNoNullEndingOfClassName == 1)
            {
                SlrrLib.Model.MessageLog.AddMessage("Potential bad : " + classFnam);
            }
            bytes = File.ReadAllBytes(classFnam).Skip(32 + realSize + 13 + 4 - minusOneForNoNullEndingOfClassName).TakeWhile(x => x != 0 && x != 4).ToArray();
            oldSize = BitConverter.ToInt32(File.ReadAllBytes(classFnam).Skip(32 + realSize + 13 - minusOneForNoNullEndingOfClassName).Take(4).ToArray(), 0);
            int realSpuerSize = bytes.Length;
            if (bytes.Length != oldSize)
            {
                SlrrLib.Model.MessageLog.AddMessage("Incorrect In SuperClass Name: " + classFnam);
                if (ret == 1)
                    ret = 3;
                else
                    ret = 2;
            }
            if (ret > 0)
            {
                bytes = File.ReadAllBytes(classFnam);
                var nameBytes = BitConverter.GetBytes((int)realSize);
                var superBytes = BitConverter.GetBytes((int)realSpuerSize);
                Array.Copy(nameBytes, 0, bytes, 28, 4);
                Array.Copy(superBytes, 0, bytes, 32 + realSize + 13 - minusOneForNoNullEndingOfClassName, 4);
                int fix_i = 0;
                while (File.Exists(classFnam + "_IncorrectClassNameFix" + fix_i.ToString()))
                    fix_i++;
                File.Copy(classFnam, classFnam + "_IncorrectClassNameFix" + fix_i.ToString(), true);
                File.WriteAllBytes(classFnam, bytes);
            }
            return ret;
        }

        public void SaveClassFileAs(string fnam)
        {
            BinaryWriter bw = new BinaryWriter(File.Create(fnam));
            foreach (var tufa in BinaryClassTUFAEntries)
            {
                tufa.Save(bw);
            }
            bw.Close();
        }

        public void SaveClassFile(bool backup = true, string suffix = "_BAK_ClassJavaPair_")
        {
            string classFnamLocal = FileEntry.GetWindowsPhysicalPath(classFnam);
            if (backup && File.Exists(classFnamLocal))
            {
                int bakInd = 0;
                while (File.Exists(classFnamLocal + suffix + bakInd.ToString()))
                    bakInd++;
                File.Copy(classFnamLocal, classFnamLocal + suffix + bakInd.ToString());
            }
            foreach (var tufa in BinaryClassTUFAEntries)
                tufa.Cache.CacheData();
            if (File.Exists(classFnamLocal))
                File.Delete(classFnamLocal);
            BinaryWriter bw = new BinaryWriter(File.Create(classFnamLocal));
            foreach (var tufa in BinaryClassTUFAEntries)
            {
                tufa.Save(bw);
            }
            bw.Close();
        }

        public void SaveBoth(bool backup = true)
        {
            if (IsJavaPresent)
                javaFile.SaveChangesToOriginalFile(backup);
            if (IsClassPresent)
                SaveClassFile(backup);
        }

        public IEnumerable<ScriptClass> GetConsistentClasses()
        {
            if (consistentClasses == null)
                return UpdateConsistentClasses();
            return consistentClasses;
        }

        public IEnumerable<ScriptClass> UpdateConsistentClasses()
        {
            consistentClasses = new List<ScriptClass>();
            if (!IsJavaPresent && IsClassPresent)
            {
                consistentClasses.AddRange(BinaryClassTUFAEntries.Select(x => new ScriptClass(x.ConsEntry, null)));
                return consistentClasses;
            }
            if (!IsClassPresent && IsJavaPresent)
            {
                consistentClasses.AddRange(javaFile.ClassesDefined.Select(x => new ScriptClass(null, x)));
                return consistentClasses;
            }
            List<ScriptClassCONSChunk> constList = new List<ScriptClassCONSChunk>();
            if (BinaryClassTUFAEntries.Count == 0)
            {
                MessageLog.AddError("Could not read TUFA entries of " + GetClassFileName());
                return consistentClasses;
            }
            constList.AddRange(BinaryClassTUFAEntries.Select(x => x.ConsEntry));
            if (!constList.First().FullClassName.StartsWith(javaFile.PackageNameToken.Token))
            {
                throw new Exception("Could not find match (in package name)");
            }
            foreach (var frmJava in javaFile.ClassesDefined)
            {
                ScriptClassCONSChunk found = null;
                foreach (var find in constList)
                {
                    if (find.FullClassName.EndsWith(frmJava.ClassName.Token)
                        && (find.FullExtendsClassName == "java.lang.Object" || find.FullExtendsClassName.EndsWith(frmJava.ClassExtendsName.Token)))
                    {
                        List<string> convertedRpkRef1 = find.RpkRefs.Select(x => x.RPKNameString + ":" + x.TypeIdInRPK.ToString()).ToList();
                        List<string> convertedRpkRef2 = frmJava.RPKRefs.Select(x => x.RPKasSlrrRootRelativeFnam + ":" + x.ResID.ToString()).ToList();
                        if (convertedRpkRef1.Count != convertedRpkRef2.Count)
                        {
                            throw new Exception("Could not find match (in rpkrefs) count different");
                        }
                        foreach (var frmClassStr in convertedRpkRef1)
                        {
                            string foundStr = null;
                            foreach (var findStr in convertedRpkRef2)
                            {
                                if (findStr.ToLower() == frmClassStr.ToLower())
                                {
                                    foundStr = findStr;
                                    break;
                                }
                            }
                            if (foundStr == null && frmClassStr != "system.rpk:0")//this comes upsometimes
                            {
                                throw new Exception("Could not find match (in rpkrefs)");
                            }
                            else
                            {
                                convertedRpkRef2.Remove(foundStr);
                            }
                        }
                        found = find;
                    }
                }
                if (found == null)
                {
                    if (javaFile.ClassesDefined.Count() == 1 && BinaryClassTUFAEntries.Count == 1 &&
                        constList[0].FullExtendsClassName.EndsWith(javaFile.ClassesDefined.First().ClassExtendsName.Token) &&
                        Path.GetFileNameWithoutExtension(javaFnam).ToLower() == javaFile.ClassesDefined.First().ClassName.Token.ToLower())
                    {
                        constList[0].FullClassName = javaFile.PackageNameToken.Token + "." + javaFile.ClassesDefined.First().ClassName.Token;
                        SaveClassFile(true);
                        consistentClasses.Add(new ScriptClass(found, frmJava));
                        return consistentClasses;
                        //return CheckConsistency();
                    }
                    else if (javaFile.ClassesDefined.Count() == 1 && BinaryClassTUFAEntries.Count == 1 &&
                             Path.GetFileNameWithoutExtension(javaFnam).ToLower() != javaFile.ClassesDefined.First().ClassName.Token.ToLower() &&
                             constList[0].FullClassName.EndsWith(Path.GetFileNameWithoutExtension(javaFnam)))
                    {
                        javaFile.ClassesDefined.First().ClassName.Token = Path.GetFileNameWithoutExtension(javaFnam);
                        javaFile.SaveChangesToOriginalFile(true);
                        consistentClasses.Add(new ScriptClass(found, frmJava));
                        return consistentClasses;
                        //return CheckConsistency();
                    }
                    else
                        throw new Exception("Could not find match (in names)");
                }
                else
                {
                    consistentClasses.Add(new ScriptClass(found, frmJava));
                    constList.Remove(found);
                }
            }
            return consistentClasses;
        }

        public static string SlrRelativeRpkNameFromRpkExternalReferenceString(string rpkExternalRef)
        {
            return rpkExternalRef.Remove(rpkExternalRef.Length - 1).Replace("/", "\\").Trim('\\', '/') + ".rpk";
        }

        public static string SlrRelativeRpkNameFromRpkRefDef(string nonslrrelativeRpkname)
        {
            return nonslrrelativeRpkname.Replace(".", "\\") + ".rpk";
        }

        public static string RpkRefDefFromSlrRelativeRpkName(string slrrelativeRpkname)
        {
            return slrrelativeRpkname.Remove(slrrelativeRpkname.Length - 4).Replace("\\", ".");
        }

        public static string JavaFnamFromClassFnam(string classFnam)
        {
            string name = Path.GetFileName(classFnam);
            if (name == "")
                return "";
            return classFnam.Replace(name, "src\\" + name).ToLower().Replace(".class", ".java");
        }

        public static string ClassFnamFromJavaFnam(string javaFnam)
        {
            int lastSrc = javaFnam.LastIndexOf("src\\");
            if (lastSrc == -1)
                return "";
            return javaFnam.Remove(lastSrc, 4).ToLower().Replace(".java", ".class");
        }

        private void getJavaFromClass()
        {
            string name = Path.GetFileName(classFnam);
            javaFnam = classFnam.Replace(name, "src\\" + name).Replace(".class", ".java");
        }

        private void getClassFromJava()
        {
            int lastSrc = javaFnam.LastIndexOf("src\\");
            classFnam = javaFnam.Remove(lastSrc, 4);
        }

        private List<ScriptClassTUFAChunk> loadBinaryClassTUFAEntries()
        {
            if (!IsClassPresent)
                return new List<ScriptClassTUFAChunk>();
            var ret = new List<ScriptClassTUFAChunk>();
            FileCacheHolder clsHolder = new FileCacheHolder(classFnam, true);
            var byts = clsHolder.GetFileData();
            if (byts.Count(x => x == 0x20) > byts.Length / 10)
            {
                for (int i = 0; i != byts.Length; ++i)
                {
                    if (byts[i] == 0x20)
                        byts[i] = 0x00;
                }
                clsHolder = new FileCacheHolder(byts);
            }
            int curOffset = 0;
            int maxLength = clsHolder.GetFileDataLength();
            while (curOffset != maxLength)
            {
                var toad = new ScriptClassTUFAChunk(clsHolder, curOffset);
                ret.Add(toad);
                curOffset += toad.GetSizeFromEntries();
            }
            return ret;
        }
    }
}