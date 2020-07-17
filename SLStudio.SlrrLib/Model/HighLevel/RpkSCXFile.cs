using System;
using System.Collections.Generic;

namespace SlrrLib.Model.HighLevel
{
    public class RpkSCXFile : Comparer<RpkSCXFile>, IEquatable<RpkSCXFile>
    {
        private int? requestedHashCode;

        public RpkManager Rpk
        {
            get;
            private set;
        }

        public string ScxFullFnam
        {
            get;
            set;
        }

        public RpkSCXFile(RpkManager rpkManag, string scxFullFileName)
        {
            Rpk = rpkManag;
            ScxFullFnam = scxFullFileName;
        }

        public override string ToString()
        {
            return ScxFullFnam;
        }

        public override int Compare(RpkSCXFile x, RpkSCXFile y)
        {
            return string.Compare(x.ScxFullFnam, y.ScxFullFnam);
        }

        public override int GetHashCode()
        {
            if (!requestedHashCode.HasValue)
            {
                requestedHashCode = StringComparer.InvariantCulture.GetHashCode(ScxFullFnam);
            }
            return requestedHashCode.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is RpkSCXFile)
            {
                return Compare(obj as RpkSCXFile, this) == 0;
            }
            return base.Equals(obj);
        }

        public bool Equals(RpkSCXFile obj)
        {
            return Compare(obj as RpkSCXFile, this) == 0;
        }
    }
}