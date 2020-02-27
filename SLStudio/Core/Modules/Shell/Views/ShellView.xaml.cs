using SLStudio.Core.Resources.Sounds;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace SLStudio.Core.Modules.Shell.Views
{
    public partial class ShellView
    {
        private const string DEFAULT_CHEAT_STRING = "ilikecheating";
        private const string DEFAULT_CHEAT_STRING2 = "begformoney";
        private string typedString = string.Empty;

        public ShellView()
        {
            InitializeComponent();
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            new SoundPlayer(SoundResources.mnumove).Play();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            new SoundPlayer(SoundResources.mnuselect).Play();
            VerifyCheat();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            typedString = $"{typedString}{e.Key.ToString()}";
        }

        private void VerifyCheat()
        {
            if (typedString.Equals(DEFAULT_CHEAT_STRING, System.StringComparison.InvariantCultureIgnoreCase) ||
                typedString.Equals(DEFAULT_CHEAT_STRING2, System.StringComparison.InvariantCultureIgnoreCase))
            {
                new SoundPlayer(SoundResources.dayrace_loose).Play();
                //IoC.Get<IStatusBar>().Status = "You lose :D";
                //MessageBox.Show(this, "Fon", "SehLoiro Studio", MessageBoxButton.OK, MessageBoxImage.Question);
            }

            typedString = string.Empty;
        }
    }
}
