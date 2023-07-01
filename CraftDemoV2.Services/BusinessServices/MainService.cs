using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.API.RequestModels.FreshDeskModels;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;
using CraftDemoV2.Services.APIServices.FreshDeskServices.Interfaces;
using CraftDemoV2.Services.APIServices.GitHubServices.Interfaces;
using CraftDemoV2.Services.BusinessServices.Interfaces;
using Newtonsoft.Json;

namespace CraftDemoV2.Services.BusinessServices
{
    public class MainService:IMainService
    {

        private readonly IFreshDeskApiService freshDeskApiService;
        private readonly IGitHubApiService gitHubApiService;

        public MainService(IFreshDeskApiService freshDeskApiService,IGitHubApiService gitHubApiService)
        {
            this.freshDeskApiService=freshDeskApiService;
            this.gitHubApiService=gitHubApiService;
        }

        public async Task CreateFreshDeskContactFromGitUser(HttpClient client, string gitHubUserName)
        {
            try
            {

                GitHubGetUserModel gitHubUser = await gitHubApiService.GetUser(client, gitHubUserName);


                FreshDeskContactModel newFreshDeskContact = new FreshDeskContactModel()
                {
                    Name = string.IsNullOrEmpty(gitHubUser.FullName) ? gitHubUser.UserName : gitHubUser.FullName,
                    Email = gitHubUser.Email,
                    Address = gitHubUser.Location,
                    Description = gitHubUser.Bio,
                    TwitterId = gitHubUser.TwitterUserName,
                    UniqueExternalId = gitHubUser.Id,
                };


                string freshDeskContent = JsonConvert.SerializeObject(newFreshDeskContact);


                await freshDeskApiService.CreateContact(client, freshDeskContent);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
