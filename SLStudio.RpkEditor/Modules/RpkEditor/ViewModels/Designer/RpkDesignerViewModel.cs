using GongSolutions.Wpf.DragDrop;
using SLStudio.Core;
using SLStudio.Core.Behaviors;
using SLStudio.RpkEditor.Data;
using SLStudio.RpkEditor.Modules.RpkEditor.Resources;
using SLStudio.RpkEditor.Modules.Toolbox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    internal class RpkDesignerViewModel : RpkEditorBase, IDropTarget
    {
        private readonly RpkMetadata rpk;
        private readonly RpkManager rpkManager;

        private IListBoxSelection listBoxSelection;

        public RpkDesignerViewModel(RpkMetadata rpk, RpkManager rpkManager)
        {
            this.rpk = rpk;
            this.rpkManager = rpkManager;

            DisplayName = RpkEditorResources.Design;
            IconSource = "FrameworkDesignStudio";
        }

        public IListBoxSelection ListBoxSelection => listBoxSelection ??= GetService<IListBoxSelection>();

        public BindableCollection<ResourceMetadata> Resources => rpk.Resources;

        public BindableCollection<ExternalReferenceMetadata> ExternalReferences => rpk.ExternalReferences;

        public ResourceMetadata SelectedResource
        {
            get => GetProperty(() => SelectedResource);
            set => SetProperty(() => SelectedResource, value);
        }

        public void AddReference()
        {
        }

        public void AddResource(Type resourceType, int index = -1)
        {
            var metadata = rpkManager.AddResource(resourceType, index);
            if (metadata != null)
            {
                SelectedResource = metadata;
                ListBoxSelection.Focus();
            }
        }

        public void EditResource()
        {
            EditResource(Resources.FirstOrDefault(r => r.IsSelected));
        }

        public void EditResource(ResourceMetadata metadata)
        {
            if (metadata == null)
                return;

            rpkManager.EditResource(metadata);
        }

        public void RemoveResources()
        {
            var targets = Resources.Where(r => r.IsSelected).ToList();
            if (!targets.Any())
                return;

            var focusTarget = GetCommandToFocusBeforeRemoval(targets);

            rpkManager.RemoveResources(targets);

            if (focusTarget == null)
                SelectedResource = Resources.FirstOrDefault();
            else
            if (Resources.Count == 1)
                SelectedResource = Resources.First();
            else
                SelectedResource = focusTarget;

            ListBoxSelection.Focus();
        }

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
    }
}