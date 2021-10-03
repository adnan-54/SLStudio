using System;
using System.Collections.Generic;
using System.Linq;

namespace SlrrLib.Model
{
    public class CfgSlotLine : CfgPartPosRotLine
    {
        private string weakParentCfgFnam = "NOT CONSTRUCTED FROM FILE";
        private List<CfgLine> attachLines_cache = new List<CfgLine>();
        private List<CfgLine> compatibleLines_cache = new List<CfgLine>();
        private CfgLine slotTypeLine = null;
        private CfgLine slotDmgModeLine = null;
        private CfgPartPosRotLine slotBonePosLine = null;
        private CfgLine slotDeformLine = null;
        private CfgPartPosRotLine slotFlapLine = null;
        private List<CfgLine> lnsInParent = null;
        private Cfg weakParent;

        public IEnumerable<CfgLine> AttachLines
        {
            get
            {
                return attachLines_cache;
            }
        }

        public IEnumerable<CfgLine> CompatibleLines
        {
            get
            {
                return compatibleLines_cache;
            }
        }

        public CfgLine SlotTypeLine
        {
            get
            {
                return slotTypeLine;
            }
            set
            {
                if (slotTypeLine != null && lnsInParent != null)
                {
                    lnsInParent.Remove(slotTypeLine);
                    if (value == null)
                    {
                        slotTypeLine = null;
                        return;
                    }
                }
                slotTypeLine = new CfgLine(value.ToString());
                if (lnsInParent != null)
                {
                    lnsInParent.Insert(lastIndexofSlot() + 1, slotTypeLine);
                }
            }
        }

        public CfgLine SlotDmgModeLine
        {
            get
            {
                return slotDmgModeLine;
            }
            set
            {
                if (slotDmgModeLine != null && lnsInParent != null)
                {
                    lnsInParent.Remove(slotDmgModeLine);
                    if (value == null)
                    {
                        slotDmgModeLine = null;
                        return;
                    }
                }
                slotDmgModeLine = new CfgLine(value.ToString());
                if (lnsInParent != null)
                {
                    lnsInParent.Insert(lastIndexofSlot() + 1, slotDmgModeLine);
                }
            }
        }

        public CfgPartPosRotLine SlotBonePosLine
        {
            get
            {
                return slotBonePosLine;
            }
            set
            {
                if (slotBonePosLine != null && lnsInParent != null)
                {
                    lnsInParent.Remove(slotBonePosLine.PosLine);
                    if (value == null)
                    {
                        slotBonePosLine = null;
                        return;
                    }
                }
                slotBonePosLine = new CfgPartPosRotLine(new CfgLine(value.ToString()));
                if (lnsInParent != null)
                {
                    lnsInParent.Insert(lastIndexofSlot() + 1, slotBonePosLine.PosLine);
                }
            }
        }

        public CfgLine SlotDeformLine
        {
            get
            {
                return slotDeformLine;
            }
            set
            {
                if (slotDeformLine != null && lnsInParent != null)
                {
                    lnsInParent.Remove(slotDeformLine);
                    if (value == null)
                    {
                        slotDeformLine = null;
                        return;
                    }
                }
                slotDeformLine = new CfgLine(value.ToString());
                if (lnsInParent != null)
                {
                    lnsInParent.Insert(lastIndexofSlot() + 1, slotDeformLine);
                }
            }
        }

        public CfgPartPosRotLine SlotFlapLine
        {
            get
            {
                return slotFlapLine;
            }
            set
            {
                if (slotFlapLine != null && lnsInParent != null)
                {
                    lnsInParent.Remove(slotFlapLine.PosLine);
                    if (value == null)
                    {
                        slotFlapLine = null;
                        return;
                    }
                }
                slotFlapLine = new CfgPartPosRotLine(new CfgLine(value.ToString()));
                if (lnsInParent != null)
                {
                    lnsInParent.Insert(lastIndexofSlot() + 1, slotFlapLine.PosLine);
                }
            }
        }

        public int SlotID
        {
            get
            {
                if (PosLine.Tokens.Count > 7)
                    return PosLine.Tokens[7].ValueAsInt;
                return 0;
            }
            set
            {
                PosLine.FixCountFromFormatDescriptor(8);
                PosLine.Tokens[7].ValueAsInt = value;
            }
        }

