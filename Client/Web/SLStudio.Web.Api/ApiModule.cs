namespace SLStudio.Web.Api
{
    public class ApiModule : Module
    {
        public static readonly IServiceContainer ServiceContainer = new ApiServices();

        protected override void Register(IModuleRegister register)
        {
            register.ServiceContainer(ServiceContainer);
        }
    }
}