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
            var gitHubAuthToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN"); //Gets the gitHub token in case one was set


            client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("CraftDemoV2");

            if (gitHubAuthToken != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", gitHubAuthToken);// If a token was presented we add it to the headers
            }

            HttpResponseMessage response= await client.GetAsync(endPointUrl); // Making a Get Request to the GitHub API


            // Here we set up a system to retry the request in case it came back with an Internal Server Error. The MaxRetryAttempts and RetryDelayMilliseconds can be set from the Common Project
            int retryAttempts = 0;

            while (retryAttempts != MaxRetryAttempts)
            {
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();//If the response was successful we read the content as a string

                    GitHubGetUserModel gitHubUser = JsonConvert.DeserializeObject<GitHubGetUserModel>(responseBody);//We create a GitHubUser model from the json

                    return gitHubUser; //We return the GitHubUser
                }
                else if (response.StatusCode >= HttpStatusCode.BadRequest &&
                         response.StatusCode < HttpStatusCode.InternalServerError)
                {

                    throw new Exception($"Client side error(Bad Request): {response.RequestMessage}");//In case there was an error with the request from client side we throw an error and stop the program
                }
                else if (response.StatusCode >= HttpStatusCode.InternalServerError)// If it is a server side error we print out a message and proceed with retrying until the request is successful or we get to the Maximum monmouth of times
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
                throw new Exception("Server is unavailable, please try again later"); //If we git the Maximum retries we throw an error that the server is not working currently
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
