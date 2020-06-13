namespace SLStudio.Core
{
    public interface IOutput
    {
        void AppendLine(string content);

        void Clear();
    }
}