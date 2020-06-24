namespace SlrrLib.Model
{
    public class SplAttributes
    {
        public float splineWidth;
        public float speedRatio;
        public int unknown;

        public override string ToString()
        {
            return splineWidth.ToString("F4") + " " + speedRatio.ToString("F4") + " " + unknown.ToString();
        }

        public SplAttributes(float splineWidth, float speedRatio, int unknown)
        {
            this.splineWidth = splineWidth;
            this.speedRatio = speedRatio;
            this.unknown = unknown;
        }

        public SplAttributes()
        {
        }

        public SplAttributes(SplAttributes other)
        {
            splineWidth = other.splineWidth;
            speedRatio = other.speedRatio;
            unknown = other.unknown;
        }
    }
}