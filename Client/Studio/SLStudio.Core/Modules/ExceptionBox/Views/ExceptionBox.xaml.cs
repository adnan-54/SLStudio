using MahApps.Metro.Controls;
using SLStudio.Logging;
using System;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SLStudio.Core.Modules.ExceptionBox.Views
{
    public partial class ExceptionBox : MetroWindow
    {
        private static readonly ILogger Logger = LogManager.GetLoggerFor<ExceptionBox>();

        [ThreadStatic]
        private static bool showingBox;

        private readonly Exception exception;
        private bool mustTerminate;

        private ExceptionBox(Exception exception, bool mustTerminate)
        {
            InitializeComponent();
            this.mustTerminate = mustTerminate;
            this.exception = exception;
            exceptionDetails.Text = GetClipboardString();

            if (mustTerminate)
            {
                continueButton.Visibility = Visibility.Collapsed;
                continueButton.IsDefault = false;
            }
        }

        public static void Initialize()
        {
#if !DEBUG
            System.Windows.Forms.Application.ThreadException += OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Dispatcher.CurrentDispatcher.UnhandledException += OnDispatcherUnhandledException;
#endif
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Logger.Error(e.Exception);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            Logger.Error(exception);
            if (e.IsTerminating)
                Logger.Fatal("Runtime is terminating because of unhandled exception.");
            ShowErrorBox(exception, e.IsTerminating);
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error(e.Exception);
            ShowErrorBox(e.Exception, false);
            e.Handled = true;
        }

        private static void ShowErrorBox(Exception exception, bool mustTerminate)
        {
            if (showingBox)
                return;
            showingBox = true;

            var exceptionBox = new ExceptionBox(exception, mustTerminate);
            try
            {
                exceptionBox.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Warn(ex);
                MessageBox.Show(exception.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
            finally
            {
                if (exceptionBox.mustTerminate)
                {
                    LogManager.RequestDump();
                    Environment.FailFast("Fatal unhandled exception", exception);
                    Environment.Exit(-1);
                }

                showingBox = false;
            }
        }

        private string GetClipboardString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(exception.Message);
            builder.AppendLine();
            builder.AppendLine(exception.ToString());

            return builder.ToString();
        }

        private void OnContinueClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnExitApplicationClick(object sender, RoutedEventArgs e)
        {
            mustTerminate = true;
            Close();
        }

        private void OnCopyToClipboardClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(GetClipboardString());
            }
            catch (Exception ex)
            {
                Logger.Warn(ex);
            }
        }
    }
}