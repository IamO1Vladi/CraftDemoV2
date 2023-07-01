using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;
using CraftDemoV2.Services.APIServices.GitHubServices.Interfaces;
using Newtonsoft.Json;
using static CraftDemoV2.Common.APILimits.GitHubLimits.GitHubApiLimits;

namespace CraftDemoV2.Services.APIServices.GitHubServices
{
    public class GitHubApiService:IGitHubApiService
    {
        public async Task<GitHubGetUserModel> GetUser(HttpClient client, string username)
        {
            string endPointUrl = $"https://api.github.com/users/{username}";
            var gitHubAuthToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");


            client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("CraftDemoV2");

            if (gitHubAuthToken != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", gitHubAuthToken);
            }

            HttpResponseMessage response= await client.GetAsync(endPointUrl);

            int retryAttempts = 0;

            while (retryAttempts != MaxRetryAttempts)
            {
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    GitHubGetUserModel gitHubUser = JsonConvert.DeserializeObject<GitHubGetUserModel>(responseBody);

                    return gitHubUser;
                }
                else if (response.StatusCode >= HttpStatusCode.BadRequest &&
                         response.StatusCode < HttpStatusCode.InternalServerError)
                {

                    throw new Exception($"Client side error(Bad Request): {response.RequestMessage}");
                }
                else if (response.StatusCode >= HttpStatusCode.InternalServerError)
                {
                    Console.WriteLine($"Server side error: {response.RequestMessage}");
                }

                retryAttempts++;
                Console.WriteLine("Retrying");
                Thread.Sleep(RetryDelayMilliseconds);

                response = await client.GetAsync(endPointUrl);

            }

            if (retryAttempts == MaxRetryAttempts)
            {
                throw new Exception("Server is unavailable, please try again later");
            }

            return null;

            //if (!response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine("First request to GitHub was not successful, trying again");
            //    for (int i = 0; i < 10; i++)
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            break;
            //        }

            //        Console.WriteLine("Trying again");
            //        Thread.Sleep(500);
            //        response= await client.GetAsync(endPointUrl);
            //    }

            //    if (!response.IsSuccessStatusCode)
            //    {
            //        throw new Exception($"All requests failed with code: {response.StatusCode}");
            //    }
            //}






        }

      


    }
}
