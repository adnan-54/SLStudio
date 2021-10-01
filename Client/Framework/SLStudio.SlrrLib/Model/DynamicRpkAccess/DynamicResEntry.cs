using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class DynamicResEntry
    {
        public int SuperID
        {
            get;
            set;
        }

        public int SuperIDExternalRefPart
        {
            get
            {
                return SuperID >> 16;
            }
            set
            {
                int idInExternalRPK = SuperID & 0x0000FFFF;
                SuperID = (value << 16) & idInExternalRPK;
            }
        }

        public int SuperIDExternalIDPart
        {
            get
            {
                return SuperID & 0x0000FFFF;
            }
            set
            {
                int refPart = SuperID >> 16;
                SuperID = refPart & (value & 0x0000FFFF);
            }
        }

        public int TypeID
        {
            get;
            set;
        }

        public int TypeIDExternalRefPart
        {
            get
            {
                return TypeID >> 16;
            }
            set
            {
                int idInExternalRPK = TypeID & 0x0000FFFF;
                TypeID = (value << 16) & idInExternalRPK;
            }
        }

        public int TypeIDExternalIDPart
        {
            get
            {
                return TypeID & 0x0000FFFF;
            }
            set
            {
                int refPart = TypeID >> 16;
                TypeID = refPart & (value & 0x0000FFFF);
            }
        }

        public byte TypeOfEntry
        {
            get;
            set;
        }

        public byte AdditionalType
        {
            get;
            set;
        }

        public float IsParentCompatible
        {
            get;
            set;
        }

        public string Alias
        {
            get;
            set;
        }

        public int Size
        {
            get
            {
                int additionalFloatsLength = 0;
                if (AdditionalFloatList != null)
                    additionalFloatsLength = 48;
                return 23 + getAliasLength() + additionalFloatsLength;
            }
        }

        public DynamicRsdEntry RSD
        {
            get;
            set;
        }

        public List<float> AdditionalFloatList
        {
            get;
            set;
        }//count should be 12

        public DynamicResEntry()
        : this(null)
        {
        }

        public DynamicResEntry(BinaryResEntry from = null)
        {
            if (from == null)
                return;
            SuperID = from.SuperID;
            TypeID = from.TypeID;
            TypeOfEntry = from.TypeOfEntry;
            AdditionalType = from.AdditionalType;
            AdditionalFloatList = null;
            if (from.AdditionalFloatsPresent)
            {
                AdditionalFloatList = new List<float>();
                AdditionalFloatList.AddRange(from.AdditionalFloatList);
            }
            IsParentCompatible = from.IsParentCompatible;
            Alias = from.Alias.TrimEnd('\0');
            from.LoadDanglingHiddenEntries();//they will be used in the RSD
            if (from.RSD != null)
            {
                RSD = new DynamicRsdEntry(from.RSD);
            }
            foreach (var hEntry in from.DanglingHiddenEntries)
            {
                if (hEntry is BinarySpatialNode)
                    RSD.HiddenEntries.Add(new DynamicSpatialNode(hEntry as BinarySpatialNode));
                if (hEntry is BinaryInnerPhysEntry)
                    RSD.HiddenEntries.Add(new DynamicInnerPhysEntry(hEntry as BinaryInnerPhysEntry));
                if (hEntry is BinaryInnerPolyEntry)
                    RSD.HiddenEntries.Add(new DynamicInnerPolyEntry(hEntry as BinaryInnerPolyEntry));
                if (hEntry is BinaryRSDInnerEntry)
                    RSD.HiddenEntries.Add(new DynamicRSDInnerEntry(hEntry as BinaryRSDInnerEntry));
                if (hEntry is BinaryEXTPInnerEntry)
                    RSD.HiddenEntries.Add(new DynamicEXTPInnerEntry(hEntry as BinaryEXTPInnerEntry));
            }
        }

        public void Save(BinaryWriter bw, int offsetOfRSD)
        {
            bw.Write(SuperID);
            bw.Write(TypeID);
            bw.Write(TypeOfEntry);
            bw.Write(AdditionalType);
            bw.Write(IsParentCompatible);
            bw.Write(offsetOfRSD);
            bw.Write(RSD.Size);
            setAliasLength();
            bw.Write((byte)Alias.Length);
            bw.Write(ASCIIEncoding.ASCII.GetBytes(Alias));
            if (AdditionalFloatList != null)
            {
                while (AdditionalFloatList.Count < 12)
                    AdditionalFloatList.Add(0.0f);
                while (AdditionalFloatList.Count > 12)
                    AdditionalFloatList.RemoveAt(AdditionalFloatList.Count - 1);
                foreach (var f in AdditionalFloatList)
                    bw.Write(f);
            }
        }

        public override string ToString()
        {
            if (RSD.InnerEntries.Count == 1 && RSD.InnerEntries.First() is DynamicStringInnerEntry)
                return Alias + (RSD.InnerEntries.First() as DynamicStringInnerEntry).StringData;
            return Alias;
        }

        private void setAliasLength(int size = 32)
        {
            if (Alias.Length == 0)
                Alias = "\0";
            if (Alias.Last() != '\0')
                Alias += "\0";
            if (Alias.Length > size)
                Alias = Alias.Substring(0, size);
            Alias = Alias.Substring(0, Alias.Length - 1) + "\0";
        }

        private byte getAliasLength()
        {
            if (Alias.Length == 0)
                return 0;
            if (Alias.Last() == '\0')
                return (byte)Alias.Length;
            return (byte)(Alias.Length + 1);
        }
    }
}