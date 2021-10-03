using System;
using System.Windows;
using SLStudio.Logging;

namespace SLStudio.ErrorReporting

{
    public partial class ExceptionBox : Window
    {
        private static readonly ILogger logger = LogManager.GetLogger<ExceptionBox>();

        private readonly string exception;
        private readonly bool isTerminating;

        public ExceptionBox(Exception exception, bool isTerminating)
        {
            this.exception = exception.ToString();
            this.isTerminating = isTerminating;

            InitializeComponent();
            UpdateState();
        }

        private void OnCopyToClipboardClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(exception);
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
            }
        }

        private void OnActionClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateState()
        {
            TextBoxException.Text = exception;

            var actionButton = isTerminating ? ButtonClose : ButtonContinue;
            actionButton.Visibility = Visibility.Visible;
        }
    }
}