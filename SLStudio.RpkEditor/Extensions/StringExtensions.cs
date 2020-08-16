namespace SLStudio.RpkEditor
{
    public static class StringExtensions
    {
        public static int ToIntId(this string id)
        {
            var sanitized = id.Replace("0x", "");
            return int.Parse(sanitized);
        }
    }
}