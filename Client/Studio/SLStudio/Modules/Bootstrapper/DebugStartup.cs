namespace SLStudio
{
    internal class DebugStartup : Startup
    {
        public DebugStartup()
        {
            ErrorHandler = new DebugErrorHandler();
        }

        public override ErrorHandler ErrorHandler { get; }

        protected override int Run()
        {
            return RunApplication();
        }
    }
}