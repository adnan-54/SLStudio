using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Framework.Results
{
    public class OpenDocumentResult : OpenResultBase<IDocument>
	{
		private readonly IDocument _editor;
		private readonly Type _editorType;
		private readonly string _path;

        [Import]
		private IShell _shell;

        public OpenDocumentResult(IDocument editor)
		{
			_editor = editor;
		}

		public OpenDocumentResult(string path)
		{
			_path = path;
		}

		public OpenDocumentResult(Type editorType)
		{
			_editorType = editorType;
		}

		public override void Execute(CoroutineExecutionContext context)
		{
			var editor = _editor ??
				(string.IsNullOrEmpty(_path)
					? (IDocument)IoC.GetInstance(_editorType, null)
					:  GetEditor(_path));

			if (editor == null)
			{
				OnCompleted(null, true);
				return;
			}

            _setData?.Invoke(editor);

            _onConfigure?.Invoke(editor);

            editor.Deactivated += (s, e) =>
			{
				if (!e.WasClosed)
					return;

                _onShutDown?.Invoke(editor);
            };

			_shell.OpenDocument(editor);

			OnCompleted(null, false);
		}

		private static IDocument GetEditor(string path)
		{
		    return OpenFileCommandHandler.GetEditor(path).Result;
		}
	}
}