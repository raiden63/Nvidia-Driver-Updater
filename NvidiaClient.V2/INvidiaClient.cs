namespace NvidiaDriverUpdater.NvidiaClient.V2;

public interface INvidiaClient
{
    Task<string?> DownloadDriverAsync();
}