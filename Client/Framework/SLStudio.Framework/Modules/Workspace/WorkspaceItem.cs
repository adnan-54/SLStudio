using System;

namespace SLStudio.Framework.Modules.Workspace
{
    public class WorkspaceItem
    {
    }

    public interface IWorkspaceItem
    {
        Guid Id { get; }

        string Title { get; }

        string ToolTip { get; }

        object Icon { get; }

        WorkspacePlacement Placement { get; }

        bool IsActive { get; }

        bool IsVisible { get; }

        void Activate();

        void Show();

        void Close();

        void Float();
    }

    public enum WorkspacePlacement
    {
        Left,
        Right,
        Top,
        Bottom,
        Document
    }
}
