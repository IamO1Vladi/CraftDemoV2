using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;

namespace CraftDemoV2.Services.APIServices.GitHubServices.Interfaces
{
    public interface IGitHubApiService
    {
       
        public Task<GitHubGetUserModel> GetUser(HttpClient client,string username);

    }
}
