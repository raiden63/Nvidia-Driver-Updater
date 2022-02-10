namespace NvidiaDriverUpdater.NvidiaClient
{
    public interface INvidiaClient
    {
        Task<NvidiaOptions> GetNvidiaOptionsAsync();
    }
}