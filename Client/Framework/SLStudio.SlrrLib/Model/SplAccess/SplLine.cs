using System;

namespace SlrrLib.Model
{
    public class SplLine
    {
        public SplNode position = new SplNode();
        public SplNode normal = new SplNode();
        public SplAttributes additionalAttributes = new SplAttributes();

        public SplLine(string line)
        {
            var spl = line.Split(new string[] { " ", "\t", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (spl.Length > 0)
            {
                position.x = parseOrZero(spl[0]);
            }
            if (spl.Length > 1)
            {
                position.y = parseOrZero(spl[1]);
            }
            if (spl.Length > 2)
            {
                position.z = parseOrZero(spl[2]);
            }
            if (spl.Length > 3)
            {
                normal.x = parseOrZero(spl[3]);
            }
            if (spl.Length > 4)
            {
                normal.y = parseOrZero(spl[4]);
            }
            if (spl.Length > 5)
            {
                normal.z = parseOrZero(spl[5]);
            }
            if (spl.Length > 6)
            {
                additionalAttributes.splineWidth = parseOrZero(spl[6]);
            }
            if (spl.Length > 7)
            {
                additionalAttributes.speedRatio = parseOrZero(spl[7]);
            }
            if (spl.Length > 8)
            {
                additionalAttributes.unknown = parseOrZeroInt(spl[8]);
            }
        }

        public SplLine()
        {
        }

        public override string ToString()
        {
            return position.ToString() + "\t" + normal.ToString() + "\t" + additionalAttributes.ToString();
        }

        private float parseOrZero(string str)
        {
            try
            {
                return float.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int parseOrZeroInt(string str)
        {
            try
            {
                return int.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}