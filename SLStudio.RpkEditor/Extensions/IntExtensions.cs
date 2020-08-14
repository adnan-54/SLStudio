namespace SLStudio.RpkEditor
{
    public static class IntExtensions
    {
        public static string ToStringId(this int id)
        {
            return $"0x{id:X8}";
        }
    }
}