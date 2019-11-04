using Caliburn.Micro;
using SLStudio.Studio.Core.Modules.ToolBars.Controls;
using SLStudio.Studio.Core.Modules.ToolBars.Views;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Modules.ToolBars.ViewModels
{
    [Export(typeof(IToolBars))]
    public class ToolBarsViewModel : ViewAware, IToolBars
    {
        private readonly BindableCollection<IToolBar> _items;
        public IObservableCollection<IToolBar> Items
        {
            get { return _items; }
        }

        private readonly IToolBarBuilder _toolBarBuilder;

        private bool _visible;
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                NotifyOfPropertyChange();
            }
        }

        [ImportingConstructor]
        public ToolBarsViewModel(IToolBarBuilder toolBarBuilder)
        {
            _toolBarBuilder = toolBarBuilder;
            _items = new BindableCollection<IToolBar>();
        }

        protected override void OnViewLoaded(object view)
        {
            _toolBarBuilder.BuildToolBars(this);

            foreach (var toolBar in Items)
                ((IToolBarsView)view).ToolBarTray.ToolBars.Add(new MainToolBar
                {
                    ItemsSource = toolBar
                });

            base.OnViewLoaded(view);
        }
    }
}