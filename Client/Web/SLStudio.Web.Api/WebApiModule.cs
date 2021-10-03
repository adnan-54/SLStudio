using System.ComponentModel;

namespace SLStudio.Web.Api
{
    public class WebApiModule : Module
    {
        private static readonly IServiceCollection serviceCollection = new ApiServices();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IReportExceptionApi ReportExceptionApi => serviceCollection.Get<IReportExceptionApi>();

        protected override void Register(IModuleRegister register)
        {
            register.ServiceCollection(serviceCollection);
        }
    }
}