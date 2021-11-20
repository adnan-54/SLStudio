using DevExpress.Mvvm;

namespace SLStudio
{
    public abstract class WorkspaceHeader : BindableBase, IWorkspaceHeader
    {
        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}
