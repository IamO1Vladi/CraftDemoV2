using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
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

        //This method is used to add a user to the database
        public async Task AddUser(GitHubGetUserModel user)
        {

            
            //Here we create the new user that will be added 
            GitHubUser newUser = new GitHubUser()
            {
                CreatedDate = user.CreatedAt,
                Id = int.Parse(user.Id),
                Name = string.IsNullOrEmpty(user.FullName)?user.UserName:user.FullName,
                UserName = user.UserName
            };

            if (await dataBaseContext.GitHubUsersAddedAsContacts.AnyAsync(g => g.Id == newUser.Id))
            {
                Console.WriteLine($"{user.UserName} is already in DataBase"); //In this if statement we check if the user is already in the database. If he is we proceed to print out a message and don't make any changes
            }
            else
            {
                //if he isn't we use the context to add the user. Because the key field is an integer we have to turn Identity insert to on to add the user and then turn it to off for safe measures
                await using (dataBaseContext)
                {
                    await dataBaseContext.Database.OpenConnectionAsync();
                    await dataBaseContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT GitHubUsersAddedAsContacts ON");//Setting this to on so i can add an explicit ID as a primary key
                    await dataBaseContext.AddAsync(newUser);
                    await dataBaseContext.SaveChangesAsync();
                    await dataBaseContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT GitHubUsersAddedAsContacts OFF");
                }

                Console.WriteLine($"{newUser.UserName} successfully added to database");//Finally we print out a message that the user was added
            }
        }

        //This method is used to update a user in the database
        public async Task UpdateUser(GitHubGetUserModel user)
        {

            var userToUpdate =
                await dataBaseContext.GitHubUsersAddedAsContacts.FirstOrDefaultAsync(u => u.Id == int.Parse(user.Id));//Here we get the user from the database

            if (userToUpdate == null)
            {
                throw new Exception("User not found in database");//If he isn't in there we throw an exception message
            }

            //If he is we use the database we proceed with updating the name and username as those are the only 2 fields that can change.
            await using (dataBaseContext)
            {
                userToUpdate.Name = string.IsNullOrEmpty(user.FullName) ? user.UserName : user.FullName;
                userToUpdate.UserName = user.UserName;

                await dataBaseContext.SaveChangesAsync();
            }

            Console.WriteLine($"{userToUpdate.UserName} successfully updated in the database");//Finally we print out an error message
        }
    }
}
