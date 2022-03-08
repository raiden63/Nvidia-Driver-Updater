namespace NvidiaDriverUpdater.NvidiaClient
{
    public interface INvidiaClient
    {
        Task<NvidiaLookupResponse> Lookup(string typeId, string parentId = null);

        Task<NvidiaRootOptions> GetNvidiaRootOptionsAsync();

        Task<string> DownloadDriverAsync(string productSeriesId, string productId, string osId, string languageId, string downloadTypeId);

        Task<string> DownloadGeforceDriverAsync(string productTypeId, string productSeriesId, string productId, string osId, string languageId, string downloadTypeId);
    }
}