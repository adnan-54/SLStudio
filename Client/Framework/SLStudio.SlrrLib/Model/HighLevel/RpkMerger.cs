using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlrrLib.Model.HighLevel
{
    public class RpkMerger
    {
        private static string[] stringInnerEntryFileRefFormats =//this is tested out and true, not a guess
        {
      "sourcefile"
      ,"shape"
      ,"native car"
      ,"script"
      ,"native part"
      ,"native button"
      ,"native invitem"
      ,"skeleton"
      ,"native ped"
      ,"skidmark"
      ,"particle"
      ,"native object"
    };

        private static string[] stringInnerEntryTypeIDRefDefs =//this is tested out and true, not a guess
        {
      "mesh"
      ,"texture"
      ,"lf_texture"
      ,"attH_texture"
      ,"attV_texture"
      ,"rendertype"
      ,"native sun"
    };

        private GameFileManager gManag;
        private DynamicRpk rpkToMergeToData;
        private int reTypeIDNextID = 6;
        private string saveFilename;
        private string saveFileResDir;
        private CfgCache cfgCache = null;//all cached cfgs should be saved
        private BinaryRpkDataCache rpkCache = null;//all chached rpks should be saved
        private Dictionary<string, ClassJavaPair> javasThatShouldBeSaved = null;
        private List<KeyValuePair<string, string>> directoryCopyOpeartions = null;//copying is not done directly
        private List<KeyValuePair<string, string>> rpkRenameOpeartions = null;//copying is not done directly
        private ClassJavaPairCache javaCache = null;

        public RpkMerger(string saveFileName, GameFileManager gManag, bool appendDataToResourceFolderName = true)
        {
            rpkToMergeToData = new DynamicRpk();
            saveFilename = saveFileName;
            if (File.Exists(saveFilename))
            {
                var binRpk = new BinaryRpk(saveFilename);
                rpkToMergeToData = new DynamicRpk(binRpk);
                reTypeIDNextID = rpkToMergeToData.Entries.Max(x => x.TypeID) + 1;
                mergedIntoRpksASExtRefs.Add(gManag.GetRpkFromWeakFileName(saveFilename).GetRpkAsRPKRefString().Trim('\\', '/') + ".rpk");
            }
            this.gManag = gManag;
            if (appendDataToResourceFolderName)
                saveFileResDir = Path.GetFullPath(Path.GetDirectoryName(saveFilename) + "\\" + Path.GetFileNameWithoutExtension(saveFilename) + "_data").ToLower();
            else
                saveFileResDir = Path.GetFullPath(Path.GetDirectoryName(saveFilename) + "\\" + Path.GetFileNameWithoutExtension(saveFilename)).ToLower();
        }

        public void PushRpk(RpkManager rpk)
        {
            if (rpk.Rpk.RESEntries.Any(x => x.DanglingHiddenEntries.Any() || x.RSD.InnerEntries.Any(y => !(y is BinaryStringInnerEntry))))
            {
                MessageLog.AddError("Merging RPKs with non StringInnerRSDEntries not supported RPK: " + rpk.RpkFileName + " will not be added");
                return;
            }
            foreach (var potentialExtRef in gManag.Rpks)
            {
                if (potentialExtRef.GetIndexOfExternalRPKRefFromStringVal(rpk.GetRpkAsRPKRefString()) != -1)
                {
                    if (potentialExtRef.Rpk.RESEntries.Any(x => x.DanglingHiddenEntries.Any() || x.RSD.InnerEntries.Any(y => !(y is BinaryStringInnerEntry))))
                    {
                        MessageLog.AddError("Merging RPKs that are referenced in RPKs with non StringInnerRSDEntries or HiddenEntries not supported; rpk: "
                                            + rpk.RpkFileName
                                            + " is referenced in rpk with hidden entries or non StringInnerEntries: "
                                            + potentialExtRef.RpkFileName);
                        return;
                    }
                }
            }
            if (rpkToMergeToData != null)
            {
                if (rpkToMergeToData.ExternalReferences.Any(x => x.ToLower() == (rpk.GetRpkAsRPKRefString().Trim('\\', '/') + ".rpk").ToLower()))
                {
                    MessageLog.AddError("The RPK: " + rpk.RpkFileName + " is a parent(external referenced rpk) of the stack currently being merged\r\n This RPK will not be added!");
                    return;
                }
            }
            if (mergedIntoRpksASExtRefs.Any(x => rpk.Rpk.ExternalReferences.Any(y => y.ReferenceString.ToLower() == x.ToLower())))
            {
                MessageLog.AddError("The RPK: " + rpk.RpkFileName + " has a external referenced rpk that has been merged to the current stack \r\n This RPK will not be added");
                return;
            }
            mergedIntoRpksASExtRefs.Add(rpk.GetRpkAsRPKRefString().Trim('\\', '/') + ".rpk");
            copyRpkContentAndResources(rpk);
        }

        public void SaveMerge()
        {
            int max = javasThatShouldBeSaved.Count;
            foreach (var jav in javasThatShouldBeSaved)
            {
                MessageLog.AddMessage("Java: " + jav.Value.GetClassFileName());
                jav.Value.SaveBoth();
            }
            max = directoryCopyOpeartions.Count;
            foreach (var dirCopy in directoryCopyOpeartions)
            {
                MessageLog.AddMessage("Dir cpy: " + dirCopy.Key + " -> " + dirCopy.Value);
                directoryCopy(dirCopy.Key, dirCopy.Value, true);
            }
            max = cfgCache.CachedCfgs.Count();
            foreach (var cfg in cfgCache.CachedCfgs)
            {
                MessageLog.AddMessage("Cfg: " + cfg.CfgFileName);
                cfg.Save();
            }
            max = rpkCache.GetAllCachedWithSourceFileNames().Count();
            foreach (var rpk in rpkCache.GetAllCachedWithSourceFileNames())
            {
                MessageLog.AddMessage("Rpk: " + rpk.Key);
                rpk.Value.SaveAs(FileEntry.GetWindowsPhysicalPath(rpk.Key));
            }
            max = rpkRenameOpeartions.Count();
            foreach (var rpkRenam in rpkRenameOpeartions)
            {
                MessageLog.AddMessage("RpkRename: " + rpkRenam.Key + " -> " + rpkRenam.Value);
                if (File.Exists(rpkRenam.Value))
                {
                    int bakI = 0;
                    while (File.Exists(rpkRenam.Value + "_" + bakI.ToString()))
                        bakI++;
                    File.Move(rpkRenam.Key, rpkRenam.Value + "_" + bakI.ToString());
                }
                else
                    File.Move(rpkRenam.Key, rpkRenam.Value);
            }
            MessageLog.AddMessage("Saving dest Rpk: " + saveFilename);
            rpkToMergeToData.SaveAs(saveFilename);
        }

        public void ReportOperation()
        {
            if (javasThatShouldBeSaved != null)
            {
                foreach (var jav in javasThatShouldBeSaved)
                {
                    MessageLog.AddMessage("Java: " + jav.Value.GetClassFileName());
                }
            }
            if (directoryCopyOpeartions != null)
            {
                foreach (var dirCopy in directoryCopyOpeartions)
                {
                    MessageLog.AddMessage("Dir cpy: " + dirCopy.Key + " -> " + dirCopy.Value);
                }
            }
            if (cfgCache != null)
            {
                foreach (var cfg in cfgCache.CachedCfgs)
                {
                    MessageLog.AddMessage("Cfg: " + cfg.CfgFileName);
                }
            }
            if (rpkCache != null)
            {
                foreach (var rpk in rpkCache.GetAllCachedWithSourceFileNames())
                {
                    MessageLog.AddMessage("Rpk: " + rpk.Key);
                }
            }
            if (rpkRenameOpeartions != null)
            {
                foreach (var rpkRenam in rpkRenameOpeartions)
                {
                    MessageLog.AddMessage("RpkRename: " + rpkRenam.Key + " -> " + rpkRenam.Value);
                }
            }
            MessageLog.AddMessage("Saving dest Rpk: " + saveFilename);
        }

        private static void directoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                MessageLog.AddError(
                  "Source directory does not exist or could not be found: "
                  + sourceDirName);
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    directoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private HashSet<string> getFoldersWithResources(RpkManager rpk)
        {
            HashSet<string> ret = new HashSet<string>();
            foreach (var res in rpk.Rpk.RESEntries)// i know that only stringinnerentries are possible
            {
                if (stringInnerEntryFileRefFormats.Any(x => res.RSD.RSDDataString.Contains(x)))
                {
                    var spl = res.RSD.RSDDataString.Split(new string[] { "\r", "\n", }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in spl)
                    {
                        foreach (var pattern in stringInnerEntryFileRefFormats)
                        {
                            if (line.StartsWith(pattern))
                            {
                                string toad = line.Remove(0, pattern.Length).Trim(' ', '\t');
                                if (toad.Contains(' '))
                                    toad = toad.Remove(toad.IndexOf(' '));
                                toad = Path.GetDirectoryName(toad);
                                if (!Directory.Exists(rpk.SlrrRootDir + "\\" + toad))
                                {
                                    MessageLog.AddError("Path(" + toad + ") referenced in rpk: " + rpk.RpkFileName + " can not be found source is TypeID: 0x" + res.TypeID.ToString("X8") + " RSD:\n" + res.RSD.RSDDataString);
                                }
                                else
                                {
                                    ret.Add(toad);
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }

        private string matchCaseFromEndTillDifferentCharacter(string toUpdate, string pattern)
        {
            int indUpd = toUpdate.Length - 1;
            int indPattern = pattern.Length - 1;
            string lowerUpd = toUpdate.ToLower();
            string lowerPatt = pattern.ToLower();
            List<char> ret = new List<char>(toUpdate);
            while (lowerUpd[indUpd] == lowerPatt[indPattern])
            {
                ret[indUpd] = pattern[indPattern];
                indUpd--;
                indPattern--;
                if (indUpd < 0 || indPattern < 0)
                    break;
            }
            return new string(ret.ToArray());
        }

        private void replaceResourcePathsInRES(DynamicStringInnerEntry strEntry, List<RpkMergerResourceDirectoryMoveDescriptor> pattern)
        {
            var spl = strEntry.StringData.Split(new string[] { "\r", "\n", }, StringSplitOptions.RemoveEmptyEntries);
            bool wasChange = false;
            for (int spl_i = 0; spl_i < spl.Length; ++spl_i)
            {
                foreach (var fileRefPattern in stringInnerEntryFileRefFormats)
                {
                    if (spl[spl_i].StartsWith(fileRefPattern))
                    {
                        var spaceSpl = spl[spl_i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        int fileRefInd = 1;
                        if (fileRefPattern.Contains(' '))
                            fileRefInd++;
                        foreach (var dirMovepattern in pattern)
                        {
                            string lowerVar = spaceSpl[fileRefInd].ToLower();
                            if (lowerVar.StartsWith(dirMovepattern.wasSlrrRelativeDir))
                            {
                                spaceSpl[fileRefInd] = matchCaseFromEndTillDifferentCharacter(lowerVar.Replace(dirMovepattern.wasSlrrRelativeDir, dirMovepattern.newSlrrRelativeDir), spaceSpl[fileRefInd]);
                                wasChange = true;
                                break;
                            }
                        }
                        spl[spl_i] = spaceSpl.Aggregate((x, y) => x + " " + y);
                    }
                }
            }
            if (wasChange)
            {
                strEntry.StringData = spl.Aggregate((x, y) => x + "\r\n" + y) + "\r\n";
            }
        }

        private void replaceResourcePathsInCfg(Cfg cfg, List<RpkMergerResourceDirectoryMoveDescriptor> pattern)
        {
            if (cfg != null)
            {
                foreach (var ln in cfg.Lines)
                {
                    if (ln.IsEmpty)
                        continue;
                    if (ln.Tokens[0].IsComment)
                        continue;
                    var format = ln.MatchedFormat().Split(' ');
                    for (int f_i = 0; f_i != format.Length; ++f_i)
                    {
                        foreach (var dirMovepattern in pattern)
                        {
                            if (ln.Tokens[f_i].Value.ToLower().StartsWith(dirMovepattern.wasSlrrRelativeDir))
                            {
                                ln.Tokens[f_i].Value = matchCaseFromEndTillDifferentCharacter(ln.Tokens[f_i].Value.ToLower().Replace(dirMovepattern.wasSlrrRelativeDir, dirMovepattern.newSlrrRelativeDir), ln.Tokens[f_i].Value);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void replaceTypeIDsInRESRSD(DynamicStringInnerEntry strEntry, List<RpkMergerRpkRenameEntry> pattern, Dictionary<int, int> oldExtRefIndToNewExtRefInd, int localExternalRefPart)
        {
            var spl = strEntry.StringData.Split(new string[] { "\r", "\n", }, StringSplitOptions.RemoveEmptyEntries);
            bool wasChange = false;
            for (int spl_i = 0; spl_i < spl.Length; ++spl_i)
            {
                foreach (var typeIDRefPattern in stringInnerEntryTypeIDRefDefs)
                {
                    if (spl[spl_i].StartsWith(typeIDRefPattern))
                    {
                        var spaceSpl = spl[spl_i].Split(new string[] { " ", "\t", ";", "//", "\\\\", "#" }, StringSplitOptions.RemoveEmptyEntries);
                        int fileRefInd = 1;
                        if (typeIDRefPattern.Contains(' '))
                            fileRefInd++;
                        foreach (var typeIDMovepattern in pattern)
                        {
                            try
                            {
                                int curtypeID = int.Parse(spaceSpl[fileRefInd].Substring(2), System.Globalization.NumberStyles.HexNumber);
                                if (RpkManager.TypeIDExternalIDPart(curtypeID) == typeIDMovepattern.wasTypeIDLocalPart && RpkManager.TypeIDExternalRefPart(curtypeID) == localExternalRefPart)
                                {
                                    wasChange = true;
                                    spaceSpl[fileRefInd] = "0x" + localExternalRefPart.ToString("X4") + typeIDMovepattern.newTypeIDLocalPart.ToString("X4");
                                    break;
                                }
                                int curExtRefpart = RpkManager.TypeIDExternalRefPart(curtypeID);
                                if (oldExtRefIndToNewExtRefInd.ContainsKey(curExtRefpart))
                                {
                                    string toSetTo = setExternalRefPartOfTypeID(spaceSpl[fileRefInd], oldExtRefIndToNewExtRefInd[curExtRefpart]);
                                    if (toSetTo != spaceSpl[fileRefInd])
                                    {
                                        wasChange = true;
                                        spaceSpl[fileRefInd] = toSetTo;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (wasChange)
                        {
                            spl[spl_i] = spaceSpl.Aggregate((x, y) => x + " " + y);
                            break;
                        }
                    }
                }
            }
            if (wasChange)
            {
                strEntry.StringData = spl.Aggregate((x, y) => x + "\r\n" + y) + "\r\n";
            }
        }

        private string replaceTypeIDInTypeIDStr(string strEntry, List<RpkMergerRpkRenameEntry> pattern, Dictionary<int, int> oldExtRefIndToNewExtRefInd, int localExternalRefPart)
        {
            foreach (var typeIDMovepattern in pattern)
            {
                try
                {
                    int curtypeID = int.Parse(strEntry.Substring(2), System.Globalization.NumberStyles.HexNumber);
                    if (RpkManager.TypeIDExternalIDPart(curtypeID) == typeIDMovepattern.wasTypeIDLocalPart && RpkManager.TypeIDExternalRefPart(curtypeID) == localExternalRefPart)
                    {
                        return "0x" + localExternalRefPart.ToString("X4") + typeIDMovepattern.newTypeIDLocalPart.ToString("X4");
                    }
                    int curExtRefpart = RpkManager.TypeIDExternalRefPart(curtypeID);
                    if (oldExtRefIndToNewExtRefInd.ContainsKey(curExtRefpart))
                    {
                        return setExternalRefPartOfTypeID(strEntry, oldExtRefIndToNewExtRefInd[curExtRefpart]);
                    }
                }
                catch (Exception)
                {
                }
            }
            return strEntry;
        }

        private void replaceTypeIDInCfg(Cfg cfg, List<RpkMergerRpkRenameEntry> pattern, Dictionary<int, int> oldExtRefIndToNewExtRefInd, int localExternalRefPart)
        {
            if (cfg != null)
            {
                foreach (var ln in cfg.Lines)
                {
                    if (ln.IsEmpty)
                        continue;
                    if (ln.Tokens[0].IsComment)
                        continue;
                    var format = ln.MatchedFormat().Split(' ');
                    for (int f_i = 0; f_i != format.Length; ++f_i)
                    {
                        if (format[f_i] == "R")
                        {
                            ln.Tokens[f_i].Value = replaceTypeIDInTypeIDStr(ln.Tokens[f_i].Value, pattern, oldExtRefIndToNewExtRefInd, localExternalRefPart);
                        }
                    }
                }
            }
        }

        private int setExternalRefPartOfTypeID(int typeID, int newExternalRef)
        {
            return (typeID & 0x0000FFFF) + (newExternalRef << 16);
        }

        private int getExtRefPartIntFromString(string typeID)
        {
            try
            {
                return int.Parse(typeID.Substring(2, 4));
            }
            catch (Exception)
            {
            }
            return -1;
        }

        private string setExternalRefPartOfTypeID(string typeID, int newExternalRef)
        {
            return "0x" + newExternalRef.ToString("X4") + typeID.Substring(typeID.Length - 4, 4);
        }

        private int getNewTypeIDAccordingToRenameData(int TypeID, int localExternalRefInd, List<RpkMergerRpkRenameEntry> currentRpkRenames, Dictionary<int, int> oldExtRefIndToNewExtRefInd)
        {
            var corrsponding = currentRpkRenames.FirstOrDefault(x => x.wasTypeIDLocalPart == RpkManager.TypeIDExternalIDPart(TypeID) && RpkManager.TypeIDExternalRefPart(TypeID) == localExternalRefInd);
            if (corrsponding != null)
                return corrsponding.newTypeIDLocalPart + (localExternalRefInd << 16);
            int extRefPart = RpkManager.TypeIDExternalRefPart(TypeID);
            if (oldExtRefIndToNewExtRefInd.ContainsKey(extRefPart))
                return setExternalRefPartOfTypeID(TypeID, oldExtRefIndToNewExtRefInd[extRefPart]);
            return TypeID;
        }

        private string getCfgSlrrRelativeFnamFromStringInnerEntry(DynamicStringInnerEntry strInnerEntry)
        {
            var tokensWithCfg = strInnerEntry.StringData.Split(new string[] { "\r", "\n", " ", "\t" }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.ToLower().EndsWith(".cfg")).ToList();
            if (tokensWithCfg.Count > 1)
            {
                MessageLog.AddError("Multiple cfgs defined in: " + strInnerEntry);
            }
            if (tokensWithCfg.Count != 0)
            {
                return tokensWithCfg.First();
            }
            return "";
        }

        private void copyRpkContentAndResources(RpkManager rpk)
        {
            foreach (var res in rpk.Rpk.RESEntries)
            {
                if (RpkManager.TypeIDExternalRefPart(res.TypeID) != 0)
                {
                    MessageLog.AddError("TypeID with External part not 0 : 0x" + res.TypeID.ToString("X8") + " in rpk: " + rpk.RpkFileName + "\n\tAbortingMerge");
                    return;
                }
            }
            if (rpkRenameOpeartions == null)
                rpkRenameOpeartions = new List<KeyValuePair<string, string>>();
            rpkRenameOpeartions.Add(new KeyValuePair<string, string>(rpk.RpkFileName, rpk.RpkFileName + "_MERGED_RPK_BAK"));
            DynamicRpk tempRpk = new DynamicRpk(rpk.Rpk);
            var foldersToCopy = getFoldersWithResources(rpk);
            var dirMoves = new List<RpkMergerResourceDirectoryMoveDescriptor>();
            if (directoryCopyOpeartions == null)
                directoryCopyOpeartions = new List<KeyValuePair<string, string>>();
            foreach (var folder in foldersToCopy)
            {
                MessageLog.AddMessage("Folder will be copied for merge potentially can be deleted: " + folder);
                string pattern = Path.GetFullPath(rpk.SlrrRootDir + "\\" + folder).ToLower();
                string target = saveFileResDir + "\\" + Path.GetFileNameWithoutExtension(rpk.RpkFileName) + "\\" + Path.GetFileName(pattern.TrimEnd('\\', '/'));
                int targetSuffix = 1;
                if (Directory.Exists(target) || dirMoves.Any(x => x.newSlrrRelativeDir == target.Remove(0, Path.GetFullPath(rpk.SlrrRootDir).Length).Trim('\\', '/')))
                {
                    while (Directory.Exists(target + "_" + targetSuffix.ToString())
                           || dirMoves.Any(x => x.newSlrrRelativeDir == (target + "_" + targetSuffix.ToString()).Remove(0, Path.GetFullPath(rpk.SlrrRootDir).Length).Trim('\\', '/')))
                        targetSuffix++;
                    target = target + "_" + targetSuffix.ToString();
                }
                target = target.ToLower();
                //DirectoryCopy(pattern, target);
                directoryCopyOpeartions.Add(new KeyValuePair<string, string>(pattern, target));
                dirMoves.Add(new RpkMergerResourceDirectoryMoveDescriptor
                {
                    wasSlrrRelativeDir = folder.ToLower(),
                    newSlrrRelativeDir = target.Remove(0, Path.GetFullPath(rpk.SlrrRootDir).Length).Trim('\\', '/')
                });
            }
            //rebase typeIDs
            List<RpkMergerRpkRenameEntry> currentRpkRenames = new List<RpkMergerRpkRenameEntry>();
            foreach (var res in tempRpk.Entries)
            {
                int wasTypeID = res.TypeID;
                res.TypeID = reTypeIDNextID;
                if (reTypeIDNextID >= 0x0000FFFF)
                {
                    MessageLog.AddError("!TypeIDs exceeded in rpk there can not be more than 0xFFFF records in one rpk! this merge will create a bad RPK if you run it");
                }
                reTypeIDNextID++;
                currentRpkRenames.Add(new RpkMergerRpkRenameEntry
                {
                    wasRefString = rpk.GetRpkAsRPKRefString(),
                    wasTypeIDLocalPart = wasTypeID,
                    newRefString = RpkManager.GetExternalRefStringFromRpkFnam(saveFilename, rpk.SlrrRootDir),
                    newTypeIDLocalPart = res.TypeID
                });
            }
            //check externalRefs rebaseIf needed
            Dictionary<int, int> oldExtRefIndToNewExtRefInd = new Dictionary<int, int>();
            for (int extref_i = 0; extref_i < tempRpk.ExternalReferences.Count; ++extref_i)
            {
                string extRefString = tempRpk.ExternalReferences[extref_i];
                int extRefInMergeRpk = rpkToMergeToData.ExternalReferences.IndexOf(extRefString);
                if (extRefInMergeRpk == -1)
                    rpkToMergeToData.ExternalReferences.Add(extRefString);
                extRefInMergeRpk = rpkToMergeToData.ExternalReferences.IndexOf(extRefString);
                if (extref_i == extRefInMergeRpk)
                    continue;
                oldExtRefIndToNewExtRefInd[extref_i + 1] = extRefInMergeRpk + 1;
            }
            if (cfgCache == null)
                cfgCache = new CfgCache(rpk.SlrrRootDir);
            if (rpkCache == null)
                rpkCache = new BinaryRpkDataCache(rpk.SlrrRootDir);
            if (javaCache == null)
                javaCache = new ClassJavaPairCache(rpk.SlrrRootDir);
            //UpdaetTypeIDRefsLocally, in rpk and in cfgs, javas can be updated globally they are not rpk dependent
            foreach (var res in tempRpk.Entries)
            {
                res.SuperID = getNewTypeIDAccordingToRenameData(res.SuperID, 0, currentRpkRenames, oldExtRefIndToNewExtRefInd);
                if (!res.RSD.InnerEntries.Any())
                    continue;
                var innerStr = res.RSD.InnerEntries.FirstOrDefault() as DynamicStringInnerEntry;
                var cfg = cfgCache.GetCFGFromFileName(getCfgSlrrRelativeFnamFromStringInnerEntry(innerStr));
                replaceResourcePathsInRES(innerStr, dirMoves);
                replaceTypeIDsInRESRSD(innerStr, currentRpkRenames, oldExtRefIndToNewExtRefInd, 0);
                replaceTypeIDInCfg(cfg, currentRpkRenames, oldExtRefIndToNewExtRefInd, 0);
                replaceResourcePathsInCfg(cfg, dirMoves);
                if (cfg != null)
                    cfg.OverWriteCfgFileName(rpk.SlrrRootDir + "\\" + getCfgSlrrRelativeFnamFromStringInnerEntry(innerStr));
            }
            Dictionary<int, int> emptyRenameData = new Dictionary<int, int>();
            //Update typeIDs in rpks referencing the current one being merged and corresponding cfgs
            string newRpkRef = Path.GetFullPath(saveFilename).Remove(0, gManag.SlrrRoot.Length).Trim('\\', '/');
            foreach (var extRpk in gManag.Rpks)
            {
                int extRefInd = extRpk.GetIndexOfExternalRPKRefFromStringVal(rpk.GetRpkAsRPKRefString());
                if (extRefInd != -1)//only proper(only stringInnerRSDentries) rpks can have != -1 this is checkedearlier
                {
                    var tempExtRpk = rpkCache.GetRPKFromClassFileName(extRpk.RpkFileName);
                    extRefInd++;//real extpart
                    foreach (var res in tempExtRpk.Entries)
                    {
                        res.SuperID = getNewTypeIDAccordingToRenameData(res.SuperID, extRefInd, currentRpkRenames, emptyRenameData);
                        if (!res.RSD.InnerEntries.Any())
                            continue;
                        replaceTypeIDsInRESRSD(res.RSD.InnerEntries.First() as DynamicStringInnerEntry, currentRpkRenames, emptyRenameData, extRefInd);
                        var cfg = cfgCache.GetCFGFromFileName(getCfgSlrrRelativeFnamFromStringInnerEntry(res.RSD.InnerEntries.First() as DynamicStringInnerEntry));
                        replaceTypeIDInCfg(cfg, currentRpkRenames, emptyRenameData, extRefInd);
                    }
                    tempExtRpk.ExternalReferences[extRefInd - 1] = newRpkRef;
                }
            }
            //Update Scripts they can be updated globally
            if (!javaCache.GetAllCached().Any())
            {
                MessageLog.AddMessage("Iterating *.java");
                var javas = Directory.EnumerateFiles(gManag.SlrrRoot, "*.java", SearchOption.AllDirectories).ToList();
                javas = javas.Where(y => !GameFileManager.BlackListWordsForFiles.Any(x => y.Contains(x))).ToList();
                MessageLog.AddMessage("Found " + javas.Count.ToString() + " javas");
                foreach (var java in javas)
                {
                    javaCache.GetPairFromClassFileName(ClassJavaPair.ClassFnamFromJavaFnam(java));
                }
                MessageLog.AddMessage("Iterating *.class");
                var classes = Directory.EnumerateFiles(gManag.SlrrRoot, "*.class", SearchOption.AllDirectories).ToList();
                classes = classes.Where(y => !GameFileManager.BlackListWordsForFiles.Any(x => y.Contains(x))).ToList();
                MessageLog.AddMessage("Found " + classes.Count.ToString() + " classes");
                foreach (var classFnam in classes)
                {
                    javaCache.GetPairFromClassFileName(classFnam);
                }
                MessageLog.AddMessage("Done creating JavaChace");
            }
            if (javasThatShouldBeSaved == null)
            {
                javasThatShouldBeSaved = new Dictionary<string, ClassJavaPair>();
            }
            foreach (var clsJav in javaCache.GetAllCached())
            {
                IEnumerable<ScriptClass> consClasses = null;
                try
                {
                    consClasses = clsJav.GetConsistentClasses();
                }
                catch (Exception)
                {
                    if (clsJav.IsJavaPresent)
                    {
                        clsJav.ForceHideClassFileFromPair = true;
                        clsJav.UpdateConsistentClasses();
                        consClasses = clsJav.GetConsistentClasses();
                        //MessageLog.AddMessage("There are inconsistent class definitions between the java: " + clsJav.GetJavaFileName() + " and its cls counterpart: " + clsJav.GetClassFileName() + " only the java will be modified!");
                    }
                }
                try
                {
                    foreach (var consClass in consClasses)
                    {
                        foreach (var renameData in currentRpkRenames)
                        {
                            if (consClass.ReplaceRpkReference(ClassJavaPair.SlrRelativeRpkNameFromRpkExternalReferenceString(renameData.wasRefString), renameData.wasTypeIDLocalPart,
                                                              ClassJavaPair.SlrRelativeRpkNameFromRpkExternalReferenceString(renameData.newRefString), renameData.newTypeIDLocalPart))
                            {
                                javasThatShouldBeSaved[clsJav.GetClassFileName()] = clsJav;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageLog.AddError("Could not get Consistent Classes for: " + clsJav.GetClassFileName() + "\r\nMsg: " + e.Message);
                }
            }
            //Move all entries to rpkToMergeTo
            rpkToMergeToData.Entries.AddRange(tempRpk.Entries);
        }

        private List<string> mergedIntoRpksASExtRefs = new List<string>();
    }
}