using System;
using System.Threading;
using System.Windows;

namespace SLStudio.ErrorReporting
{
    public partial class ReportExceptionDialog : Window
    {
        private readonly IErrorReportingService errorReportingService;
        private readonly Exception exception;

        private bool isReporting;
        private CancellationTokenSource cancellationTokenSource;

        public ReportExceptionDialog(IErrorReportingService errorReportingService, Exception exception)
        {
            this.errorReportingService = errorReportingService;
            this.exception = exception;

            InitializeComponent();
            SetStateInitial();
        }

        private void OnNoClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void OnYesClick(object sender, RoutedEventArgs e)
        {
            if (isReporting)
                return;
            isReporting = true;

            ButtonYes.IsEnabled = false;

            SetStateReporting();

            try
            {
                cancellationTokenSource = new CancellationTokenSource();
                await errorReportingService.ReportException(exception, cancellationTokenSource.Token);
            }
            catch { }

            SetStateResult();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            ButtonCancel.IsEnabled = false;

            if (cancellationTokenSource == null)
                return;

            cancellationTokenSource.Cancel();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetStateInitial()
        {
            InitialState.Visibility = Visibility.Visible;
            ReportingState.Visibility = Visibility.Collapsed;
            ResultState.Visibility = Visibility.Collapsed;

            ButtonNo.Visibility = Visibility.Visible;
            ButtonYes.Visibility = Visibility.Visible;
            ButtonCancel.Visibility = Visibility.Collapsed;
            ButtonOk.Visibility = Visibility.Collapsed;
        }

        private void SetStateReporting()
        {
            InitialState.Visibility = Visibility.Collapsed;
            ReportingState.Visibility = Visibility.Visible;
            ResultState.Visibility = Visibility.Collapsed;

            ButtonNo.Visibility = Visibility.Collapsed;
            ButtonYes.Visibility = Visibility.Collapsed;
            ButtonCancel.Visibility = Visibility.Visible;
            ButtonOk.Visibility = Visibility.Collapsed;
        }

        private void SetStateResult()
        {
            InitialState.Visibility = Visibility.Collapsed;
            ReportingState.Visibility = Visibility.Collapsed;
            ResultState.Visibility = Visibility.Visible;

            ButtonNo.Visibility = Visibility.Collapsed;
            ButtonYes.Visibility = Visibility.Collapsed;
            ButtonCancel.Visibility = Visibility.Collapsed;
            ButtonOk.Visibility = Visibility.Visible;

            Close();
        }
    }
}