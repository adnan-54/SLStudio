using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Modules.Shell.ViewModels
{
    [Export(typeof(IShell))]
    class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {

    }
}
