using System.Text.Json;
using System.Xml.Serialization;
using HtmlAgilityPack;

namespace NvidiaDriverUpdater.NvidiaClient
{
    public class NvidiaClient : INvidiaClient
    {
        private readonly HttpClient _httpClient;

        public NvidiaClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetDriverDownloadLinkAsync(string productSeriesId, string productFamilyId, string osId, string languageId, string downloadTypeId)
        {
            var response1 = await _httpClient.GetAsync($"download/processDriver.aspx?psid={productSeriesId}&pfid={productFamilyId}&osId={osId}&lid={languageId}&dtid={downloadTypeId}&lang-en-us&ctk=0&rpf=1&dtcid=1");
            var driverPageLink = await response1.Content.ReadAsStringAsync();

            var response2 = await _httpClient.GetAsync(driverPageLink);
            var confirmationPage = await response2.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(confirmationPage);

            var confirmationLink = htmlDoc.DocumentNode.SelectSingleNode("//a[@id='lnkDwnldBtn']").GetAttributeValue("href", null);
            var response3 = await _httpClient.GetAsync(confirmationLink);
            var downloadPage = await response3.Content.ReadAsStringAsync();

            var htmlDoc2 = new HtmlDocument();
            htmlDoc2.LoadHtml(downloadPage);
            var downloadLink = htmlDoc2.DocumentNode.SelectSingleNode("//div[@id='mainContent']/table//a").GetAttributeValue("href", null);

            return downloadLink;
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