using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlrrLib.Model
{
    public class Cfg
    {
        private string cfgFnam = "";
        private string cfgCache = null;
        private List<CfgLine> cfgLines = new List<CfgLine>();

        public string CfgFileName
        {
            get
            {
                return cfgFnam;
            }
        }

        public IEnumerable<CfgLine> Lines
        {
            get
            {
                return cfgLines;
            }
            private set
            {
                cfgLines = new List<CfgLine>(value);
            }
        }

        public IEnumerable<CfgPartPositionLine> LinesWithPositionData
        {
            get
            {
                List<CfgPartPositionLine> ret = new List<CfgPartPositionLine>();
                foreach (var ln in cfgLines)
                {
                    if (ln.IsEmpty)
                        continue;
                    if (ln.Tokens[0].IsComment)
                        continue;
                    if (ln.NameStr == "spring" ||
                        ln.NameStr == "pedals" ||
                        ln.NameStr == "bonepos" ||
                        ln.NameStr == "flap")
                    {
                        ret.Add(new CfgPartPositionLine(ln));
                    }
                    if (ln.NameStr == "body")
                        ret.Add(new CfgBodyLine(ln));
                    if (ln.NameStr == "slot" ||
                        ln.NameStr == "wheel" ||
                        ln.NameStr == "steering" ||
                        ln.NameStr == "seat" ||
                        ln.NameStr == "camera" ||
                        ln.NameStr == "bonepos")
                    {
                        ret.Add(new CfgPartPosRotLine(ln));
                    }
                    if (ln.NameStr == "render" && ln.Tokens.Count >= 5)
                    {
                        ret.Add(new CfgRenderLine(ln));
                    }
                }
                return ret;
            }
        }

        public IEnumerable<CfgSlotLine> Slots
        {
            get
            {
                List<CfgSlotLine> ret = new List<CfgSlotLine>();
                foreach (var ln in cfgLines)
                {
                    if (ln.IsEmpty)
                        continue;
                    if (ln.Tokens[0].IsComment)
                        continue;
                    if (ln.NameStr == "slot")
                    {
                        ret.Add(new CfgSlotLine(this, cfgLines.IndexOf(ln)));
                    }
                }
                return ret;
            }
        }

        public IEnumerable<CfgBodyLine> BodyLines
        {
            get
            {
                List<CfgBodyLine> ret = new List<CfgBodyLine>();
                foreach (var ln in cfgLines)
                {
                    if (ln.IsEmpty)
                        continue;
                    if (ln.Tokens[0].IsComment)
                        continue;
                    if (ln.NameStr == "body")
                    {
                        ret.Add(new CfgBodyLine(ln));
                    }
                }
                return ret;
            }
        }

        public IEnumerable<int> ReferencedRpkTypeIDs
        {
            get
            {
                List<int> ret = new List<int>();
                foreach (var ln in cfgLines)
                {
                    if (ln.IsEmpty)
                        continue;
                    if (ln.Tokens[0].IsComment)
                        continue;
                    var format = ln.MatchedFormat().Split(' ');
                    for (int f_i = 0; f_i != format.Length; ++f_i)
                    {
                        if (format[f_i] == "R")
                            ret.Add(ln.Tokens[f_i].ValueAsHexInt);
                    }
                }
                return ret.Distinct();
            }
        }

        public Cfg(string fnam)
        {
            cfgFnam = fnam;
            LoadCfgFile();
        }

        public Cfg(string fnam, string fileContents)
        {
            cfgFnam = fnam;
            cfgCache = fileContents;
            LoadCfgFile();
        }

        public void ForceReLoadCfgFile()
        {
            cfgCache = null;
            LoadCfgFile();
        }

        public void LoadCfgFile()
        {
            if (cfgCache == null && File.Exists(cfgFnam))
                cfgCache = File.ReadAllText(cfgFnam);
            var spl = cfgCache.Replace("\r", "").Split(new string[] { "\n" }, StringSplitOptions.None);
            bool eofReached = false;
            foreach (var ln in spl)
            {
                CfgLine toad = new CfgLine(ln, eofReached);
                cfgLines.Add(toad);
                if (!toad.IsEmpty)
                {
                    if (toad.Tokens.First().Value.ToLower() == "eof")
                        eofReached = true;
                }
            }
        }

        public void OverWriteCfgFileName(string newName)
        {
            cfgFnam = newName.Replace("\\\\", "\\");
        }

        public void Save(bool bak = true, string bakSuffix = "_BAK_CfgScn_")
        {
            string cfgFnamLocal = FileEntry.GetWindowsPhysicalPath(cfgFnam);
            if (bak && File.Exists(cfgFnamLocal))
            {
                int bakInd = 0;
                while (File.Exists(cfgFnamLocal + bakSuffix + bakInd.ToString()))
                    bakInd++;
                File.Copy(cfgFnamLocal, cfgFnamLocal + bakSuffix + bakInd.ToString());
            }
            SaveAs(cfgFnamLocal);
        }

        public void SaveAs(string fnam)
        {
            StreamWriter sw = new StreamWriter(fnam);
            while (!cfgLines.Any() || cfgLines.Last().ToString() == "")
                cfgLines.RemoveAt(cfgLines.Count - 1);
            foreach (var ln in cfgLines)
            {
                ln.SaveTo(sw);
            }
            sw.Close();
        }

        public int GetTypeIDRefLineTypeIDFromName(string lineName)
        {
            var retLine = FirstLineWithName(lineName);
            if (retLine != null && retLine.Tokens.Count >= 2 && retLine.Tokens[1].IsValueHexInt)
            {
                return retLine.Tokens[1].ValueAsHexInt;
            }
            return -1;
        }

        public void AddLine(CfgLine toad)
        {
            cfgLines.Insert(FirstIndexOfNamedLine("eof"), new CfgLine(toad.ToString()));
        }

        public void AddLine(string toad)
        {
            cfgLines.Insert(FirstIndexOfNamedLine("eof"), new CfgLine(toad));
        }

        public void InsertLine(int index, CfgLine toad)
        {
            cfgLines.Insert(index, toad);
        }

        public void AddSlot(CfgSlotLine toad)
        {
            cfgLines.Insert(FirstIndexOfNamedLine("eof"), new CfgLine(toad.PosLine.ToString()));
            foreach (var at in toad.AttachLines)
                cfgLines.Insert(FirstIndexOfNamedLine("eof"), new CfgLine(at.ToString()));
            foreach (var cmp in toad.CompatibleLines)
                cfgLines.Insert(FirstIndexOfNamedLine("eof"), new CfgLine(cmp.ToString()));
            if (toad.SlotTypeLine != null)
                cfgLines.Insert(FirstIndexOfNamedLine("eof"), new CfgLine(toad.SlotTypeLine.ToString()));
            if (toad.SlotDmgModeLine != null)
                cfgLines.Insert(FirstIndexOfNamedLine("eof"), new CfgLine(toad.SlotDmgModeLine.ToString()));
            if (toad.SlotBonePosLine != null)
                cfgLines.Insert(FirstIndexOfNamedLine("eof"), new CfgLine(toad.SlotBonePosLine.PosLine.ToString()));
            if (toad.SlotDeformLine != null)
                cfgLines.Insert(FirstIndexOfNamedLine("eof"), new CfgLine(toad.SlotDeformLine.ToString()));
            if (toad.SlotFlapLine != null)
                cfgLines.Insert(FirstIndexOfNamedLine("eof"), new CfgLine(toad.SlotFlapLine.PosLine.ToString()));
        }

        public void RemoveSlot(CfgSlotLine torem)
        {
            cfgLines.Remove(torem.PosLine);
            foreach (var at in torem.AttachLines)
                cfgLines.Remove(at);
            foreach (var cmp in torem.CompatibleLines)
                cfgLines.Remove(cmp);
            if (torem.SlotTypeLine != null)
                cfgLines.Remove(torem.SlotTypeLine);
            if (torem.SlotTypeLine != null)
                cfgLines.Remove(torem.SlotDmgModeLine);
            if (torem.SlotTypeLine != null)
                cfgLines.Remove(torem.SlotBonePosLine.PosLine);
            if (torem.SlotTypeLine != null)
                cfgLines.Remove(torem.SlotDeformLine);
            if (torem.SlotTypeLine != null)
                cfgLines.Remove(torem.SlotFlapLine.PosLine);
        }

        public void ReplaceReference(int typeIDFrom, int typeIDTo)
        {
            foreach (var ln in cfgLines)
            {
                if (ln.IsEmpty)
                    continue;
                if (ln.Tokens[0].IsComment)
                    continue;
                var format = ln.MatchedFormat().Split(' ');
                for (int f_i = 0; f_i != format.Length; ++f_i)
                {
                    if (format[f_i] == "R" && ln.Tokens[f_i].ValueAsHexInt == typeIDFrom)
                        ln.Tokens[f_i].ValueAsHexInt = typeIDTo;
                }
            }
        }

        public int FirstIndexOfNamedLine(string name)
        {
            for (int ln_i = 0; ln_i != cfgLines.Count; ++ln_i)
            {
                if (cfgLines[ln_i].IsEmpty)
                    continue;
                if (cfgLines[ln_i].Tokens[0].IsComment)
                    continue;
                if (cfgLines[ln_i].NameStr.ToLower() == name.ToLower())
                    return ln_i;
            }
            return -1;
        }

        public CfgLine FirstLineWithName(string name)
        {
            int ret_i = FirstIndexOfNamedLine(name);
            if (ret_i == -1)
                return null;
            return cfgLines[ret_i];
        }

        public IEnumerable<IEnumerable<CfgLine>> GetMultiLineStructs(string identifierLineName, IEnumerable<string> subLineNames)
        {
            List<List<CfgLine>> ret = new List<List<CfgLine>>();
            List<CfgLine> toad = null;
            bool inStruct = false;
            for (int i = 0; i != cfgLines.Count; ++i)
            {
                if (cfgLines[i].NameStr == identifierLineName)
                {
                    if (inStruct)
                    {
                        ret.Add(toad);
                    }
                    inStruct = true;
                    toad = new List<CfgLine>
          {
            cfgLines[i]
          };
                    continue;
                }
                if (!inStruct)
                    continue;
                if (!subLineNames.Any(x => x == cfgLines[i].NameStr))
                {
                    inStruct = false;
                    ret.Add(toad);
                    continue;
                }
                else
                {
                    toad.Add(cfgLines[i]);
                }
            }
            if (inStruct)
                ret.Add(toad);
            return ret;
        }

        public int LastIndexOfNamedLine(string name)
        {
            for (int ln_i = cfgLines.Count - 1; ln_i != -1; --ln_i)
            {
                if (cfgLines[ln_i].IsEmpty)
                    continue;
                if (cfgLines[ln_i].Tokens[0].IsComment)
                    continue;
                if (cfgLines[ln_i].NameStr.ToLower() == name.ToLower())
                    return ln_i;
            }
            return -1;
        }

        public CfgLine LastLineWithName(string name)
        {
            int ret_i = LastIndexOfNamedLine(name);
            if (ret_i == -1)
                return null;
            return cfgLines[ret_i];
        }

        public IEnumerable<CfgLine> LinesWithName(string name)
        {
            List<CfgLine> ret = new List<CfgLine>();
            for (int ln_i = 0; ln_i != cfgLines.Count; ++ln_i)
            {
                if (cfgLines[ln_i].IsEmpty)
                    continue;
                if (cfgLines[ln_i].Tokens[0].IsComment)
                    continue;
                if (cfgLines[ln_i].NameStr.ToLower() == name.ToLower())
                    ret.Add(cfgLines[ln_i]);
            }
            return ret;
        }

        public override string ToString()
        {
            return System.IO.Path.GetFileNameWithoutExtension(CfgFileName);
        }
    }
}