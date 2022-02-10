namespace NvidiaDriverUpdater.NvidiaClient
{
    public class NvidiaRootOptions
    {
        public ProductType[]? ProductTypes { get; set; }

        public DownloadType[]? DownloadTypes { get; set; }

        public Language[]? Languages { get; set; }
    }

    public class ProductType : NvidiaOption
    { }

    public class DownloadType : NvidiaOption
    { }

    public class Language : NvidiaOption
    { }
}