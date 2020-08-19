using GongSolutions.Wpf.DragDrop;
using SLStudio.Core;
using SLStudio.Core.Behaviors;
using SLStudio.RpkEditor.Data;
using SLStudio.RpkEditor.Modules.Editors.ViewModels;
using SLStudio.RpkEditor.Modules.RpkEditor.Resources;
using SLStudio.RpkEditor.Modules.Toolbox.Models;
using SLStudio.RpkEditor.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    internal class RpkDesignerViewModel : RpkEditorBase, IDropTarget
    {
        private readonly RpkMetadata rpk;
        private readonly IWindowManager windowManager;
        private readonly IObjectFactory objectFactory;
        private readonly IRpkManager rpkManager;
        private readonly IUiSynchronization uiSynchronization;

        private IListBoxSelection listBoxSelection;

        public RpkDesignerViewModel(RpkMetadata rpk, IWindowManager windowManager, IObjectFactory objectFactory, IRpkManager rpkManager, IUiSynchronization uiSynchronization)
        {
            this.rpk = rpk;
            this.windowManager = windowManager;
            this.objectFactory = objectFactory;
            this.rpkManager = rpkManager;
            this.uiSynchronization = uiSynchronization;

            DisplayName = RpkEditorResources.Design;
            IconSource = "FrameworkDesignStudio";
        }

        public BindableCollection<ResourceMetadata> Resources => rpk.ResourceMetadatas;

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

        public IListBoxSelection ListBoxSelection => listBoxSelection ??= GetService<IListBoxSelection>();

        public void RequestSearchFocus()
        {
            uiSynchronization.InvokeOnUi(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(20));
                FocusRequested = false;
                FocusRequested = true;
            });
        }

        private void AddResource(Type resourceType, int index = -1)
        {
            var result = rpkManager.AddResource(resourceType, index);
            if (result != null)
            {
                SelectedResource = result;
                ListBoxSelection.Focus();
            }
        }

        //todo: move this to rpkManager
        public void EditResource()
        {
            if (Resources.Where(r => r.IsSelected).Count() != 1)
                return;

            EditResource(Resources.FirstOrDefault(r => r.IsSelected));
        }

        //todo: move this to rpkManager
        public void EditResource(ResourceMetadata metadata)
        {
            if (metadata == null)
                return;

            var editor = new ResourceEditorViewModel(metadata, true);
            windowManager.ShowDialog(editor);
        }

        //todo: move this to rpkManager
        public void RemoveResources()
        {
            if (!Resources.Any(r => r.IsSelected))
                return;

            var focusTarget = GetCommandToFocusBeforeRemoval(Resources.Where(r => r.IsSelected));

            foreach (var metadata in Resources.Where(r => r.IsSelected).ToList())
                Resources.Remove(metadata);

            if (focusTarget == null)
                SelectedResource = Resources.FirstOrDefault();
            else
            if (Resources.Count == 1)
                SelectedResource = Resources.First();
            else
                SelectedResource = focusTarget;

            ListBoxSelection.Focus();
        }

        //todo: move this to rpkManager
        public void MoveResourcesUp()
        {
            var resources = Resources.Where(r => r.IsSelected).ToList();

            if (!resources.Any())
                return;

            var firstResource = resources.First();
            var firstIndex = Resources.IndexOf(firstResource);
            if (firstIndex <= 0)
                return;

            foreach (var metadata in resources)
            {
                var index = Resources.IndexOf(metadata);
                if (index - 1 >= 0)
                    Resources.Move(index, --index);
            }
        }

        //todo: move this to rpkManager
        public void MoveResourcesDown()
        {
            var resources = Resources.Where(r => r.IsSelected).Reverse().ToList();

            if (!resources.Any())
                return;

            var lastResource = resources.First();
            var lastIndex = Resources.IndexOf(lastResource);
            if (lastIndex >= Resources.Count - 1)
                return;

            foreach (var metadata in resources)
            {
                var index = Resources.IndexOf(metadata);
                if (index <= Resources.Count - 1)
                    Resources.Move(index, ++index);
            }
        }

        private ResourceMetadata GetCommandToFocusBeforeRemoval(IEnumerable<ResourceMetadata> metadatas)
        {
            var first = metadatas.First();
            var last = metadatas.Last();
            var firstIndex = Resources.IndexOf(first);
            var lastIndex = Resources.IndexOf(last);

            if (lastIndex + 1 < Resources.Count - 1)
                return Resources[lastIndex + 1];
            else
            if (firstIndex - 1 > 0)
                return Resources[--firstIndex];

            return first;
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