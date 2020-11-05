using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AGL.Models;
using AGL.Services.Implementation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AGL.Services.Tests
{
    public class HttpServiceTests
    {
        readonly string correctUri = "http://agl-developer-test.azurewebsites.net/people.json";
        readonly string incorrectUri = "http://agl-developer-test.azurewebsites.net/person.json";

        private HttpClient GetHttpClient(string clientName) {
            HttpClient httpClient = null;
            switch (clientName)
            {
                case "CorrectUri":
                    httpClient = new HttpClient
                    {
                        BaseAddress = new Uri(correctUri)
                    };
                    break;
                case "IncorrectUri":
                    httpClient = new HttpClient
                    {
                        BaseAddress = new Uri(incorrectUri)
                    };
                    break;
                default:
                    break;
            }
            
            return httpClient;
        }

        [Fact]
        public async Task RetrieveData_From_CorrectUri()
        {
            // Arrange
            string clientName = "CorrectUri";
            var mockHttpfactory = new Mock<IHttpClientFactory>();
            mockHttpfactory.Setup(httpFactory => httpFactory.CreateClient(clientName))
                .Returns(GetHttpClient(clientName));
            var mockLogger = new Mock<ILogger<HttpService>>(); 
            var httpService = new HttpService(mockHttpfactory.Object, mockLogger.Object);

            //NullReferenceException

            // Act
            var result = await httpService.RetrieveJsonDataAsync<List<People>>(clientName);

            // Assert
            Assert.IsType<List<People>>(result);            
        }

        [Fact]
        public async Task ThrowExceptionWhenIncorrectUri()
        {
            // Arrange
            string clientName = "IncorrectUri";
            string errorMessage = "Unable to retrieve data.";
            var mockHttpfactory = new Mock<IHttpClientFactory>();
            mockHttpfactory.Setup(httpFactory => httpFactory.CreateClient(clientName))
                .Returns(GetHttpClient(clientName));
            var mockLogger = new Mock<ILogger<HttpService>>();
            var httpService = new HttpService(mockHttpfactory.Object, mockLogger.Object);

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => httpService.RetrieveJsonDataAsync<List<People>>(clientName));

            // Assert
            Assert.Equal(errorMessage, ex.Message);
        }

        [Fact]
        public async Task ThrowExceptionWhenClientNotExists()
        {
            // Arrange
            string clientName = "ClientNotExists";
            string errorMessage = "Object reference not set to an instance of an object.";
            var mockHttpfactory = new Mock<IHttpClientFactory>();
            mockHttpfactory.Setup(httpFactory => httpFactory.CreateClient(clientName))
                .Returns(GetHttpClient(clientName));
            var mockLogger = new Mock<ILogger<HttpService>>();
            var httpService = new HttpService(mockHttpfactory.Object, mockLogger.Object);

            // Act
            var ex = await Assert.ThrowsAsync<NullReferenceException>(() => httpService.RetrieveJsonDataAsync<List<People>>(clientName));

            // Assert
            Assert.Equal(errorMessage, ex.Message);
        }
    }
}
