using CraftDemoV2.API.ResponseModels.GitHubModels.Users;
using CraftDemoV2.Services.APIServices.GitHubServices;

namespace CraftDemoV2
{
    internal class StartUp
    {
        static async Task Main(string[] args)
        {
            
            Environment.SetEnvironmentVariable("PasswordTest",Console.ReadLine());

            var doeItWork = Environment.GetEnvironmentVariable("PasswordTest");

            Console.WriteLine(doeItWork);

            var testService = new GitHubApiService();

            HttpClient client = new HttpClient();
            try
            {
                GitHubGetUserModel testModel = await testService.GetUser(client, "IamO1Vladisadasdasd");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine();

        }
    }
}