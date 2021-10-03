namespace SlrrLib.Model
{
    public class CfgPartPosRotLine : CfgPartPositionLine
    {
        public float LineRotX
        {
            get
            {
                if (PosLine.Tokens.Count > 4)
                    return PosLine.Tokens[4].ValueAsFloat;
                return 0.0f;
            }
            set
            {
                PosLine.FixCountFromFormatDescriptor(5);
                PosLine.Tokens[4].ValueAsFloat = value;
            }
        }

        public float LineRotY
        {
            get
            {
                if (PosLine.Tokens.Count > 5)
                    return PosLine.Tokens[5].ValueAsFloat;
                return 0.0f;
            }
            set
            {
                PosLine.FixCountFromFormatDescriptor(6);
                PosLine.Tokens[5].ValueAsFloat = value;
            }
        }

        public float LineRotZ
        {
            get
            {
                if (PosLine.Tokens.Count > 6)
                    return PosLine.Tokens[6].ValueAsFloat;
                return 0.0f;
            }
            set
            {
                PosLine.FixCountFromFormatDescriptor(7);
                PosLine.Tokens[6].ValueAsFloat = value;
            }
        }

        public CfgPartPosRotLine(CfgLine line)
        : base(line)
        {
        }

        protected CfgPartPosRotLine()
        : base()
        {
        }
    }
}