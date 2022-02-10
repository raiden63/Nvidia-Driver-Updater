namespace NvidiaDriverUpdater.NvidiaClient
{
    public interface INvidiaClient
    {
        Task<NvidiaLookupResponse> Lookup(string typeId, string parentId = null);

        Task<NvidiaRootOptions> GetNvidiaRootOptionsAsync();

        Task<string> GetDriverDownloadLinkAsync(string productSeriesId, string productFamilyId, string osId, string languageId, string downloadTypeId);
    }
}