// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using NvidiaDriverUpdater;
using NvidiaDriverUpdater.NvidiaClient;

var config = Startup.BuildConfig();
var services = Startup.BuildServices(config);


// Get current version number on system

var currentVersion = CommandProcess.Execute("nvidia-smi --query-gpu=driver_version --format=csv,noheader");


// Get latest version

var client = services.GetRequiredService<INvidiaClient>();

var options = await client.GetNvidiaRootOptionsAsync();

var productTypeId = options.ProductTypes.Where(t => t.Label.Equals(config["Nvidia:ProductType"], StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();
var downloadTypeId = options.DownloadTypes.Where(t => t.Label.Equals(config["Nvidia:DownloadType"], StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();
var languageId = options.Languages.Where(t => t.Label.Equals(config["Nvidia:Language"], StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();

var productSeriesLookup = await client.Lookup("2", productTypeId);
var productSeriesId = productSeriesLookup.LookupValues.Values.Where(t => t.Name.Equals(config["Nvidia:ProductSeries"], StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();

var productFamilyLookup = await client.Lookup("3", productSeriesId);
var productFamilyId = productFamilyLookup.LookupValues.Values.Where(t => t.Name.Equals(config["Nvidia:ProductFamily"], StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();

var osLookup = await client.Lookup("4", productSeriesId);
var osId = osLookup.LookupValues.Values.Where(t => t.Name.Equals(config["Nvidia:OperatingSystem"], StringComparison.CurrentCultureIgnoreCase)).Select(t => t.Value).FirstOrDefault();

// TODO: Provide text-based selection instead of relying on appsetting values


// TODO: Extract latest version number, and compare. Only download if higher version.


var downloadPath = await client.DownloadDriverAsync(productSeriesId, productFamilyId, osId, languageId, downloadTypeId);




// TODO: Include text-based progress bar



// TODO: Run driver updater