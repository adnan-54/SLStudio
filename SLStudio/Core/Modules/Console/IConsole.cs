namespace SLStudio.Core
{
    public interface IConsole
    {
        string GetText();

        void AppendLine(string sender, string message);

        void Clear();

        void ToggleTextWrapping();
    }
}