using CraftDemoV2.Services.APIServices.GitHubServices;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CraftDemoV2.Tests.APIServices.GitHubServices
{
    [TestFixture]
    public class GitHubApiServiceTests
    {
        private MockRepository mockRepository;



        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private GitHubApiService CreateService()
        {
            return new GitHubApiService();
        }

        [Test]
        public async Task GetUser_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            HttpClient client = null;
            string username = null;

            // Act
            var result = await service.GetUser(
                client,
                username);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
