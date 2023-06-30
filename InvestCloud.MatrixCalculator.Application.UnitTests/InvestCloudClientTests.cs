using InvestCloud.MatrixCalculator.Application.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace InvestCloud.MatrixCalculator.Application.UnitTests
{
    public class InvestCloudClientTests
    {                                              
        [Fact]
        public async Task GetRowSuccessfulTest()
        {            
            // Arrange
            var result = new ResultOfRowInt32()
            {
                Value = new List<int> { 1, -1 },
                Success = true,
            };

            (var mockHttpClientFactory, var mockConfiguration) = 
                CreateMockHttpClientFactory<ResultOfRowInt32>("Endpoints:Get", "{{dataset}}/{{type}}/{{idx}}", result);

            // Act
            var client = new InvestCloudClient( mockHttpClientFactory.Object, mockConfiguration.Object);
            var response = await client.GetRow("A", 1);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equivalent(result, response);
            
        }

        private (Mock<IHttpClientFactory>, Mock<IConfiguration>) CreateMockHttpClientFactory<TResponse>(string section, string value, TResponse response) 
        {
            var mockConfiguration = new Mock<IConfiguration>();
            var mockConfigurationSection = new Mock<IConfigurationSection>();
            mockConfigurationSection.Setup(c => c.Value).Returns(value);
            mockConfiguration.Setup(c => c.GetSection(section)).Returns(mockConfigurationSection.Object);

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            
            var jsonString = JsonSerializer.Serialize(response);
            HttpContent content = new StringContent(jsonString);

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://recruitment-test.investcloud.com/api/numbers/")
            };

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(c => c.CreateClient("InvestCloudClient")).Returns(httpClient);

            return (mockHttpClientFactory, mockConfiguration);
        }
    }
}