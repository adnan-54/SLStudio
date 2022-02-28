namespace SLStudio;

public static class TaskExtensions
{
    private static readonly ILogger logger = LogManager.GetLogger();

    public static async void FireAndForget(this Task task, Action<Task>? resultCallback = default)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.Exception(ex);
        }

        resultCallback?.Invoke(task);
    }
}