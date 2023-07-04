using CraftDemoV2.Services.APIServices.GitHubServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.Services.APIServices.FreshDeskServices;
using Moq.Protected;
using System.Net;
using CraftDemoV2.API.RequestModels.FreshDeskModels;
using CraftDemoV2.API.ResponseModels.FreshDeskModels;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Drawing;

namespace CraftDemoV2.Tests.FreshDesk
{
    internal class FreshDeskApiServiceTests
    {

        private Mock<HttpMessageHandler> mockHandler;
        private FreshDeskApiService service;
        [SetUp]
        public void SetUp()
        {
            this.mockHandler = new Mock<HttpMessageHandler>();
            this.service = new FreshDeskApiService();
        }

        [Test]

        public async Task All_Fucntions_Should_Throw_An_Excetipn_When_FreshDeskDomain_isNull()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new HttpClient(mockHandler.Object);

            Environment.SetEnvironmentVariable("FreshDeskDomain",null);

            try
            {
                await service.GetContact(client, "test");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Value cannot be null. (Parameter 'FreshDeskDomain cannot be null')", ex.Message);
            }

            try
            {
                await service.UpdateContact(client, "test", "test");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Value cannot be null. (Parameter 'FreshDeskDomain cannot be null')", ex.Message);
            }

            try
            {
                await service.CreateContact(client, "test");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Value cannot be null. (Parameter 'FreshDeskDomain cannot be null')", ex.Message);
            }




        }

        [Test]
        public async Task All_Fucntions_Should_Throw_An_Excetipn_When_FreshDeskApiKey_isNull()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new HttpClient(mockHandler.Object);
            Environment.SetEnvironmentVariable("FreshDeskDomain","test");
            Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", null);

            try
            {
                await service.GetContact(client, "test");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Value cannot be null. (Parameter 'FreshDeskKey cannot be null')", ex.Message);
            }

            try
            {
                await service.UpdateContact(client, "test", "test");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Value cannot be null. (Parameter 'FreshDeskKey cannot be null')", ex.Message);
            }

