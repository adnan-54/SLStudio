using System;

namespace SLStudio.Core
{
    public interface IFileDescription
    {
        Type EditorType { get; }
        string DisplayName { get; }
        string Description { get; }
        string IconSource { get; }
        string Extension { get; }
        string Filter { get; }
    }
}