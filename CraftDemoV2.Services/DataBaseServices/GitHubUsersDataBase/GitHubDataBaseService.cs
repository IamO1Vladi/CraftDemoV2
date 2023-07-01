using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;
using CraftDemoV2.Data;
using CraftDemoV2.Data.Models.GitHubModels;
using CraftDemoV2.Services.DataBaseServices.GitHubUsersDataBase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CraftDemoV2.Services.DataBaseServices.GitHubUsersDataBase
{
    public class GitHubDataBaseService:IGitHubDataBaseService
    {

        private readonly CraftDemoV2DbContext dataBaseContext;

        public GitHubDataBaseService()
        {
            this.dataBaseContext = new CraftDemoV2DbContext();
        }


        public async Task AddUser(GitHubGetUserModel user)
        {

            

            GitHubUser newUser = new GitHubUser()
            {
                CreatedDate = user.CreatedAt,
                Id = int.Parse(user.Id),
                Name = string.IsNullOrEmpty(user.FullName)?user.UserName:user.FullName,
                UserName = user.UserName
            };

            if (await dataBaseContext.GitHubUsersAddedAsContacts.AnyAsync(g => g.Id == newUser.Id))
            {
                Console.WriteLine($"{user.UserName} is already in DataBase");
            }
            else
            {
                await using (dataBaseContext)
                {
                    await dataBaseContext.Database.OpenConnectionAsync();
                    await dataBaseContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT GitHubUsersAddedAsContacts ON");//Setting this to on so i can add an explicit ID as a primary key
                    await dataBaseContext.AddAsync(newUser);
                    await dataBaseContext.SaveChangesAsync();
                    await dataBaseContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT GitHubUsersAddedAsContacts OFF");
                }

                Console.WriteLine($"{newUser.UserName} successfully added to database");
            }
        }
    }
}
