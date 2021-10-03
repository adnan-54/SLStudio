using System;

namespace SLStudio
{
    public interface IViewModelLocator : IService
    {
        Type LocateFromView(Type viewType);

        Type LocateFromViewModel(Type viewModelType);
    }
}