namespace NvidiaDriverUpdater.NvidiaClient.V2;

public interface INvidiaClient
{
    Task<(Version Version, string DownloadUrl)> GetLatestDriverAsync();

    Task<string> DownloadDriver(string url);
}