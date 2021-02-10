using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Search.Services
{
    public class GoogleSearchService : IGoogleSearchService
    {
        /// <summary>
        /// User Agent to emulate a browser
        /// </summary>
        /// <remarks>
        /// Require to have unuglified classes in result html
        /// </remarks>
        private const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36";

        private readonly HttpClient httpClient;

        private readonly GoogleSearchServiceSettings settings;


        public GoogleSearchService(HttpClient httpClient, IOptions<GoogleSearchServiceSettings> options)
        {
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            this.settings = options.Value;
        }

        public async Task<string> GetSearchPositions(string keywords, string url)
        {
            var request = $"{settings.Host}/search?q={string.Join('+', keywords.Split(" "))}&num=50&hl=en";
            if (!url.EndsWith("/"))
            {
                url += "/";
            }
            using var response = await httpClient.GetAsync(request);
            using HttpContent content = response.Content;
            var result = await content.ReadAsStringAsync();
            var postitions = new List<int>();

            // obviously search would be more efficient and extensible with html-agility-pack
            var resultDivs = result.Split("class=\"g\"");
            for (int i = 0; i < resultDivs.Length; i++)
            {
                //exact href added to exclude partial matches
                if (resultDivs[i].Contains($"href=\"{url}\""))
                {
                    postitions.Add(i);
                }
            }

            return string.Join(", ", postitions);
        }
    }
}
