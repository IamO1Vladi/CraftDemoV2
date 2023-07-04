using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;
using CraftDemoV2.Services.APIServices.GitHubServices;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace CraftDemoV2.Tests.GitHub
{
    internal class GitHubApiServiceTests
    {
        private Mock<HttpMessageHandler> mockHandler;
        private GitHubApiService service;
        [SetUp]
        public void SetUp()
        {
             this.mockHandler = new Mock<HttpMessageHandler>();
            this.service = new GitHubApiService();
        }


        [Test]

        public async Task GetUser_Should_Throw_An_Exception_When_The_Response_Failed_With_Code_For_Client_Side()
        {

            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);


            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new HttpClient(mockHandler.Object);

            string testUser = "WillNotWork";

            try
            {
                await service.GetUser(client, testUser);
            }
            catch (Exception ex)
            {
                Assert.AreEqual($"Client side error(Bad Request): {responseMessage.RequestMessage}",ex.Message);
            }

        }

        [Test]

        public async Task GetUser_Should_Throw_An_Internal_Error_Exception()
        {

            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);


            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new HttpClient(mockHandler.Object);

            string testUser = "WillNotWork";

            try
            {
                await service.GetUser(client, testUser);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Server is unavailable, please try again later", ex.Message);
            }

        }

        [Test]

        public async Task GetUser_Should_Return_A_GitHubUserResponse_Model()
        {


            
            GitHubGetUserModel expectedUser = new GitHubGetUserModel()
            {
                AvatarUrl = "testUrl",
                Bio = "Test bio",
                CreatedAt = DateTime.Today,
                Email = "testemail.com",
                FullName = "Vladi Vladimirov",
                Id = "Test Id",
                Location = "test location",
                TwitterUserName = "testTwitter",
                UserName = "C9Vladi"
            };

            var expectedResponseBody= JsonConvert.SerializeObject(expectedUser);

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expectedResponseBody,Encoding.UTF8,"application/json"),
            };

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var client = new HttpClient(mockHandler.Object);

            var result = await service.GetUser(client, expectedUser.UserName);

            string expectedResultString = JsonConvert.SerializeObject(result);

           Assert.AreEqual(expectedResponseBody,expectedResultString);

        }

        [Test]

        public async Task GetUser_Should_Return_A_GitHubUserResponse_Model_When_Token_is_Added()
        {


            Environment.SetEnvironmentVariable("GITHUB_TOKEN","test");
            GitHubGetUserModel expectedUser = new GitHubGetUserModel()
            {
                AvatarUrl = "testUrl",
                Bio = "Test bio",
                CreatedAt = DateTime.Today,
                Email = "testemail.com",
                FullName = "Vladi Vladimirov",
                Id = "Test Id",
                Location = "test location",
                TwitterUserName = "testTwitter",
                UserName = "C9Vladi"
            };

            var expectedResponseBody = JsonConvert.SerializeObject(expectedUser);

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expectedResponseBody, Encoding.UTF8, "application/json"),
            };

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var client = new HttpClient(mockHandler.Object);

            var result = await service.GetUser(client, expectedUser.UserName);

            string expectedResultString = JsonConvert.SerializeObject(result);

            Assert.AreEqual(expectedResponseBody, expectedResultString);

        }
    }
}
