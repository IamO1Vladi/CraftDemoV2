using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.API.RequestModels.FreshDeskModels;
using CraftDemoV2.API.ResponseModels.FreshDeskModels;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;

namespace CraftDemoV2.Services.BusinessServices.Interfaces
{
    public interface IMainService
    {

        public Task CreateOrUpdateFreshDeskContactFromGitUser(HttpClient client, string gitHubUserName);

        

    }
}
