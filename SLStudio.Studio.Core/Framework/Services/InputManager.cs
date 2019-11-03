using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SLStudio.Studio.Core.Framework.Services
{
    [Export(typeof(IInputManager))]
	public class InputManager : IInputManager
	{
		public void SetShortcut(DependencyObject view, InputGesture gesture, object handler)
		{
            InputBindingTrigger inputBindingTrigger = new InputBindingTrigger
            {
                InputBinding = new InputBinding(new RoutedCommand(), gesture)
            };

            Interaction.GetTriggers(view).Add(inputBindingTrigger);

			inputBindingTrigger.Actions.Add(new TestTriggerAction(handler));
		}

		public void SetShortcut(InputGesture gesture, object handler)
		{
			SetShortcut(Application.Current.MainWindow, gesture, handler);
		}

		private class TestTriggerAction : TriggerAction<FrameworkElement>
		{
			private readonly object _handler;

			public TestTriggerAction(object handler)
			{
				_handler = handler;
			}

			protected override void Invoke(object parameter)
			{
                var context = new ActionExecutionContext
                {
                    Target = _handler,
                    Message = new ActionMessage { MethodName = "Execute" },
                    Method = _handler.GetType().GetMethod("Execute")
                };
			    ActionMessage.PrepareContext(context);

                if (context.CanExecute())
				    ActionMessage.InvokeAction(context);
			}
		}
	}
}