using GongSolutions.Wpf.DragDrop;
using SLStudio.Core;
using SLStudio.RpkEditor.Modules.Editors.ViewModels;
using SLStudio.RpkEditor.Modules.RpkEditor.Resources;
using SLStudio.RpkEditor.Modules.Toolbox.Models;
using SLStudio.RpkEditor.Rpk;
using System;
using System.Windows;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    internal class RpkDesignerViewModel : RpkEditorBase, IDropTarget
    {
        private readonly RpkMetadata rpk;
        private readonly IWindowManager windowManager;
        private readonly IObjectFactory objectFactory;

        public RpkDesignerViewModel(RpkMetadata rpk, IWindowManager windowManager, IObjectFactory objectFactory)
        {
            this.rpk = rpk;
            this.windowManager = windowManager;
            this.objectFactory = objectFactory;

            DisplayName = RpkEditorResources.Design;
            IconSource = "FrameworkDesignStudio";
        }

        public BindableCollection<ResourceMetadata> Resources => rpk.Resources;

        public ResourceMetadata SelectedResource
        {
            get => GetProperty(() => SelectedResource);
            set => SetProperty(() => SelectedResource, value);
        }

        public bool FocusRequested
        {
            get => GetProperty(() => FocusRequested);
            set => SetProperty(() => FocusRequested, value);
        }

        private void AddResource(Type resourceType, int index = -1)
        {
            var metadata = objectFactory.Create(resourceType) as ResourceMetadata;
            var editor = new ResourceEditorViewModel(metadata);
            var result = windowManager.ShowDialog(editor);
            if (result.GetValueOrDefault())
            {
                if (index == -1)
                    Resources.Add(metadata);
                else
                    Resources.Insert(index, metadata);

                SelectedResource = metadata;
                RequestFocus();
            }
        }

        public void EditResource(ResourceMetadata metadata)
        {
            var editor = new ResourceEditorViewModel(metadata, true);
            windowManager.ShowDialog(editor);
        }

        public void RequestFocus()
        {
            if (!FocusRequested)
                FocusRequested = true;
            else
                RaisePropertyChanged(() => FocusRequested);
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is ToolboxItemModel)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is ToolboxItemModel toolboxItem)
            {
                AddResource(toolboxItem.Metadata.GetType());
                return;
            }
            return;
        }
    }
}