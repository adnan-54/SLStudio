using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using SLStudio.Studio.Core.Framework;
using SLStudio.Studio.Core.Framework.Services;
using SLStudio.Studio.Core.Modules.Output.Views;
using SLStudio.Studio.Core.Properties;

namespace SLStudio.Studio.Core.Modules.Output.ViewModels
{
    [Export(typeof(IOutput))]
    public class OutputViewModel : Tool, IOutput
    {
        private readonly StringBuilder stringBuilder;
        private readonly OutputWriter outputWriter;
        private IOutputView view;

        public OutputViewModel()
        {
            DisplayName = Resources.OutputDisplayName;
            stringBuilder = new StringBuilder();
            outputWriter = new OutputWriter(this);
        }

        public override PaneLocation PreferredLocation => PaneLocation.Bottom;

        public TextWriter Writer => outputWriter;

        private TextWrapping textWrap = TextWrapping.NoWrap;
        public TextWrapping TextWrap
        {
            get { return textWrap; }
            set
            {
                textWrap = value;

                NotifyOfPropertyChange(() => TextWrap);
            }
        }

        private bool isTextWrapping = false;
        public bool IsTextWrapping
        {
            get { return isTextWrapping; }
            set
            {
                isTextWrapping = value;

                ToggleTextWrap();

                NotifyOfPropertyChange(() => IsTextWrapping);
            }
        }

        private void ToggleTextWrap()
        {
            if (IsTextWrapping)
                TextWrap = TextWrapping.Wrap;
            else
                TextWrap = TextWrapping.NoWrap;
        }

        public void ClearAll()
        {
            Clear();
        }

        public void Append(string text)
        {
            stringBuilder.Append(text);
            OnTextChanged();
        }
        
        private void OnTextChanged()
        {
            if (view != null)
                Execute.OnUIThread(() => view.SetText(stringBuilder.ToString()));
        }

        public void AppendLine(string text)
        {
            Append(text + Environment.NewLine);
        }

        public void Clear()
        {
            if (view != null)
                Execute.OnUIThread(() => view.Clear());
            stringBuilder.Clear();
        }

        protected override void OnViewLoaded(object view)
        {
            this.view = (IOutputView)view;
            this.view.SetText(stringBuilder.ToString());
            this.view.ScrollToEnd();
        }
    }
}
