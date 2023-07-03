using CraftDemoV2.Services.APIServices.FreshDeskServices;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CraftDemoV2.Tests.APIServices.FreshDeskServices
{
    [TestFixture]
    public class FreshDeskApiServiceTests
    {
        private MockRepository mockRepository;



        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private FreshDeskApiService CreateService()
        {
            return new FreshDeskApiService();
        }

        [Test]
        public async Task CreateContact_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            HttpClient client = null;
            string contactInfo = null;

            // Act
            await service.CreateContact(
                client,
                contactInfo);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetContact_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            HttpClient client = null;
            string contactId = null;

            // Act
            var result = await service.GetContact(
                client,
                contactId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateContact_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            HttpClient client = null;
            string contactInfo = null;
            string contactId = null;

            // Act
            await service.UpdateContact(
                client,
                contactInfo,
                contactId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