        public bool IsWheelSlot
        {
            get;
            private set;
        }

        public CfgSlotLine(Cfg parent, int indexOfSlotLine)
        {
            weakParent = parent;
            lnsInParent = new List<CfgLine>(parent.Lines);
            weakParentCfgFnam = parent.CfgFileName;
            initFromLineList(lnsInParent, indexOfSlotLine);
        }

        public CfgSlotLine(string multiLineSlotStructureInCfg)
        {
            weakParent = null;
            initFromLineList(multiLineSlotStructureInCfg.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(x => new CfgLine(x)).ToList());
        }

        public void AddAttach(int rpkRefID, int slotID)
        {
            var toad = new CfgLine("attach\t\t0x" + rpkRefID.ToString("X8") + " " + slotID.ToString());
            attachLines_cache.Add(toad);
            if (lnsInParent != null)
            {
                int toad_i = lastIndexOfNamedLineFromIndexUp(lastIndexofSlot(), "attach");
                if (toad_i == -1 || toad_i < firstIndexofSlot())
                    toad_i = lnsInParent.IndexOf(PosLine) + 1;
                lnsInParent.Insert(toad_i, toad);
            }
        }

        public void RemoveAttach(int rpkRefID, int slotID)
        {
            var mathcingLines = attachLines_cache.Where(x => x.Tokens[1].ValueAsHexInt == rpkRefID && x.Tokens[2].ValueAsInt == slotID).ToList();
            foreach (var ln in mathcingLines)
            {
                if (lnsInParent != null)
                    lnsInParent.Remove(ln);
                attachLines_cache.Remove(ln);
            }
        }

        public void AddCompatible(int rpkRefID, int slotID)
        {
            var toad = new CfgLine("compatible\t\t0x" + rpkRefID.ToString("X8") + " " + slotID.ToString());
            compatibleLines_cache.Add(toad);
            if (lnsInParent != null)
            {
                int toad_i = lastIndexOfNamedLineFromIndexUp(lastIndexofSlot(), "compatible");
                if (toad_i == -1 || toad_i < firstIndexofSlot())
                    toad_i = lnsInParent.IndexOf(PosLine) + 1;
                lnsInParent.Insert(toad_i, toad);
            }
        }

        public void RemoveCompatible(int rpkRefID, int slotID)
        {
            var mathcingLines = compatibleLines_cache.Where(x => x.Tokens[1].ValueAsHexInt == rpkRefID && x.Tokens[2].ValueAsInt == slotID).ToList();
            foreach (var ln in mathcingLines)
            {
                if (lnsInParent != null)
                    lnsInParent.Remove(ln);
                compatibleLines_cache.Remove(ln);
            }
        }

        private int lastIndexOfNamedLineFromIndexUp(int indexUp, string name)
        {
            if (lnsInParent == null)
                return -1;
            for (int ln_i = indexUp; ln_i != -1; ln_i--)
            {
                if (lnsInParent[ln_i].NameStr.ToLower() == name.ToLower())
                    return ln_i;
            }
            return -1;
        }

        private int lastIndexofSlot()
        {
            if (lnsInParent == null)
                return -1;
            List<int> indices = new List<int>();
            indices.AddRange(attachLines_cache.Select(x => lnsInParent.IndexOf(x)));
            indices.AddRange(compatibleLines_cache.Select(x => lnsInParent.IndexOf(x)));
            if (slotTypeLine != null)
                indices.Add(lnsInParent.IndexOf(slotTypeLine));
            if (slotDmgModeLine != null)
                indices.Add(lnsInParent.IndexOf(slotDmgModeLine));
            if (slotBonePosLine != null)
                indices.Add(lnsInParent.IndexOf(slotBonePosLine.PosLine));
            if (slotDeformLine != null)
                indices.Add(lnsInParent.IndexOf(slotDeformLine));
            if (slotFlapLine != null)
                indices.Add(lnsInParent.IndexOf(slotFlapLine.PosLine));
            if (PosLine != null)
                indices.Add(lnsInParent.IndexOf(PosLine));
            indices.RemoveAll(x => x == -1);
            return indices.Max();
        }

