using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;
using CraftDemoV2.Data.Models.GitHubModels;

namespace CraftDemoV2.Services.DataBaseServices.GitHubUsersDataBase.Interfaces
{
    public interface IGitHubDataBaseService
    {

        public Task AddUser(GitHubGetUserModel  user);

        public Task UpdateUser(GitHubGetUserModel user);

    }
}
