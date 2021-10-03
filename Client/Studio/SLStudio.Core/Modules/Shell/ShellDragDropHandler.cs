using GongSolutions.Wpf.DragDrop;
using System;
using System.Linq;
using System.Windows;

namespace SLStudio.Core
{
    internal class ShellDragDropHandler : IDropTarget
    {
        private readonly Lazy<IFileService> lazyFileService;

        public ShellDragDropHandler()
        {
            lazyFileService = new Lazy<IFileService>(() => IoC.Get<IFileService>());
        }

        private IFileService FileService => lazyFileService.Value;

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is DataObject data && data.ContainsFileDropList() && data.GetFileDropList().Cast<string>().Any(file => FileService.CanHandle(file)))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
            else
                dropInfo.NotHandled = true;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is DataObject data && data.ContainsFileDropList())
            {
                foreach (var file in data.GetFileDropList().Cast<string>())
                {
                    if (FileService.CanHandle(file))
                        FileService.Open(file).FireAndForget();
                }
            }
            else
                dropInfo.NotHandled = true;
        }
    }
}