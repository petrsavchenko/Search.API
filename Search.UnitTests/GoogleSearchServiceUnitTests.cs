using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Search.Services;

namespace Search.UnitTests
{
    [TestClass]
    public class GoogleSearchServiceUnitTests
    {
        private readonly Mock<HttpMessageHandler> httpMessageHandler;

        private readonly Mock<ILogger<GoogleSearchService>> loggerMock;

        private readonly IOptions<GoogleSearchServiceSettings> options;

        public GoogleSearchServiceUnitTests()
        {
            loggerMock = new Mock<ILogger<GoogleSearchService>>();
            options = Options.Create(new GoogleSearchServiceSettings() { Host = "https://mock.com" });
            httpMessageHandler = new Mock<HttpMessageHandler>();
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow(null, "")]
        [DataRow("", null)]
        [DataRow("", "")]
        [DataRow("    ", "    ")]
        // error message can be more specific
        [ExpectedException(typeof(ArgumentNullException), "input parameters must be set")]
        public async Task PassEmptyOrWhitSpaceParametersNegative(string mockedKeywords, string mockedUrls)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(""),
            };

            SetupHttpMessageHandler(response);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var googleSearchService = new GoogleSearchService(loggerMock.Object, httpClient, options);
            
            await googleSearchService.GetSearchPositions(mockedKeywords, mockedUrls);
        }

        [DataTestMethod]
        [DataRow("mockedKeyword", "invalidUrl.com")]
        // error message can be more specific
        [ExpectedException(typeof(ArgumentException), "url must be valid")]
        public async Task PassIncorrectUrlNegative(string mockedKeywords, string mockedUrls)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(""),
            };

            SetupHttpMessageHandler(response);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var googleSearchService = new GoogleSearchService(loggerMock.Object, httpClient, options);

            await googleSearchService.GetSearchPositions(mockedKeywords, mockedUrls);
        }

        [TestMethod]
        public async Task BlaBlaCarMatchFoundInMainResultsInFirstPositionPositive()
        {
            var response = LoadHttpResponseMessageFromFile("blaBlaCarMainResultsOnly.html");

            SetupHttpMessageHandler(response);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var googleSearchService = new GoogleSearchService(loggerMock.Object, httpClient, options);

            var result = await googleSearchService.GetSearchPositions("bla bla car", "https://www.blablacar.com/");
            Assert.AreEqual(result, "1");
        }

        [TestMethod]
        public async Task InfotrackMatchesFoundInMainResultsWithAddsPositive()
        {
            var response = LoadHttpResponseMessageFromFile("infotrackMainResultsAndAds.html");

            SetupHttpMessageHandler(response);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var googleSearchService = new GoogleSearchService(loggerMock.Object, httpClient, options);

            var result = await googleSearchService.GetSearchPositions("online title search", "https://www.infotrack.com.au");
            Assert.AreEqual(result, "5");
        }

        private HttpResponseMessage LoadHttpResponseMessageFromFile(string file)
        {
            string fileContent = System.IO.File.ReadAllText(file);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fileContent),
            };
        }

        private void SetupHttpMessageHandler(HttpResponseMessage response)
        {
            httpMessageHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
        }
    }
}
