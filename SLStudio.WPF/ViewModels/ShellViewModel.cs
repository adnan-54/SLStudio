using Caliburn.Micro;
using System.Data;
using System.Reflection;

namespace SLStudio.WPF.ViewModels
{
    public class ShellViewModel : Screen
    {
        static readonly Logging.ILog Log = Logging.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public DataTable LogFdp { get => Logging.LogManager.GetLog(); }
    }
}
