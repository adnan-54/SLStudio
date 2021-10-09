namespace SLStudio.ErrorReporting
{
    public class ErrorReportingModule : Module
    {
        public static readonly IServiceContainer ServiceContainer = new ErrorReportingServices();

        protected override void Register(IModuleRegister register)
        {
            register.ServiceContainer(ServiceContainer);
        }
    }
}