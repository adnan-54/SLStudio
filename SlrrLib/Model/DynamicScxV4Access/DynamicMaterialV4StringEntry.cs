using System;
using System.IO;
using System.Text;

namespace SlrrLib.Model
{
    public class DynamicMaterialV4StringEntry : DynamicMaterialV4Entry
    {
        private string entryData;

        public string Text
        {
            get
            {
                return entryData;
            }
            set
            {
                string realSet = value;
                if (realSet.Length > 32)
                    realSet = value.Remove(32);
                while (realSet.Length != 32)
                    realSet += "\0";
                entryData = realSet;
            }
        }

        public DynamicMaterialV4StringEntry(BinaryMaterialV4Entry constructFrom = null)
        : base(constructFrom)
        {
            base.type = 2048;
            if (constructFrom == null)
                return;
            UnknownFlag = constructFrom.UnknownFlag;
            type = constructFrom.Type;
            Text = constructFrom.DataAsString;

            if (constructFrom.Size != Size)
                throw new Exception("HeaderWill Mismatch");
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(UnknownFlag);
            bw.Write(this.type);
            byte[] towrite = new byte[32];
            Array.Copy(ASCIIEncoding.ASCII.GetBytes(entryData), towrite, entryData.Length);
            bw.Write(towrite);
        }
    }
}