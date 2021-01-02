using System;

namespace SLStudio.Core
{
    public interface IFileDescription
    {
        Type EditorType { get; }

        string Name { get; }

        string Description { get; }

        string Category { get; }

        string IconSource { get; }

        string Extension { get; }

        string Filter { get; }

        bool ReadOnly { get; }
    }
}