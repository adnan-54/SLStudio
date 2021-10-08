using System;

namespace SLStudio
{
    public interface IViewModelLocator
    {
        Type LocateFromView(Type viewType);

        Type LocateFromViewModel(Type viewModelType);
    }
}