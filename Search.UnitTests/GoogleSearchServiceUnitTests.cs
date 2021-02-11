using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Search.Services;

namespace Search.UnitTests
{
    [TestClass]
    public class GoogleSearchServiceUnitTests
    {
        private readonly Mock<HttpClient> httpClient;

        public GoogleSearchServiceUnitTests()
        {
            httpClient = new Mock<HttpClient>();
        }
        [TestMethod]
        public void PassEmptyParameters()
        {
            //googleSearchService.
        }
    }
}
