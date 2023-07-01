using CraftDemoV2.API.ResponseModels.GitHubModels.Users;
using CraftDemoV2.Services.APIServices.FreshDeskServices;
using CraftDemoV2.Services.APIServices.GitHubServices;
using CraftDemoV2.Services.BusinessServices;

namespace CraftDemoV2
{
    internal class StartUp
    {
        static async Task Main(string[] args)
        {
            try
            {
                Environment.SetEnvironmentVariable("FreshDeskDomain", "quickbasecraftdemov2-help");
                Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "SOiTuxzkSOYo9FO2l");


                var testService = new GitHubApiService();
                var testFreshDeskService = new FreshDeskApiService();

                var mainTestService = new MainService(testFreshDeskService,testService);

                HttpClient client = new HttpClient();

                {
                   await mainTestService.CreateFreshDeskContactFromGitUser(client, "IamO1Vladi");

                }


                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}