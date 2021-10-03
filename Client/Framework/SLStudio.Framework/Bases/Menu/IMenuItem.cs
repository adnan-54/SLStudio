namespace SLStudio
{
    public interface IMenuItem
    {
        int Index { get; }

        string Path { get; }

        bool IsVisible { get; }
    }
}