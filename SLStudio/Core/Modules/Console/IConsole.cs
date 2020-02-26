namespace SLStudio.Core
{
    public interface IConsole
    {
        string Text { get; }
        bool WordWrap { get; set; }
        void ClearText();
        void AppendLine(string sender, string message);
    }
}
