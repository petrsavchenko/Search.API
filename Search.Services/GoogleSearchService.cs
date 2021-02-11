using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Search.Services
{
    //TODO: move similarities into base class
    public class GoogleSearchService : IGoogleSearchService
    {
        /// <summary>
        /// User Agent to emulate a browser
        /// </summary>
        /// <remarks>
        /// Require to have unuglified classes in result html
        /// </remarks>
        private const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36";

        private readonly ILogger logger;

        private readonly HttpClient httpClient;

        private readonly GoogleSearchServiceSettings settings;

        public GoogleSearchService(ILogger<GoogleSearchService> logger, 
            HttpClient httpClient, IOptions<GoogleSearchServiceSettings> options)
        {
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            this.settings = options.Value;
            this.logger = logger;
        }

        // Bing and Google Search implementation looks pretty similar so they could be moved into base class
        // however, most likely html page is highly fragile as it might change at any time so I'd leave them separate
        // for greater flexibility
        public async Task<string> GetSearchPositions(string keywords, string url)
        {
            var searchUrl = $"{settings.Host}/search?q={string.Join('+', keywords.Split(" "))}&num=50&hl=en";
            logger.LogInformation(searchUrl);

            using var response = await httpClient.GetAsync(searchUrl);
            using HttpContent content = response.Content;
            var result = await content.ReadAsStringAsync();
            var postitions = new List<int>();
            int adsCount = 0;

            // obviously search would be more efficient and extensible with html-agility-pack
            var resultDivs = result.Split("class=\"g\"");
            for (int i = 0; i < resultDivs.Length; i++)
            {
                if (i == 0)
                {
                    var ads = resultDivs[0].Split("class=\"uEierd\"");
                    // start with content of first ad, so skip first half as it doesnt have search result
                    for (int j = 1; j < ads.Length; j++)
                    {
                        if (Regex.IsMatch(ads[j], $"href=\"{url}.*\""))
                        {
                            postitions.Add(j);
                            logger.LogInformation($"Match found in ads");
                            logger.LogInformation(ads[j]);
                        }
                    }
                    // to exclude first half of content
                    adsCount = ads.Length - 1;
                    continue;
                }
                //any added to include partial matches e.g *.com.in or host\relative paths
                if (Regex.IsMatch(resultDivs[i], $"href=\"{url}.*\""))
                {
                    postitions.Add(adsCount + i);
                    logger.LogInformation($"Match found in main results");
                    logger.LogInformation(resultDivs[i]);
                }
            }

            return string.Join(", ", postitions);
        }
    }
}
