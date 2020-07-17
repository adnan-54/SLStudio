using SLStudio.Core;

namespace SLStudio.Tests.Modules.Tests.ViewModels
{
    internal class TestViewModel : DocumentPanelBase, ITest
    {
        public TestViewModel()
        {
            DisplayName = "Test";
        }
    }

    internal interface ITest : IDocumentPanel
    {
    }
}