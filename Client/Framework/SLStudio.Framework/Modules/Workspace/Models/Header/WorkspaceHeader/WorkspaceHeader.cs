using DevExpress.Mvvm;

namespace SLStudio
{
    public class WorkspaceHeader : BindableBase, IWorkspaceHeader
    {
        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string ToolTip
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public object Icon
        {
            get => GetValue<object>();
            set => SetValue(value);
        }
    }
}
