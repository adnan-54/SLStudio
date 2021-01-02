namespace SlrrLib.Model
{
    public class DynamicRpkExternalReference
    {
        public string ReferenceString
        {
            get;
            set;
        }

        public short IndexOfReference
        {
            get;
            set;
        }

        public short UnkownIndexZero
        {
            get;
            set;
        }

        public DynamicRpkExternalReference()
        : this(null)
        {
        }

        public DynamicRpkExternalReference(BinaryRpkExternalReference from = null)
        {
            if (from == null)
                return;
            ReferenceString = from.ReferenceString;
            IndexOfReference = from.IndexOfReference;
            UnkownIndexZero = from.UnkownIndexZero;
        }

        public override string ToString()
        {
            return ReferenceString;
        }
    }
}