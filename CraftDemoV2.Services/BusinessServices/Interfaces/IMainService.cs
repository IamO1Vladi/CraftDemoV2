using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;

namespace CraftDemoV2.Services.BusinessServices.Interfaces
{
    public interface IMainService
    {

        public Task CreateFreshDeskContactFromGitUser(HttpClient client, string gitHubUserName);

    }
}
