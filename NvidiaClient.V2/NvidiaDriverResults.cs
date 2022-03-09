using System.Text.Json.Serialization;

namespace NvidiaDriverUpdater.NvidiaClient.V2;

public class NvidiaDriverResults
{
    [JsonPropertyName("Success")]
    public string Success { get; set; }

    [JsonPropertyName("IDS")]
    public NvidiaDriverId[] DriverIds { get; set; }
}

public class NvidiaDriverId
{
    [JsonPropertyName("downloadInfo")]
    public NvidiaDriverDownloadInfo DownloadInfo { get; set; }
}

public class NvidiaDriverDownloadInfo
{
    [JsonPropertyName("ID")]
    public string Id { get; set; }

    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [JsonPropertyName("DownloadURL")]
    public string DownloadUrl { get; set; }
}