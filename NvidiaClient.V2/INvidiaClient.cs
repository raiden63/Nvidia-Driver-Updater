namespace NvidiaDriverUpdater.NvidiaClient.V2;

public interface INvidiaClient
{
    Task<(Version Version, string DownloadUrl)> GetLatestDriverAsync();

    Task<string> DownloadDriverAsync(string url, string fileName = "");
}