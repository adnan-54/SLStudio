using Microsoft.Win32;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class OpenFileHandler : MenuCommandHandler
    {
        private readonly IFileService fileService;

        public OpenFileHandler(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            var openFileDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = BuildFilter(),
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

        //Todo: move this to file service: string BuildFilter(IEnumerable<string> extensions, FilterOptions.IncludeAllFiles | FilterOptions.IncludeSupportedFiles)
        private static string BuildFilter()
        {
            var allFilesFilter = $"All Files|*.*";
            return string.Join('|', allFilesFilter);
        }
    }
}