        private int firstIndexofSlot()
        {
            if (lnsInParent == null)
                return -1;
            List<int> indices = new List<int>();
            indices.AddRange(attachLines_cache.Select(x => lnsInParent.IndexOf(x)));
            indices.AddRange(compatibleLines_cache.Select(x => lnsInParent.IndexOf(x)));
            if (slotTypeLine != null)
                indices.Add(lnsInParent.IndexOf(slotTypeLine));
            if (slotDmgModeLine != null)
                indices.Add(lnsInParent.IndexOf(slotDmgModeLine));
            if (slotBonePosLine != null)
                indices.Add(lnsInParent.IndexOf(slotBonePosLine.PosLine));
            if (slotDeformLine != null)
                indices.Add(lnsInParent.IndexOf(slotDeformLine));
            if (slotFlapLine != null)
                indices.Add(lnsInParent.IndexOf(slotFlapLine.PosLine));
            if (PosLine != null)
                indices.Add(lnsInParent.IndexOf(PosLine));
            indices.RemoveAll(x => x == -1);
            return indices.Min();
        }

        private void initFromLineList(List<CfgLine> allLines, int indexOfSlotLine = 0)
        {
            IsWheelSlot = false;
            for (int ln_i = indexOfSlotLine - 1; ln_i >= 0; ln_i--)
            {
                if (allLines[ln_i].NameStr == "slot")
                {
                    break;
                }
                if (allLines[ln_i].NameStr == "wheel")
                {
                    IsWheelSlot = true;
                    break;
                }
            }
            PosLine = allLines[indexOfSlotLine];
            int ad_i = indexOfSlotLine;
            while (true)
            {
                ad_i++;
                if (allLines.Count <= ad_i)
                    break;
                if (allLines[ad_i].IsEmpty)
                    continue;
                if (allLines[ad_i].Tokens[0].IsComment)
                    continue;
                if (allLines[ad_i].NameStr == "attach")
                {
                    attachLines_cache.Add(allLines[ad_i]);
                    continue;
                }
                if (allLines[ad_i].NameStr == "compatible")
                {
                    compatibleLines_cache.Add(allLines[ad_i]);
                    continue;
                }
                if (allLines[ad_i].NameStr == "slottype")
                {
                    if (slotTypeLine != null)
                    {
                        MessageLog.AddError("Multiple slottype lines in slot: " + PosLine.ToString() + "\n\tin cfg: " + weakParentCfgFnam, weakParentCfgFnam, 0);
                        continue;
                    }
                    slotTypeLine = allLines[ad_i];
                    continue;
                }
                if (allLines[ad_i].NameStr == "slotdmgmode")
                {
                    if (slotDmgModeLine != null)
                    {
                        MessageLog.AddError("Multiple slotdmgmode lines in slot: " + PosLine.ToString() + "\n\tin cfg: " + weakParentCfgFnam, weakParentCfgFnam, 0);
                        continue;
                    }
                    slotDmgModeLine = allLines[ad_i];
                    continue;
                }
                if (allLines[ad_i].NameStr == "slotdeform")
                {
                    if (slotDeformLine != null)
                    {
                        MessageLog.AddError("Multiple slotdeform lines in slot: " + PosLine.ToString() + "\n\tin cfg: " + weakParentCfgFnam, weakParentCfgFnam, 0);
                        continue;
                    }
                    slotDeformLine = allLines[ad_i];
                    continue;
                }
                if (allLines[ad_i].NameStr == "flap")
                {
                    if (slotFlapLine != null)
                    {
                        MessageLog.AddError("Multiple flap lines in slot: " + PosLine.ToString() + "\n\tin cfg: " + weakParentCfgFnam, weakParentCfgFnam, 0);
                        continue;
                    }
                    slotFlapLine = new CfgPartPosRotLine(allLines[ad_i]);
                    continue;
                }
                if (allLines[ad_i].NameStr == "bonepos")
                {
                    if (slotBonePosLine != null)
                    {
                        MessageLog.AddError("Multiple bonepos lines in slot: " + PosLine.ToString() + "\n\tin cfg: " + weakParentCfgFnam, weakParentCfgFnam, 0);
                        continue;
                    }
                    slotBonePosLine = new CfgPartPosRotLine(allLines[ad_i]);
                    continue;
                }
                break;
            }
        }
    }
}