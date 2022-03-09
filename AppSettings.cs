public class AppSettings
{
    public AppSettings()
    {
        Nvidia = new NvidiaConfig();
    }

    public NvidiaConfig Nvidia { get; set; }

    public string DownloadDir { get; set; }
}

public class NvidiaConfig
{
    public string BaseUri { get; set; } = string.Empty;

    public string ProductType { get; set; } = string.Empty;

    public string ProductSeries { get; set; } = string.Empty;
    
    public string Product { get; set; } = string.Empty;
    
    public string OperatingSystem { get; set; } = string.Empty;
    
    public string DownloadType { get; set; } = string.Empty;
    
    public string Language { get; set; } = string.Empty;

    public bool IsGameReadyDriver { get; set; } = false;
}