namespace SlrrLib.Model
{
    public struct BinaryBoneIndRefV4
    {
        public byte First
        {
            get;
            set;
        }

        public byte Second
        {
            get;
            set;
        }

        public byte Third
        {
            get;
            set;
        }

        public byte Fourth
        {
            get;
            set;
        }

        public override string ToString()
        {
            return First.ToString("F3") + ", " + Second.ToString("F3") + ", " + Third.ToString("F3") + ", " + Fourth.ToString("F3");
        }
    }
}