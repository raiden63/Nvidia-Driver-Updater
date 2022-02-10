namespace NvidiaDriverUpdater.NvidiaClient
{
    public class NvidiaOptions
    {
        public ProductType[]? ProductTypes { get; set; }

        public ProductSeries[]? ProductSeries { get; set; }

        public ProductFamily[]? ProductFamilies { get; set; }

        public OperatingSystem[]? OperatingSystems { get; set; }

        public DownloadType[]? DownloadTypes { get; set; }

        public Language[]? Languages { get; set; }
    }

    public class Language : NvidiaOption
    {
    }

    public class OperatingSystem : NvidiaOption
    {
    }

    public class DownloadType : NvidiaOption
    {
    }

    public class ProductFamily : NvidiaOption
    {
    }

    public class ProductSeries : NvidiaOption
    {
    }

    public class ProductType : NvidiaOption
    {
    }
}