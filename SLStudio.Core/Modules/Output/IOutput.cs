namespace SLStudio.Core
{
    public interface IOutput : IToolPanel
    {
        void AppendLine(string text);

        void Clear();

        void ToggleWordWrap();
    }
}