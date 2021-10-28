using System;

namespace SLStudio
{
    public interface IViewModelLocator
    {
        Type Locate(object view);

        Type Locate(Type viewType);

        Type Locate<TView>()
            where TView : class;
    }
}