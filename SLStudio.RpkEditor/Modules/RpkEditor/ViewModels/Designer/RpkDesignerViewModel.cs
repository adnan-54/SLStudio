using GongSolutions.Wpf.DragDrop;
using SLStudio.Core;
using SLStudio.RpkEditor.Editors.MeshEditor.ViewModels;
using SLStudio.RpkEditor.Modules.RpkEditor.Resources;
using SLStudio.RpkEditor.Modules.Toolbox.Models;
using SLStudio.RpkEditor.Rpk;
using SLStudio.RpkEditor.Rpk.Definitions;
using System;
using System.Windows;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    internal class RpkDesignerViewModel : RpkEditorBase, IDropTarget
    {
        private readonly IWindowManager windowManager;

        public RpkDesignerViewModel(IWindowManager windowManager)
        {
            this.windowManager = windowManager;

            Resources = new BindableCollection<ResourceMetadata>();

            DisplayName = RpkEditorResources.Design;
            IconSource = "FrameworkDesignStudio";
        }

        public BindableCollection<ResourceMetadata> Resources { get; }

        private void AddResource(Type resourceType)
        {
            if (resourceType == typeof(MeshDefinition))
            {
                var definition = new MeshDefinition();
                var editor = new MeshEditorViewModel(definition);
                var result = windowManager.ShowDialog(editor);
                if (result == true)
                    Resources.Add(definition);
            }
        }

        public void EditResource(ResourceMetadata metadata)
        {
            if (metadata is MeshDefinition definition)
            {
                var editor = new MeshEditorViewModel(definition);
                windowManager.ShowDialog(editor);
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
    }
}