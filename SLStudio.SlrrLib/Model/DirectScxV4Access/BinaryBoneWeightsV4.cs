namespace SlrrLib.Model
{
    public struct BinaryBoneWeightsV4
    {
        public float First
        {
            get;
            set;
        }

        public float Second
        {
            get;
            set;
        }

        public float Third
        {
            get;
            set;
        }

        public float Implicit
        {
            get
            {
                return 1.0f - First - Second - Third;
            }
        }

        public void Normalize()
        {
            if (First < 0)
                First = 0;
            if (Second < 0)
                Second = 0;
            if (Third < 0)
                Third = 0;
            if (Implicit < 0)
            {
                float l = First + Second + Third;
                First /= l;
                Second /= l;
                Third /= l;
            }
        }

        public int ZeroCount()
        {
            Normalize();
            int ret = 0;
            if (First <= 0)
                ret++;
            if (Second <= 0)
                ret++;
            if (Third <= 0)
                ret++;
            if (Implicit <= 0)
                ret++;
            return ret;
        }

        public override string ToString()
        {
            return First.ToString("F3") + ", " + Second.ToString("F3") + ", " + Third.ToString("F3") + ", " + Implicit.ToString("F3");
        }
    }
}