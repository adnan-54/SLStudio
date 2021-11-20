namespace SLStudio
{
    public class ToolHeader : WorkspaceHeader, IToolHeader
    {
        public object IconSource
        {
            get => GetValue<object>();
            set => SetValue(value);
        }
    }
}
