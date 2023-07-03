using CraftDemoV2.Services.APIServices.FreshDeskServices.Interfaces;
using CraftDemoV2.Services.APIServices.GitHubServices.Interfaces;
using CraftDemoV2.Services.BusinessServices;
using CraftDemoV2.Services.DataBaseServices.GitHubUsersDataBase.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CraftDemoV2.Tests.BusinessServices
{
    [TestFixture]
    public class MainServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IFreshDeskApiService> mockFreshDeskApiService;
        private Mock<IGitHubApiService> mockGitHubApiService;
        private Mock<IGitHubDataBaseService> mockGitHubDataBaseService;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockFreshDeskApiService = this.mockRepository.Create<IFreshDeskApiService>();
            this.mockGitHubApiService = this.mockRepository.Create<IGitHubApiService>();
            this.mockGitHubDataBaseService = this.mockRepository.Create<IGitHubDataBaseService>();
        }

        private MainService CreateService()
        {
            return new MainService(
                this.mockFreshDeskApiService.Object,
                this.mockGitHubApiService.Object,
                this.mockGitHubDataBaseService.Object);
        }

        [Test]
        public async Task CreateOrUpdateFreshDeskContactFromGitUser_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            HttpClient client = null;
            string gitHubUserName = null;

            // Act
            await service.CreateOrUpdateFreshDeskContactFromGitUser(
                client,
                gitHubUserName);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
