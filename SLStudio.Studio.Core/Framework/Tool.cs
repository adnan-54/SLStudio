using Caliburn.Micro;
using SLStudio.Studio.Core.Framework.Services;
using SLStudio.Studio.Core.Framework.ToolBars;
using SLStudio.Studio.Core.Modules.ToolBars;
using SLStudio.Studio.Core.Modules.ToolBars.Models;
using System.Windows.Input;

namespace SLStudio.Studio.Core.Framework
{
    public abstract class Tool : LayoutItemBase, ITool
	{
		private ICommand _closeCommand;
		public override ICommand CloseCommand
		{
			get { return _closeCommand ?? (_closeCommand = new RelayCommand(p => IsVisible = false, p => true)); }
		}

	    public abstract PaneLocation PreferredLocation { get; }

	    public virtual double PreferredWidth
	    {
            get { return 200; }
	    }

        public virtual double PreferredHeight
        {
            get { return 200; }
        }

		private bool _isVisible;
		public bool IsVisible
		{
			get { return _isVisible; }
			set
			{
				_isVisible = value;
				NotifyOfPropertyChange(() => IsVisible);
			}
		}

        private ToolBarDefinition _toolBarDefinition;
        public ToolBarDefinition ToolBarDefinition
        {
            get { return _toolBarDefinition; }
            protected set
            {
                _toolBarDefinition = value;
                NotifyOfPropertyChange(() => ToolBar);
                NotifyOfPropertyChange();
            }
        }

	    private IToolBar _toolBar;
        public IToolBar ToolBar
        {
            get
            {
                if (_toolBar != null)
                    return _toolBar;

                if (ToolBarDefinition == null)
                    return null;

                var toolBarBuilder = IoC.Get<IToolBarBuilder>();
                _toolBar = new ToolBarModel();
                toolBarBuilder.BuildToolBar(ToolBarDefinition, _toolBar);
                return _toolBar;
            }
        }

        public override bool ShouldReopenOnStart
        {
            get { return true; }
        }

		protected Tool()
		{
			IsVisible = true;
		}
	}
}