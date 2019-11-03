using Caliburn.Micro;
using System;

namespace SLStudio.Studio.Core.Framework.Results
{
    public abstract class OpenResultBase<TTarget> : IOpenResult<TTarget>
	{
		protected Action<TTarget> _setData;
		protected Action<TTarget> _onConfigure;
		protected Action<TTarget> _onShutDown;

		Action<TTarget> IOpenResult<TTarget>.OnConfigure
		{
			get { return _onConfigure; }
			set { _onConfigure = value; }
		}

		Action<TTarget> IOpenResult<TTarget>.OnShutDown
		{
			get { return _onShutDown; }
			set { _onShutDown = value; }
		}

		protected virtual void OnCompleted(Exception exception, bool wasCancelled)
		{
            Completed?.Invoke(this, new ResultCompletionEventArgs { Error = exception, WasCancelled = wasCancelled });
        }

		public abstract void Execute(CoroutineExecutionContext context);

		public event EventHandler<ResultCompletionEventArgs> Completed;
	}
}