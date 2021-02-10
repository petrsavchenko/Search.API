using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Search.Services
{
    public interface ISearchService
    {

        /// <summary>
        /// Performs search of keywords in search engine followed by 
        /// analysis of page to find out where the target url is located
        /// </summary>
        /// <param name="keywords">keywords to search for</param>
        /// <param name="url">target url to find on the page</param>
        /// <returns>comma separated positions</returns>
        Task<string> GetSearchPositions(string keywords, string url);
    }
}
