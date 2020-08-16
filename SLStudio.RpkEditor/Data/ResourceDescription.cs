namespace SLStudio.RpkEditor.Data
{
    internal class ResourceDescription
    {
        public ResourceDescription(string text, bool highlight = false)
        {
            Text = text;
            Highlight = highlight;
        }

        public string Text { get; }
        public bool Highlight { get; }
    }
}