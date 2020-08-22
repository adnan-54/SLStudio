namespace SLStudio.RpkEditor
{
    public static class IntExtensions
    {
        public static string ToStringId(this int id)
        {
            if (id < 0)
                return string.Empty;

            return $"0x{id:X8}";
        }
    }
}