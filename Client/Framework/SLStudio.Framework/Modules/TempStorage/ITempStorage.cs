namespace SLStudio
{
    public interface ITempStorage : IStudioService
    {
        ITempDirectory NewDirectory(string directory = null);

        ITempFile NewFile(string extension = null, string directory = null);
    }
}