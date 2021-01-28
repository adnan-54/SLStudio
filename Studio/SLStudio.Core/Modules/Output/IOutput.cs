namespace SLStudio.Core
{
    public interface IOutput : IToolItem
    {
        void AppendLine(string text);

        void Clear();

        void ToggleWordWrap();
    }
}