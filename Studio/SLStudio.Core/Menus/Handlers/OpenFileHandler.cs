using Microsoft.Win32;
using SLStudio.Core.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class OpenFileHandler : MenuCommandHandler
    {
        private readonly IFileService fileService;
        private string filter;

        public OpenFileHandler(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            filter = BuildFilter();
            var openFileDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = filter ??= BuildFilter(),
                Multiselect = true,
                Title = SLStudioConstants.ProductName,
                ValidateNames = true
            };

            var result = openFileDialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    if (fileService.CanHandle(file))
                        await fileService.Open(file);
                }
            }
        }

        private string BuildFilter()
        {
            var descriptions = fileService.GetDescriptions();
            return $"{GetAllSupportedFiles()}{GetSingleFiles()}{StudioResources.filter_allFiles}";

            string GetSingleFiles()
            {
                var builder = new StringBuilder();
                foreach (var desc in descriptions)
                {
                    builder.Append(desc.Name);
                    builder.Append(" (*");
                    builder.Append(string.Join(", *", desc.Extensions));
                    builder.Append(")|*");
                    builder.Append(string.Join(";*", desc.Extensions));
                    builder.Append('|');
                }

                return builder.ToString();
            }

            string GetAllSupportedFiles()
            {
                var builder = new StringBuilder();
                builder.Append($"{StudioResources.filter_allSupportedFiles} (*");

                foreach (var desc in descriptions)
                    builder.Append(string.Join(", *", desc.Extensions));

                builder.Append(")|*");

                foreach (var desc in descriptions)
                    builder.Append(string.Join(";*", desc.Extensions));

                builder.Append('|');

                return builder.ToString();
            }
        }
    }
}