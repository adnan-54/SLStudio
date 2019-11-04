using Caliburn.Micro;
using SLStudio.Studio.Core.Framework.Services;
using System;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Framework.Results
{
    public class ShowToolResult<TTool> : OpenResultBase<TTool>
		where TTool : ITool
	{
		private readonly Func<TTool> _toolLocator = () => IoC.Get<TTool>();

        [Import]
		private IShell _shell;

        public ShowToolResult()
		{
			
		}

		public ShowToolResult(TTool tool)
		{
			_toolLocator = () => tool;
		}

		public override void Execute(CoroutineExecutionContext context)
		{
			var tool = _toolLocator();

            _setData?.Invoke(tool);

            _onConfigure?.Invoke(tool);

            tool.Deactivated += (s, e) =>
			{
				if (!e.WasClosed)
					return;

                _onShutDown?.Invoke(tool);

                OnCompleted(null, false);
			};

			_shell.ShowTool(tool);
		}
	}
}