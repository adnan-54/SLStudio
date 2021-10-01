namespace SlrrLib.Model
{
    public class DynamicLineNameValue
    {
        public string Key
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public DynamicLineNameValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}