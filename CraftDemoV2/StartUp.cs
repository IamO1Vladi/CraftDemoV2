using CraftDemoV2.API.ResponseModels.GitHubModels.Users;
using CraftDemoV2.Data;
using CraftDemoV2.Services.APIServices.FreshDeskServices;
using CraftDemoV2.Services.APIServices.FreshDeskServices.Interfaces;
using CraftDemoV2.Services.APIServices.GitHubServices;
using CraftDemoV2.Services.BusinessServices;
using CraftDemoV2.Services.DataBaseServices.GitHubUsersDataBase;
using Microsoft.Extensions.DependencyInjection;
using static CraftDemoV2.Services.Configuration.ServicesConfiguration;
using CraftDemoV2.Services.BusinessServices.Interfaces;
using CraftDemoV2.Services.APIServices.GitHubServices.Interfaces;
using CraftDemoV2.Services.DataBaseServices.GitHubUsersDataBase.Interfaces;

namespace CraftDemoV2
{
    internal class StartUp
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please enter a FreshDeskDomain name: ");
                Environment.SetEnvironmentVariable("FreshDeskDomain", Console.ReadLine());//quickbasecraftdemov2-help
                Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "SOiTuxzkSOYo9FO2l");

                Console.WriteLine("Enter a GitHub userName: ");
                string? gitHubUserName=Console.ReadLine();


                var services = new ServiceCollection();
                ConfigureServices(services); //Configures the services for the Dependency Injection
                

                var serviceProvider= services.BuildServiceProvider();
                

                var mainTestService = serviceProvider.GetRequiredService<IMainService>();

                
                HttpClient client = new HttpClient();

                {
                    
                   await mainTestService.CreateOrUpdateFreshDeskContactFromGitUser(client, gitHubUserName);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        
    }
}