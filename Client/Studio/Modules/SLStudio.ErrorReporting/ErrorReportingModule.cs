using System.ComponentModel;

namespace SLStudio.ErrorReporting
{
    public class ErrorReportingModule : Module
    {
        private static readonly IServiceCollection serviceCollection = new ErrorReportingServices();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IErrorReportingService ErrorReportingService => serviceCollection.Get<IErrorReportingService>();

        protected override void Register(IModuleRegister register)
        {
            register.ServiceCollection(serviceCollection);
        }
    }
}