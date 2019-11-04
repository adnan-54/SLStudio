using SLStudio.Studio.Core.Framework.Services;
using SLStudio.Studio.Core.Modules.MainMenu;
using SLStudio.Studio.Core.Modules.ToolBars;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;

namespace SLStudio.Studio.Core.Framework
{
    public abstract class ModuleBase : IModule
	{
        [Import]
        private IMainWindow _mainWindow;

        [Import]
        private IShell _shell;

        protected IMainWindow MainWindow
	    {
	        get { return _mainWindow; }
	    }

		protected IShell Shell
		{
			get { return _shell; }
		}

		protected IMenu MainMenu
		{
			get { return _shell.MainMenu; }
		}

        protected IToolBars ToolBars
        {
            get { return _shell.ToolBars; }
        }

        public virtual IEnumerable<ResourceDictionary> GlobalResourceDictionaries
        {
            get { yield break; }
        }

        public virtual IEnumerable<IDocument> DefaultDocuments
        {
            get { yield break; }
        }

	    public virtual IEnumerable<Type> DefaultTools
	    {
            get { yield break; }
	    }

        public virtual void PreInitialize()
        {
            
        }

		public virtual void Initialize()
		{
		    
		}

        public virtual void PostInitialize()
        {

        }
	}
}