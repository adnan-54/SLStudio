using Caliburn.Micro;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace SLStudio.Studio.Core.Framework
{
    public abstract class LayoutItemBase : Screen, ILayoutItem
	{
        public abstract ICommand CloseCommand { get; }

        [Browsable(false)]
        public Guid Id { get; } = Guid.NewGuid();

        [Browsable(false)]
		public string ContentId
		{
			get { return Id.ToString(); }
		}

        [Browsable(false)]
		public virtual Uri IconSource
		{
			get { return null; }
		}

		private bool _isSelected;

        [Browsable(false)]
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				_isSelected = value;
				NotifyOfPropertyChange(() => IsSelected);
			}
		}

        [Browsable(false)]
        public virtual bool ShouldReopenOnStart
        {
            get { return false; }
        }

		public virtual void LoadState(BinaryReader reader)
		{
		}

		public virtual void SaveState(BinaryWriter writer)
		{
		}
	}
}