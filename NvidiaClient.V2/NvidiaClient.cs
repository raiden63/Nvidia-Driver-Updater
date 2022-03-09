using System.Text.Json;
using Serilog;

namespace NvidiaDriverUpdater.NvidiaClient.V2;

public class NvidiaClient : INvidiaClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly AppSettings _appSettings;

    public NvidiaClient(HttpClient httpClient, AppSettings appSettings, ILogger logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Nvidia.BaseUri);

        _logger = logger;
        _appSettings = appSettings;
    }

    public async Task<string?> DownloadDriverAsync()
    {
        // 1. Get the complete list of dropdown options

        var initialResponse = await _httpClient.GetAsync("nvidia_web_services/controller.php?com.nvidia.services.Drivers.getMenuArrays");

        // TODO: Update deserializer to parse out ID's as int's instead of strings or generic objects
        var nvidiaDropdownOptions = JsonSerializer.Deserialize<List<List<NvidiaOption>?>>(await initialResponse.Content.ReadAsStringAsync());

        var productTypes = nvidiaDropdownOptions[0];
        var productSeries = nvidiaDropdownOptions[1];
        var operatingSystems = nvidiaDropdownOptions[4];
        var languages = nvidiaDropdownOptions[5];

        // 2. Get the matching menu items
        // TODO: Need to make a second call to get the right set of product series

        var productTypeId = Lookup(productTypes, _appSettings.Nvidia.ProductType);
        var productSeriesId = Lookup(productSeries, _appSettings.Nvidia.ProductSeries);
        var osId = Lookup(operatingSystems, _appSettings.Nvidia.OperatingSystem);
        var languageId = Lookup(languages, _appSettings.Nvidia.Language);

        var productResponse = await _httpClient.GetAsync($@"nvidia_web_services/controller.php?com.nvidia.services.Drivers.getMenuArrays/{{""pt"":{productTypeId.Id}, ""pst"": {productSeriesId.Id}}}");
        var productOptions = JsonSerializer.Deserialize<List<List<NvidiaOption>?>>(await productResponse.Content.ReadAsStringAsync());

        var productId = Lookup(productOptions[2], _appSettings.Nvidia.Product);

        // 3. Search Drivers

        var driverLookupUrl = "services_toolkit/services/com/nvidia/services/AjaxDriverService.php?"
                                + "func=DriverManualLookup"
                                + $"&psid={productSeriesId.Id}"
                                + $"&pfid={productId.Id}"
                                + $"&osID={osId.Id}"
                                + $"&languageCode={languageId.Id}"
                                + "&beta=0"
                                + "&isWHQL=0"
                                + "&dltype=-1"
                                + "&dch=1"
                                + $"&upCRD={(_appSettings.Nvidia.IsGameReadyDriver ? "0" : "1")}"
                                + "&qnf=0"
                                + "&sort1=0"
                                + "&numberOfResults=1"
        ;
        var searchDriverResponse = await _httpClient.GetAsync(driverLookupUrl);
        var driverResults = JsonSerializer.Deserialize<NvidiaDriverResults>(await searchDriverResponse.Content.ReadAsStreamAsync());

        return driverResults.DriverIds[0].DownloadInfo.DownloadUrl;
    }

    private NvidiaOption? Lookup(List<NvidiaOption> options, string targetLabel)
    {
        return options
                .Where(t => t.MenuText.Equals(targetLabel, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();
    }
}
