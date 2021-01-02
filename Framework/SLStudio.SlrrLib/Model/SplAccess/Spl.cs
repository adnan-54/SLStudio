using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlrrLib.Model
{
    public class Spl
    {
        private string originalFileName = "";

        public List<SplLine> SplLines
        {
            get;
            private set;
        } = new List<SplLine>();

        public Spl()
        {
        }

        public Spl(string fnam)
        {
            originalFileName = fnam;
            readData();
        }

        public static SplNode GetNormal(SplNode point0, SplNode point1, SplNode point3)
        {
            var norm = (point3 - point1) + (point1 - point0) * 0.5f;
            norm.Normalize();
            return norm;
        }

        public SplLine PrevLineSkipCircular(int index)
        {
            if (IsCircular() && SplLines.Count > 1)
            {
                return SplLines[(index - 1) < 0 ? SplLines.Count - 2 : index - 1];
            }
            else
            {
                return SplLines[(index - 1) < 0 ? SplLines.Count - 1 : index - 1];
            }
        }

        public SplLine NextLineSkipCircular(int index)
        {
            if (IsCircular() && SplLines.Count > 1)
            {
                return SplLines[(index + 1) >= SplLines.Count ? 1 : index + 1];
            }
            else
            {
                return SplLines[(index + 1) >= SplLines.Count ? 0 : index + 1];
            }
        }

        public void FillNormalsFromPositiondata()
        {
            for (int i = 0; i != SplLines.Count; ++i)
            {
                var a = SplLines[i].position;
                var b = SplLines[(i + 1) % SplLines.Count].position;
                var c = SplLines[(i + 2) % SplLines.Count].position;

                var di = (b - a).Length();
                var dj = (c - b).Length();
                var l = di + dj;
                di = di * (1.0f / (l));
                dj = dj * (1.0f / (l));

                var p1 = di * a + dj * b;
                var p2 = dj * b + di * c;

                var norm = p2 - p1;
                if (norm.Length() < 0.001f)
                {
                    SplLines[(i + 1) % SplLines.Count].normal = SplLines[(i) % SplLines.Count].normal;
                }
                else
                {
                    norm.Normalize();
                    SplLines[(i + 1) % SplLines.Count].normal = norm;
                }
            }
        }

        public bool IsCircular()
        {
            if (SplLines == null || !SplLines.Any())
                return false;
            var first = SplLines[0];
            var last = SplLines[SplLines.Count - 1];
            return !(first.position.x != last.position.x ||
                     first.position.y != last.position.y ||
                     first.position.z != last.position.z);
        }

        public void MakeCircularIfNeeded()
        {
            if (!IsCircular())
            {
                var toad = new SplLine();
                toad.normal = new SplNode(SplLines[SplLines.Count - 1].normal);
                toad.position = new SplNode(SplLines[SplLines.Count - 1].position);
                toad.additionalAttributes = new SplAttributes(SplLines[SplLines.Count - 1].additionalAttributes);
                SplLines.Insert(0, toad);
            }
        }

        public void Reverse()
        {
            SplLines.Reverse();
        }

        public void Save(string fnam = null, bool bak = true)
        {
            if (fnam == null)
                fnam = originalFileName;
            if (bak && File.Exists(fnam))
            {
                int bakInd = 0;
                while (File.Exists(fnam + "_BAK_Spl_" + bakInd.ToString()))
                    bakInd++;
                File.Copy(fnam, fnam + "_BAK_Spl_" + bakInd.ToString());
            }
            File.WriteAllText(fnam, SplLines.Select(x => x.position.ToString() + "\t" + x.normal.ToString() + "\t" + x.additionalAttributes.ToString() + "\t").Aggregate((x, y) => x + "\r\n" + y));
        }

        private void readData()
        {
            if (!File.Exists(originalFileName))
            {
                MessageLog.AddError("Spl file does not exist: " + originalFileName, originalFileName, 0);
                return;
            }
            foreach (var line in File.ReadAllLines(originalFileName))
            {
                SplLines.Add(new SplLine(line));
            }
        }
    }
}