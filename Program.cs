// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using NvidiaDriverUpdater;
using NvidiaDriverUpdater.NvidiaClient.V2;

var config = Startup.BuildConfig();
var services = Startup.BuildServices(config);

var appSettings = services.GetRequiredService<AppSettings>();

// Get current version number on system

var currentVersion = CommandProcess.Execute("nvidia-smi --query-gpu=driver_version --format=csv,noheader");


// TODO: Use Nvidia's other driver download page, that uses actual JSON responses:
// https://www.nvidia.com/en-us/geforce/drivers/


// Get latest version

var client = services.GetRequiredService<INvidiaClient>();

        // OLD WAY
        // var options = await client.GetNvidiaRootOptionsAsync();

        // var productTypeId = options.ProductTypes.Where(t => t.Label.Equals(appSettings.Nvidia.ProductType, StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();
        // var downloadTypeId = options.DownloadTypes.Where(t => t.Label.Equals(appSettings.Nvidia.DownloadType, StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();
        // var languageId = options.Languages.Where(t => t.Label.Equals(appSettings.Nvidia.Language, StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();

        // var productSeriesLookup = await client.Lookup("2", productTypeId);
        // var productSeriesId = productSeriesLookup.LookupValues.Values.Where(t => t.Name.Equals(appSettings.Nvidia.ProductSeries, StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();

        // var productFamilyLookup = await client.Lookup("3", productSeriesId);
        // var productId = productFamilyLookup.LookupValues.Values.Where(t => t.Name.Equals(appSettings.Nvidia.Product, StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();

        // var osLookup = await client.Lookup("4", productSeriesId);
        // var osId = osLookup.LookupValues.Values.Where(t => t.Name.Equals(appSettings.Nvidia.OperatingSystem, StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();

        // var downloadPath = await client.DownloadDriverAsync(productSeriesId, productId, osId, languageId, downloadTypeId);


var downloadUrl = await client.DownloadDriverAsync();







// TODO: Run driver updater