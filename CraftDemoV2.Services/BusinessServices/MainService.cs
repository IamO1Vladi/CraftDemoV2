using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.API.RequestModels.FreshDeskModels;
using CraftDemoV2.API.ResponseModels.FreshDeskModels;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;
using CraftDemoV2.Services.APIServices.FreshDeskServices.Interfaces;
using CraftDemoV2.Services.APIServices.GitHubServices.Interfaces;
using CraftDemoV2.Services.BusinessServices.Interfaces;
using CraftDemoV2.Services.DataBaseServices.GitHubUsersDataBase.Interfaces;
using Newtonsoft.Json;

namespace CraftDemoV2.Services.BusinessServices
{
    public class MainService:IMainService
    {

        private readonly IFreshDeskApiService freshDeskApiService;
        private readonly IGitHubApiService gitHubApiService;
        private readonly IGitHubDataBaseService gitHubDataBaseService;

        public MainService(IFreshDeskApiService freshDeskApiService,IGitHubApiService gitHubApiService, IGitHubDataBaseService gitHubDataBaseService)
        {
            this.freshDeskApiService=freshDeskApiService;
            this.gitHubApiService=gitHubApiService;
            this.gitHubDataBaseService=gitHubDataBaseService;
        }

        public async Task CreateOrUpdateFreshDeskContactFromGitUser(HttpClient client, string gitHubUserName)
        {
            try
            {

                GitHubGetUserModel gitHubUser = await gitHubApiService.GetUser(client, gitHubUserName);//Gets the GitHub user by the provided username

                FreshDeskResponseContactModel freshDeskUser =
                    await freshDeskApiService.GetContact(client, gitHubUser.Id) == null
                        ? new FreshDeskResponseContactModel()
                        : await freshDeskApiService.GetContact(client, gitHubUser.Id);//This checkes if the user has already been added to Contacts, if the user was added it returns the FreshDesk contact with an id if the user isn't added it returns an empty object

                FreshDeskContactModel newFreshDeskContact = new FreshDeskContactModel()
                {
                    Name = string.IsNullOrEmpty(gitHubUser.FullName) ? gitHubUser.UserName : gitHubUser.FullName,
                    Email = gitHubUser.Email,
                    Address = gitHubUser.Location,
                    Description = gitHubUser.Bio,
                    TwitterId = gitHubUser.TwitterUserName,
                    UniqueExternalId = gitHubUser.Id,
                };

                if (freshDeskUser.UniqueExternalId==null)//If there is no UniqueExternalId(GitHubId) we proceed with creating a new FreshDesk Contact
                {

                    string freshDeskContent = JsonConvert.SerializeObject(newFreshDeskContact);// Creating a json representation of the contact


                    await freshDeskApiService.CreateContact(client, freshDeskContent);//Creating the contact

                    await gitHubDataBaseService.AddUser(gitHubUser);// Adding the contact to the database
                }
                else
                {

                    string freshDeskContent = JsonConvert.SerializeObject(newFreshDeskContact); // Creating a json representation of the contact

                    await freshDeskApiService.UpdateContact(client,freshDeskContent,freshDeskUser.Id); //Updating the contact in FreshDesk

                    await gitHubDataBaseService.UpdateUser(gitHubUser); //Updates the GitUser in the database
                }

                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message==null?ex.Message:ex.InnerException.Message);
            }
            

        }

        
    }
}
