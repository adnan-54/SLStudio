namespace SLStudio;

public interface IApplicationContext
{
    IEnumerable<ViewRegistration> Views { get; }
}

public interface IConfigurationContext
{
    void AddViewModel();

    void AddView<TView, TViewModel>();

    void AddMenuConfiguration<TMenuConiguration>();

    void ScheduleAction();
}