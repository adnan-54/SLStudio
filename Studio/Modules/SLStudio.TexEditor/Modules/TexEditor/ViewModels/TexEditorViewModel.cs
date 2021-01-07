using SLStudio.Core;
using SLStudio.TexEditor.Resources;
using System.IO;
using System.Threading.Tasks;

namespace SLStudio.TexEditor
{
    [FileEditor("editorName", "editorDescription", "editorCategory", typeof(TexEditorResources), "", false, ".tex")]
    internal class TexEditorViewModel : FileDocumentBase, ITexEditor
    {
        public TexEditorViewModel()
        {
        }

        public string Content
        {
            get => GetProperty(() => Content);
            set => SetProperty(() => Content, value);
        }

        protected override async Task DoLoadFrom(Stream stream)
        {
            using var fileStream = new StreamReader(stream, leaveOpen: true);
            Content = await fileStream.ReadToEndAsync();
        }

        protected override Task DoNew(string content)
        {
            Content = content;

            return Task.CompletedTask;
        }

        protected override async Task DoSaveTo(Stream stream)
        {
            using var streamWrite = new StreamWriter(stream, leaveOpen: true);
            await streamWrite.WriteAsync(Content);
            await streamWrite.FlushAsync();
        }
    }

    public interface ITexEditor : IFileDocumentItem
    {
    }
}