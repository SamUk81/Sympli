using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Moq.Protected;
using System.Threading;
using Sympli.Services;
using Sympli.Models;

namespace Sympli.Tests
{
    [TestFixture]
    public class GoogleServiceTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private GoogleService _googleService;

        [OneTimeSetUp]
        public void Initialize()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("<div class=\"egMi0 kCrYT\"><a href=\"/url?q=https://www.sympli.com.au/&amp;sa=U&amp;ved=2ahUKEwiH9NuF-aTuAhUIM6wKHbL2DEwQFjAKegQIbBAB&amp;usg=AOvVaw1eJt4Lgr4C72uhXe-pkN1C\">"),
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _googleService = new GoogleService(_httpClientFactoryMock.Object);
        }

        [Test]
        public async Task GoogleSearchService_Get_SearchResultUrlPosition()
        {
            // Arrange
            SearchRequest request = new SearchRequest();
            request.SearchUrl = "Sympli.com.au";
            request.Search = "e-settlement";

            // Act
            var response = await _googleService.Search(request);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.AreEqual("1", response);
        }
    }
}