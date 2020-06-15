namespace SlrrLib.Model
{
    public struct BasicUV
    {
        public float U;
        public float V;

        public override string ToString()
        {
            return U.ToString("F3") + ", " + V.ToString("F3");
        }
    }
}