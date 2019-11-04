using Caliburn.Micro;
using SLStudio.Studio.Core.Modules.MainMenu;
using SLStudio.Studio.Core.Modules.StatusBar;
using SLStudio.Studio.Core.Modules.ToolBars;
using System;

namespace SLStudio.Studio.Core.Framework.Services
{
    public interface IShell : IGuardClose, IDeactivate
	{
        event EventHandler ActiveDocumentChanging;
        event EventHandler ActiveDocumentChanged;

        bool ShowFloatingWindowsInTaskbar { get; set; }
        
		IMenu MainMenu { get; }
        IToolBars ToolBars { get; }
		IStatusBar StatusBar { get; }

        ILayoutItem ActiveLayoutItem { get; set; }

		IDocument ActiveItem { get; }

		IObservableCollection<IDocument> Documents { get; }
		IObservableCollection<ITool> Tools { get; }

        void ShowTool<TTool>() where TTool : ITool;
		void ShowTool(ITool model);

		void OpenDocument(IDocument model);
		void CloseDocument(IDocument document);

		void Close();
	}
}