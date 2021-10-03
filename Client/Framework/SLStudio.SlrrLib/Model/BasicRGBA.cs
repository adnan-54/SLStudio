namespace SlrrLib.Model
{
    public struct BasicRGBA
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public override string ToString()
        {
            return R.ToString() + ", " + G.ToString() + ", " + B.ToString() + ", " + A.ToString();
        }
    }
}