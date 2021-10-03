using System.Collections.Generic;

namespace SlrrLib.Model.HighLevel
{
    public class CfgSlotReference : Comparer<CfgSlotReference>
    {
        public int TypeID
        {
            get;
            private set;
        }

        public int SlotID
        {
            get;
            private set;
        }

        public CfgSlotReference(CfgLine ln)
        {
            TypeID = ln.Tokens[1].ValueAsHexInt;
            SlotID = ln.Tokens[2].ValueAsInt;
        }

        public CfgSlotReference(int typeID, int slotID)
        {
            SlotID = slotID;
            TypeID = typeID;
        }

        public static bool IsValidLine(CfgLine ln)
        {
            return ln.Tokens[1].IsValueHexInt && ln.Tokens[2].IsValueInt && ln.Tokens.Count >= 3;
        }

        public static bool operator <(CfgSlotReference l, CfgSlotReference r)
        {
            if (l.TypeID == r.TypeID)
                return l.SlotID < r.SlotID;
            return l.TypeID < r.TypeID;
        }

        public static bool operator >(CfgSlotReference l, CfgSlotReference r)
        {
            if (l.TypeID == r.TypeID)
                return l.SlotID > r.SlotID;
            return l.TypeID > r.TypeID;
        }

        public static bool operator ==(CfgSlotReference l, CfgSlotReference r)
        {
            return l.TypeID == r.TypeID && l.SlotID == r.SlotID;
        }

        public static bool operator !=(CfgSlotReference l, CfgSlotReference r)
        {
            return l.TypeID != r.TypeID || l.SlotID != r.SlotID;
        }

        public override bool Equals(object obj)
        {
            if (obj is CfgSlotReference)
            {
                var objCorr = obj as CfgSlotReference;
                return this == objCorr;
            }
            return base.Equals(obj);
        }

        public override int Compare(CfgSlotReference x, CfgSlotReference y)
        {
            if (x < y)
                return -1;
            if (x > y)
                return 1;
            return 0;
        }

        public override int GetHashCode()
        {
            return TypeID.GetHashCode() ^ SlotID.GetHashCode();
        }

        public override string ToString()
        {
            return "(0x" + TypeID.ToString("X8") + "-" + SlotID.ToString() + ")";
        }
    }
}