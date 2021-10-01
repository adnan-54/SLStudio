using System;
using System.Collections.Generic;

namespace SlrrLib.Model.HighLevel
{
    public class RpkSlotReference : Comparer<RpkSlotReference>
    {
        private string tostringCache;
        private int? requestedHashCode;

        public RpkManager Rpk
        {
            get;
            private set;
        }

        public CfgSlotReference SlotRef
        {
            get;
            private set;
        }

        public RpkSlotReference(RpkManager rpk, int typeID, int slotID)
        {
            Rpk = rpk;
            SlotRef = new CfgSlotReference(typeID, slotID);
            tostringCache = rpk.GetRpkAsRPKRefString() + "|" + SlotRef.ToString();
        }

        public override string ToString()
        {
            return tostringCache;
        }

        public override int Compare(RpkSlotReference x, RpkSlotReference y)
        {
            return string.Compare(x.tostringCache, y.tostringCache);
        }

        public override int GetHashCode()
        {
            if (!requestedHashCode.HasValue)
            {
                requestedHashCode = StringComparer.InvariantCulture.GetHashCode(tostringCache);
            }
            return requestedHashCode.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is RpkSlotReference)
            {
                return Compare(obj as RpkSlotReference, this) == 0;
            }
            return base.Equals(obj);
        }
    }
}