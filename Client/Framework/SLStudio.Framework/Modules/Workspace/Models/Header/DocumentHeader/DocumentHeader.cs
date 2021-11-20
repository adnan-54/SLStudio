namespace SLStudio
{
    public class DocumentHeader : WorkspaceHeader, IDocumentHeader
    {
        public object IconSource
        {
            get => GetValue<object>();
            set => SetValue(value);
        }

        public string ToolTip
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool Pinned
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }
    }
}