            try
            {
                await service.CreateContact(client, "test");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Value cannot be null. (Parameter 'FreshDeskKey cannot be null')", ex.Message);
            }




        }


        [Test]
        public async Task GetContact_Should_Return_A_FreshDeskContactResponseModel()
        {

            Environment.SetEnvironmentVariable("FreshDeskDomain", "test");
            Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "test");

            FreshDeskResponseContactModel expectedContact = new FreshDeskResponseContactModel()
            {
                Address = "TestAddress",
                Description = "testDesc",
                Email = "vladi@abv.bg",
                Name = "Vladi",
                TwitterId = "twt",
                UniqueExternalId = "2001"
            };

            FreshDeskResponseContactModel[] jsonForResponse = new[]
            {
                expectedContact
            };

            

            var expectedResponseBody = JsonConvert.SerializeObject(jsonForResponse);

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expectedResponseBody, Encoding.UTF8, "application/json"),
            };

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var client = new HttpClient(mockHandler.Object);

            var result = await service.GetContact(client, expectedContact.UniqueExternalId);

            var resultBody=JsonConvert.SerializeObject(result);

            var expectedBody = JsonConvert.SerializeObject(expectedContact);

            Assert.AreEqual(expectedBody, resultBody);

        }


        [Test]

        public async Task GetContact_Should_Throw_An_Exception_When_The_Response_Is_InternServerError()
        {

            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);


            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new HttpClient(mockHandler.Object);

            string testId = "WillNotWork";

            try
            {
                await service.GetContact(client, testId);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Server is unavailable, please try again later", ex.Message);
            }

        }


        [Test]

        public async Task GetContact_Should_Throw_An_Exception_When_The_Response_Is_Bad_Request()
        {

            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);


            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new HttpClient(mockHandler.Object);

            string testId = "WillNotWork";

            try
            {
                await service.GetContact(client, testId);
            }
            catch (Exception ex)
            {
                Assert.AreEqual($"Client error(Bad request): {responseMessage.RequestMessage}", ex.Message);
            }
        }


        [Test]

        public async Task GetContact_Should_Return_Null_When_No_Contacts_Are_Found()
        {

            Environment.SetEnvironmentVariable("FreshDeskDomain", "test");
            Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "test");


            FreshDeskResponseContactModel[] jsonForResponse = new FreshDeskResponseContactModel[5];
        

            var expectedResponseBody = JsonConvert.SerializeObject(jsonForResponse);

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expectedResponseBody, Encoding.UTF8, "application/json"),
            };

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var client = new HttpClient(mockHandler.Object);

            var result = await service.GetContact(client,"test");


            Assert.AreEqual(null,result);

        }


        [Test]

        public async Task CreateContact_Successfully_Creates_A_Contact()
        {

            Environment.SetEnvironmentVariable("FreshDeskDomain", "test");
            Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "test");

            FreshDeskContactModel newUserTest = new FreshDeskContactModel()
            {
                Address = "TestAddress",
                Description = "testDesc",
                Email = "vladi@abv.bg",
                Name = "Vladi",
                TwitterId = "twt",
                UniqueExternalId = "2001"
            };

            var expectedContactInfo = JsonConvert.SerializeObject(newUserTest);

            var expectedUri = new Uri("https://test.freshdesk.com/api/v2/contacts");

          

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri == expectedUri &&
                        req.Content.ReadAsStringAsync().Result == expectedContactInfo),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Created));

           
            var client = new HttpClient(mockHandler.Object);
          

           
            await service.CreateContact(client,expectedContactInfo);

            
            mockHandler.Protected().Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == expectedUri &&
                    req.Content.ReadAsStringAsync().Result == expectedContactInfo),
                ItExpr.IsAny<CancellationToken>());

        }


        [Test]

        public async Task CreateContact_Should_Throw_An_Exception_When_The_Response_Is_InternServerError()
        {
            Environment.SetEnvironmentVariable("FreshDeskDomain", "test");
            Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "test");
            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);


            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new HttpClient(mockHandler.Object);

            string testInfo = "WillNotWork";

            try
            {
                await service.CreateContact(client, testInfo);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Server is unavailable, please try again later", ex.Message);
            }

        }


        [Test]

        public async Task CreateContact_Should_Throw_An_Exception_When_The_Response_Is_Bad_Request()
        {
            Environment.SetEnvironmentVariable("FreshDeskDomain", "test");
            Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "test");
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);


            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new HttpClient(mockHandler.Object);

            string testInfo = "WillNotWork";

            try
            {
                await service.CreateContact(client, testInfo);
            }
            catch (Exception ex)
            {
                Assert.AreEqual($"Client error(Bad request): {responseMessage.RequestMessage}", ex.Message);
            }
        }

        [Test]
        public async Task UpdateContact_Should_Update_Successfully_And_Return_A_Message()
        {
            Environment.SetEnvironmentVariable("FreshDeskDomain", "test");
            Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "test");

            FreshDeskContactModel newUserTest = new FreshDeskContactModel()
            {
                Address = "TestAddress",
                Description = "testDesc",
                Email = "vladi@abv.bg",
                Name = "Vladi",
                TwitterId = "twt",
                UniqueExternalId = "2001"
            };

            var expectedContactInfo = JsonConvert.SerializeObject(newUserTest);

            var expectedContactId = "123";

            var expectedUri = new Uri($"https://test.freshdesk.com/api/v2/contacts/{expectedContactId}");

      
           
           
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Put &&
                        req.RequestUri == expectedUri &&
                        req.Content.ReadAsStringAsync().Result == expectedContactInfo),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

           
            var client = new HttpClient(mockHandler.Object);
            

           
            await service.UpdateContact(client,expectedContactInfo, expectedContactId);

           
            mockHandler.Protected().Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri == expectedUri &&
                    req.Content.ReadAsStringAsync().Result == expectedContactInfo),
                ItExpr.IsAny<CancellationToken>());

           
        }


        [Test]

        public async Task UpdateContact_Should_Throw_An_Exception_When_The_Response_Is_InternServerError()
        {
            Environment.SetEnvironmentVariable("FreshDeskDomain", "test");
            Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "test");
            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);


            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new HttpClient(mockHandler.Object);

            string testInfo = "WillNotWork";
            string testId = "test";
            try
            {
                await service.UpdateContact(client, testInfo,testId);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Server is unavailable, please try again later", ex.Message);
            }

        }


        [Test]

        public async Task UpdateContact_Should_Throw_An_Exception_When_The_Response_Is_Bad_Request()
        {
            Environment.SetEnvironmentVariable("FreshDeskDomain", "test");
            Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "test");
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);


            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new HttpClient(mockHandler.Object);

            string testInfo = "WillNotWork";
            string testId = "test";

            try
            {
                await service.UpdateContact(client, testInfo,testId);
            }
            catch (Exception ex)
            {
                Assert.AreEqual($"Client error(Bad request): {responseMessage.RequestMessage}", ex.Message);
            }
        }

        [Test]
        public async Task UpdateContact_Confriming_Object_Is_Updated()
        {
            Environment.SetEnvironmentVariable("FreshDeskDomain", "test");
            Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "test");

            // Declare variables to capture the request
            HttpRequestMessage capturedRequest = null;
            string requestBody = null;

            FreshDeskContactModel newUserTest = new FreshDeskContactModel()
            {
                Address = "TestAddress",
                Description = "testDesc",
                Email = "vladi@abv.bg",
                Name = "Vladi",
                TwitterId = "twt",
                UniqueExternalId = "2001"
            };

            var expectedContactInfo = JsonConvert.SerializeObject(newUserTest);

            var expectedContactId = "123";

          
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, cancellationToken) =>
                {
                    capturedRequest = request;
                    requestBody = request.Content.ReadAsStringAsync().Result;
                })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

          
            var client = new HttpClient(mockHandler.Object);
            

            
            
            
            await service.UpdateContact(client,expectedContactInfo, expectedContactId);

           
            Assert.AreEqual(expectedContactInfo, requestBody);
        }


        [Test]
        public async Task CreateContact_Confriming_Object_Is_Updated()
        {
            Environment.SetEnvironmentVariable("FreshDeskDomain", "test");
            Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "test");

            // Declare variables to capture the request
            HttpRequestMessage capturedRequest = null;
            string requestBody = null;

            FreshDeskContactModel newUserTest = new FreshDeskContactModel()
            {
                Address = "TestAddress",
                Description = "testDesc",
                Email = "vladi@abv.bg",
                Name = "Vladi",
                TwitterId = "twt",
                UniqueExternalId = "2001"
            };

            var expectedContactInfo = JsonConvert.SerializeObject(newUserTest);

            

          
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, cancellationToken) =>
                {
                    capturedRequest = request;
                    requestBody = request.Content.ReadAsStringAsync().Result;
                })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var client = new HttpClient(mockHandler.Object);


           


            await service.CreateContact(client, expectedContactInfo);

           
            Assert.AreEqual(expectedContactInfo, requestBody);
        }

    }
}
