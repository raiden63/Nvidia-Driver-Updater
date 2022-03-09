using System.Text.Json.Serialization;

namespace NvidiaDriverUpdater.NvidiaClient.V2;


public class NvidiaOption
{
    [JsonPropertyName("id")]
    public object Id { get; set; }

    [JsonPropertyName("menutext")]
    public string MenuText { get; set; }
}