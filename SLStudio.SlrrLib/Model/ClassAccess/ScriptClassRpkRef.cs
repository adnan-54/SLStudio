namespace SlrrLib.Model
{
    public class ScriptClassRpkRef
    {
        public string SlrrRelativeRpkName
        {
            get;
            private set;
        }

        public int TypeIdInRpk
        {
            get;
            private set;
        }

        public ScriptClassRpkRef(string rpkName, int typeID)
        {
            SlrrRelativeRpkName = rpkName;
            TypeIdInRpk = typeID;
        }
    }
}