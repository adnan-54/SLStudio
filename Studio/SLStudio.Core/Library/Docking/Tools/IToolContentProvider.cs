using System;

namespace SLStudio.Core
{
    public interface IToolContentProvider
    {
        IToolContent GetContent(Type tool);
    }
}
