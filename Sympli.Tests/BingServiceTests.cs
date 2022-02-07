using Moq;
using Moq.Protected;
using NUnit.Framework;
using Sympli.Models;
using Sympli.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sympli.Tests
{
    [TestFixture]
    public class BingServiceTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private BingService _bingService;

        [OneTimeSetUp]
        public void Initialize()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("<li class=\"b_algo\"><h2><a href=\"https://www.sympli.com.au/e-settlement-services/\" h=\"ID=SERP,5129.1\"><strong>e-Settlement Services</strong> " +
                    "- Sympli</a></h2><div class=\"b_caption\"><div class=\"b_attribution\" u=\"2|5071|4758175519213448|0pLOsgLem9FzxO-mtcFyHkkgrPNCJyNy\">" +
                    "<cite>https://www.sympli.com.au/<strong>e-settlement-services</strong></cite><span class=\"c_tlbxTrg\"><span class=\"c_tlbxH\" H=\"BASE:CACHEDPAGEDEFAULT\" " +
                    "K=\"SERP,5130.1\"></span></span></div><p><strong>e-Settlement Services</strong>. Designed by users, for users. You’ve told us what you need to conduct " +
                    "<strong>e</strong>-<strong>Settlements</strong> with confidence and our highly experienced team have taken your feedback to develop a service that’s efficient, easy to use, " +
                    "reliable and secure. Improved efficiency. Workspace and document creation is 25-50% more efficient for all users. Integration. …</p></div></li>"),
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _bingService = new BingService(_httpClientFactoryMock.Object);
        }

        [Test]
        public async Task BingSearchService_Get_SearchResultUrlPosition()
        {
            // Arrange
            SearchRequest request = new SearchRequest();
            request.SearchUrl = "Sympli.com.au";
            request.Search = "e-settlement";

            // Act
            var response = await _bingService.Search(request);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.AreEqual("1", response);
        }
    }
}
