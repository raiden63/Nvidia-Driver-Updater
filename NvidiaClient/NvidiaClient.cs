using HtmlAgilityPack;

namespace NvidiaDriverUpdater.NvidiaClient
{
    public class NvidiaClient : INvidiaClient
    {
        private const string _selectionUrl = "https://www.nvidia.com/download/index.aspx";
        private readonly HttpClient _httpClient;

        public NvidiaClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_selectionUrl);
        }

        public async Task<NvidiaOptions> GetNvidiaOptionsAsync()
        {
            var response = await _httpClient.GetAsync("");

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


            // // Product Series

            // var productSeriesNode = htmlDoc.DocumentNode.SelectSingleNode("//select[@id='selProductSeries']");
            // var productSeriesOptions = productSeriesNode.SelectNodes("./option")
            //     .Select(t => new ProductSeries { 
            //         Value = t.GetAttributeValue("value", null), 
            //         Label = t.GetDirectInnerText() 
            //     });


            // // Product Family

            // var productFamilyNode = htmlDoc.DocumentNode.SelectSingleNode("//select[@id='selProductFamily']");
            // var productFamilyOptions = productFamilyNode.SelectNodes("./option")
            //     .Select(t => new ProductFamily { 
            //         Value = t.GetAttributeValue("value", null), 
            //         Label = t.GetDirectInnerText() 
            //     });


            // // Operating System
            
            // var operatingSystemNode = htmlDoc.DocumentNode.SelectSingleNode("//select[@id='selOperatingSystem']");
            // var operatingSystemOptions = operatingSystemNode.SelectNodes("./option")
            //     .Select(t => new OperatingSystem { 
            //         Value = t.GetAttributeValue("value", null), 
            //         Label = t.GetDirectInnerText() 
            //     });


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

            return new NvidiaOptions
            {
                ProductTypes = productTypeOptions.ToArray(),
                // ProductSeries = productSeriesOptions.ToArray(),
                // ProductFamilies = productFamilyOptions.ToArray(),
                // OperatingSystems = operatingSystemOptions.ToArray(),
                DownloadTypes = downloadTypeOptions.ToArray(),
                Languages = languageOptions.ToArray()
            };
        }
    }
}