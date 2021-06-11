using SLStudio;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window, ISplashScreenView
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        public bool IsShowing { get; private set; }

        public string Status
        {
            get => StatusText.Text;
            set => StatusText.Text = value;
        }

        void ISplashScreenView.Show()
        {
            IsShowing = true;
            Show();
        }

        void ISplashScreenView.Close()
        {
            IsShowing = false;
            Close();
        }

        private int count;
        private Task task;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (task != null)
                return;
            task = Test();
            await task;
            count = 0;
            task.Dispose();
            task = null;
        }

        private async Task Test()
        {
            while (count < 1000)
            {
                IoC.Get<ISplashScreen>().UpdateStatus("{0}", count++);
                await Task.Delay(1);
            }
        }
    }
}