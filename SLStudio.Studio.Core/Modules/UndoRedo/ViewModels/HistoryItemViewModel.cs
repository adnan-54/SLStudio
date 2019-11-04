using Caliburn.Micro;

namespace SLStudio.Studio.Core.Modules.UndoRedo.ViewModels
{
    public class HistoryItemViewModel : PropertyChangedBase
    {
        public IUndoableAction Action { get; }

        private readonly string _name;
        public string Name
        {
            get { return _name ?? Action.Name; }
        }

        private HistoryItemType _itemType;
        public HistoryItemType ItemType
        {
            get { return _itemType; }
            set
            {
                if (_itemType == value)
                    return;

                _itemType = value;

                NotifyOfPropertyChange(() => ItemType);
            }
        }

        public HistoryItemViewModel(IUndoableAction action)
        {
            Action = action;
        }

        public HistoryItemViewModel(string name)
        {
            _name = name;
        }
    }
}
