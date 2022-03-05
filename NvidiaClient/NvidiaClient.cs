using System.Xml.Serialization;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace NvidiaDriverUpdater.NvidiaClient
{
    public class NvidiaClient : INvidiaClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly string _downloadDir;

        public NvidiaClient(HttpClient httpClient, IConfiguration config, ILogger logger)
        {
            _httpClient = httpClient;

            _logger = logger;
            _downloadDir = config["DownloadDir"];
        }

        public async Task<string?> DownloadDriverAsync(string productSeriesId, string productFamilyId, string osId, string languageId, string downloadTypeId)
        {
            _logger.Information("Scrubbing through Nvidia site");

            // Part 1: Submit driver search parameters
            // Response body will be a URL

            var response1 = await _httpClient.GetAsync($"download/processDriver.aspx?psid={productSeriesId}&pfid={productFamilyId}&osId={osId}&lid={languageId}&dtid={downloadTypeId}&lang-en-us&ctk=0&rpf=1&dtcid=1");
            var driverPageLink = await response1.Content.ReadAsStringAsync();

            _logger.Information("Redirect Link: {RedirectLink}", driverPageLink);

            // Part 2: Get first download button link

            var response2 = await _httpClient.GetAsync(driverPageLink);
            var confirmationPage = await response2.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(confirmationPage);
            var confirmationLink = htmlDoc.DocumentNode.SelectSingleNode("//a[@id='lnkDwnldBtn']").GetAttributeValue("href", null);

            _logger.Information("Download Button Link 1: {DownloadLink1}", confirmationLink);

            // Part 3: Get second download button link

            var response3 = await _httpClient.GetAsync(confirmationLink);
            var downloadPage = await response3.Content.ReadAsStringAsync();

            var htmlDoc2 = new HtmlDocument();
            htmlDoc2.LoadHtml(downloadPage);
            var downloadLink = htmlDoc2.DocumentNode.SelectSingleNode("//div[@id='mainContent']/table//a").GetAttributeValue("href", null);

            _logger.Information("Download Button Link 2: {DownloadLink2}", downloadLink);

            // Part 4: Download the driver

            // TODO: Use HttpCompletionOption.ResponseHeadersRead to capture download progress
            // https://github.com/dotnet/runtime/issues/16681#issuecomment-195980023
            
            var fileName = Path.GetFileName(downloadLink);

            // TODO: check if file exists, prompt for delete/redownload
            
            var downloadPath = Path.Combine(_downloadDir, fileName);

            if (File.Exists(downloadPath))
            {
                var overwrite = InputHelper.PromptKey($"The file [{downloadPath}] already exists. Do you want to overwrite it? [Y]es, or [N]o to exit: ");

                if (overwrite != ConsoleKey.Y)
                {
                    _logger.Information("File already exists. Exiting...");
                    return null;
                }

                File.Delete(downloadPath);
            }
            
            _logger.Information("Downloading driver");

            var downloadedFile = await _httpClient.DownloadFileWithProgressBarAsync(downloadLink, downloadPath);

            _logger.Information("Download successful: {DriverFilePath}", downloadedFile);

            return downloadedFile;
        }

        public async Task<NvidiaRootOptions> GetNvidiaRootOptionsAsync()
        {
            var response = await _httpClient.GetAsync("download/index.aspx");

            var responseString = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseString);

            // Product Type

            var productTypeNode = htmlDoc.DocumentNode.SelectSingleNode("//select[@id='selProductSeriesType']");
            var productTypeOptions = productTypeNode.SelectNodes("./option")
                .Select(t => new ProductType { 
                    Value = t.GetAttributeValue("value", null), 
                    Label = t.GetDirectInnerText() 
                });


            // Download Type
            
            var downloadTypeNode = htmlDoc.DocumentNode.SelectSingleNode("//select[@id='ddlDownloadTypeCrdGrd']");
            var downloadTypeOptions = downloadTypeNode.SelectNodes("./option")
                .Select(t => new DownloadType { 
                    Value = t.GetAttributeValue("value", null), 
                    Label = t.GetDirectInnerText() 
                });

            
            // Language
            
            var languageNode = htmlDoc.DocumentNode.SelectSingleNode("//select[@id='ddlLanguage']");
            var languageOptions = languageNode.SelectNodes("./option")
                .Select(t => new Language { 
                    Value = t.GetAttributeValue("value", null), 
                    Label = t.GetDirectInnerText() 
                });

            return new NvidiaRootOptions
            {
                ProductTypes = productTypeOptions.ToArray(),
                DownloadTypes = downloadTypeOptions.ToArray(),
                Languages = languageOptions.ToArray()
            };
        }

        public async Task<NvidiaLookupResponse> Lookup(string typeId, string parentId = "")
        {
            var response = await _httpClient.GetAsync($"download/API/lookupValueSearch.aspx?TypeID={typeId}&ParentID={parentId}");

            var serializer = new XmlSerializer(typeof(NvidiaLookupResponse));
            var responseString = await response.Content.ReadAsStringAsync();
            var lookupResponse = serializer.Deserialize(await response.Content.ReadAsStreamAsync()) as NvidiaLookupResponse;
            
            return lookupResponse;
        }
    }
}