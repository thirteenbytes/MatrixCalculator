using InvestCloud.MatrixCalculator.Application.Extensions;
using InvestCloud.MatrixCalculator.Application.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace InvestCloud.MatrixCalculator.Application.UnitTests
{
    public class InvestCloudClientTests
    {
        [Fact]
        public async Task GetEndpointSuccessfulTest()
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
            var client = new InvestCloudClient(mockHttpClientFactory.Object, mockConfiguration.Object);
            var response = await client.GetRow("A", 1);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equivalent(result, response);

        }

        [Fact]
        public async Task InitializeEndPointSuccessfulTest()
        {
            int matrixSize = 1000;

            // Arrange
            var result = new ResultOfInt32()
            {
                Success = true,
                Value = matrixSize
            };

            (var mockHttpClientFactory, var mockConfiguration) =
                CreateMockHttpClientFactory<ResultOfInt32>("Endpoints:Initialize", "init/{{size}}", result);

            // Act
            var client = new InvestCloudClient(mockHttpClientFactory.Object, mockConfiguration.Object);
            var response = await client.Initialize(matrixSize);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equivalent(result, response);
        }

        [Fact]
        public async Task ValidateEndpointSuccessfulTest()
        {
            var result = new ResultOfString()
            {
                Success = true,
                Value = "Release the Cracken"
            };

            (var mockHttpClientFactory, var mockConfiguration) =
                CreateMockHttpClientFactory<ResultOfString>("Endpoints:Validate", "validate", result);

            // Act
            var client = new InvestCloudClient(mockHttpClientFactory.Object, mockConfiguration.Object);

            var matrixResult = "0-111";
            var response = await client.Validate(matrixResult.ToMD5());

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equivalent(result, response);
        }

        private (Mock<IHttpClientFactory>, Mock<IConfiguration>) CreateMockHttpClientFactory<TResponse>(string section, string value, TResponse response)
        {

            // Mock a configuration with section and value
            var mockConfiguration = new Mock<IConfiguration>();
            var mockConfigurationSection = new Mock<IConfigurationSection>();
            mockConfigurationSection.Setup(c => c.Value).Returns(value);
            mockConfiguration.Setup(c => c.GetSection(section)).Returns(mockConfigurationSection.Object);

            // Mock the HttpMessageHandler and build the expected response
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