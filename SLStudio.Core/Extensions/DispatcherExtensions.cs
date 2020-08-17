using System;
using System.Windows.Threading;

namespace SLStudio.Core
{
    public static class DispatcherExtensions
    {
        public static IDisposable InvokeDisposableAsync(this Dispatcher dispatcher, Action action, DispatcherPriority priority = DispatcherPriority.Normal)
            => new DisposableDispatcherOperation(dispatcher.InvokeAsync(action, priority));
    }

    internal class DisposableDispatcherOperation : IDisposable
    {
        private DispatcherOperation operation;

        public DisposableDispatcherOperation(DispatcherOperation operation)
        {
            this.operation = operation;
            operation.Completed += OnDispatcherOperationCompleted;
        }

        private void OnDispatcherOperationCompleted(object sender, EventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            if (operation == null)
                return;

            if (operation.Status == DispatcherOperationStatus.Executing || operation.Status == DispatcherOperationStatus.Pending)
                operation.Abort();

            operation.Completed -= OnDispatcherOperationCompleted;
            operation = null;
        }
    }
}