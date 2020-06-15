using SlrrLib.Model;
using System;
using System.Linq;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class NativeGeometryObjResEntryContext
    {
        public DynamicResEntry res;
        public DynamicRpk rpk;
        public Vector3D pos;
        public Vector3D ypr;
        public string prefix = "";
        public int asExternalTypeID = -1;

        public NativeGeometryObjResEntryContext(DynamicResEntry res, DynamicRpk rpk, Vector3D pos, Vector3D ypr)
        {
            this.res = res;
            this.rpk = rpk;
            this.pos = pos;
            this.ypr = ypr;
        }

        public int GetMehsTypeID()
        {
            int ret = -1;
            if (res == null)
                return ret;
            if (res.RSD.InnerEntries.First() is DynamicStringInnerEntry)
            {
                var strRsd = res.RSD.InnerEntries.First() as DynamicStringInnerEntry;
                var spl = strRsd.StringData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));
                foreach (var line in spl)
                {
                    if (line[0].ToLower() == "mesh")
                    {
                        try
                        {
                            return int.Parse(line[1].Replace("0x", ""), System.Globalization.NumberStyles.HexNumber);
                        }
                        catch (Exception) { }
                    }
                }
            }
            return ret;
        }

        public string GetSourcefile()
        {
            string ret = "";
            if (res == null)
                return "";
            if (res.RSD.InnerEntries.First() is DynamicStringInnerEntry)
            {
                var strRsd = res.RSD.InnerEntries.First() as DynamicStringInnerEntry;
                var spl = strRsd.StringData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));
                foreach (var line in spl)
                {
                    if (line[0].ToLower() == "sourcefile")
                    {
                        return line[1];
                    }
                }
            }
            return ret;
        }

        public override string ToString()
        {
            if (asExternalTypeID != -1)
                return prefix + "0x" + asExternalTypeID.ToString("X8") + "  |  " + res.Alias;
            return prefix + "0x" + res.TypeID.ToString("X8") + "  |  " + res.Alias;
        }
    }
}