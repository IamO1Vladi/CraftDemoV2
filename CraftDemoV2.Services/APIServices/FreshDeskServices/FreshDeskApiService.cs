using System.Net;
using System.Net.Http.Headers;
using System.Text;
using CraftDemoV2.API.RequestModels.FreshDeskModels;
using CraftDemoV2.API.ResponseModels.FreshDeskModels;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;
using CraftDemoV2.Services.APIServices.FreshDeskServices.Interfaces;
using Newtonsoft.Json;
using static CraftDemoV2.Common.APILimits.FreshDeskLimits.FreshDeskApiLimits;

namespace CraftDemoV2.Services.APIServices.FreshDeskServices;

public class FreshDeskApiService:IFreshDeskApiService
{
    public async Task CreateContact(HttpClient client, string contactInfo)
    {

        //Here we get the FreshDeskDomain and FreshDesk token. And in case they have not been provided we throw and Exception
        string? freshDeskDomain = Environment.GetEnvironmentVariable("FreshDeskDomain");

        string? freshDeskApiKey = Environment.GetEnvironmentVariable("FRESHDESK_TOKEN");

        string urlEndpoint = $"https://{freshDeskDomain}.freshdesk.com/api/v2/contacts";

        if(string.IsNullOrEmpty(freshDeskDomain))
        {
            throw new ArgumentNullException("FreshDeskDomain cannot be null");
        }

        if (string.IsNullOrEmpty(freshDeskApiKey))
        {
            throw new ArgumentNullException("FreshDeskKey cannot be null");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{freshDeskApiKey}:x")));//Adding the Authorization header as required by the FreshDeskAPI

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//Adding the Accept header as required by the FreshDeskAPI

        var bodyContent = new StringContent(contactInfo, Encoding.UTF8, "application/json"); //Creating the body for the request

        //Here we set up a logic to retry the API request in case it fails with a server error. The MaxRetryAttempts and the RetryDelayMilliseconds are set in the Common project
        int retryAttempts = 0;
        HttpResponseMessage response = await client.PostAsync(urlEndpoint, bodyContent); //We make the request

        while (retryAttempts != MaxRetryAttempts)
        {
            

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("FreshDesk Contact was successfully created"); // If the request was successful we print out a message
                break;
            }
            else if (response.StatusCode >= HttpStatusCode.BadRequest &&
                     response.StatusCode < HttpStatusCode.InternalServerError)
            {
                throw new Exception($"Client error(Bad request): {response.RequestMessage}"); //If the error code is coming from client side we throw an error and stop the program
                
            }
            else if (response.StatusCode >= HttpStatusCode.InternalServerError)//In case the error comes from the server we print out a message and proceed with retrying the request.
            {
                Console.WriteLine($"There was a server side error: {response.RequestMessage}");

            }

            retryAttempts++;
            Console.WriteLine("Retrying");

            Thread.Sleep(RetryDelayMilliseconds);

            response = await client.PostAsync(urlEndpoint, bodyContent);

        }

        if (retryAttempts == MaxRetryAttempts)
        {
            throw new Exception("Server is unavailable, please try again later");//In case we hit the MaxRetryAttempts we throw an Exception that the server is unavailable and stop the pgoram.
        }

        //if (response.IsSuccessStatusCode)
        //{
        //    Console.WriteLine("FreshDesk Contact was successfully created");
        //}
        //else if(response.StatusCode>=HttpStatusCode.InternalServerError)
        //{
        //    Console.WriteLine("Failed to create FreshDesk contact, trying again");

        //    for (int i = 0; i < 10; i++)
        //    {
        //        Console.WriteLine("Trying again");
        //        Thread.Sleep(500);

        //        response = await client.PostAsync(urlEndpoint, bodyContent);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine("FreshDesk Contact was successfully created");
        //            break;
        //        }

        //    }

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"Request failed with status code: {response.StatusCode}");
        //    }
        //}
    }

    public async Task<FreshDeskResponseContactModel> GetContact(HttpClient client, string contactId)
    {
        //Here we get the freskDeskDomain and API key from the environmental variables and check if they have been added. In case they haven't we throw an error and stop the programm
        string? freshDeskDomain = Environment.GetEnvironmentVariable("FreshDeskDomain");

        string? freshDeskApiKey = Environment.GetEnvironmentVariable("FRESHDESK_TOKEN");

        if (string.IsNullOrEmpty(freshDeskDomain))
        {
            throw new ArgumentNullException("FreshDeskDomain cannot be null");
        }

        if (string.IsNullOrEmpty(freshDeskApiKey))
        {
            throw new ArgumentNullException("FreshDeskKey cannot be null");
        }


        string urlEndpoint = $"https://{freshDeskDomain}.freshdesk.com/api/v2/contacts?unique_external_id={contactId}";//Setting the URL for the request

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{freshDeskApiKey}:x")));//Adding the authorization header based on FreshDesk API 

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//Adding an Accept header as required by FreshDesk

        HttpResponseMessage response= await client.GetAsync(urlEndpoint); //Making the request

        //Here we set up a system to retry the request in case it has failed. The MaxRetryAttempts and RetryDelayMilliseconds are set up in the Common project
        int retryAttempts = 0;

        while (retryAttempts != MaxRetryAttempts)
        {


            if (response.IsSuccessStatusCode)
            {

                FreshDeskResponseContactModel[] contact =
                     JsonConvert.DeserializeObject<FreshDeskResponseContactModel[]>(await response.Content.ReadAsStringAsync());//Getting the contact and creating a FreshDesk Object that we can return in case the request is successful

                return contact[0];
            }
            else if (response.StatusCode >= HttpStatusCode.BadRequest &&
                     response.StatusCode < HttpStatusCode.InternalServerError)
            {
                throw new Exception($"Client error(Bad request): {response.RequestMessage}");//In case we get a client side error we thrown an Exception and stop the program

            }
            else if (response.StatusCode >= HttpStatusCode.InternalServerError)//Here we check if the error is coming from the Server
            {
                Console.WriteLine($"There was a server side error: {response.RequestMessage}");

            }
            //In case it comes from the server we print out a message that we will be trying the request 
            retryAttempts++;
            Console.WriteLine("Retrying");

            Thread.Sleep(RetryDelayMilliseconds);

            response = await client.GetAsync(urlEndpoint);

        }

        if (retryAttempts == MaxRetryAttempts)
        {
            throw new Exception("Server is unavailable, please try again later");//In case we hit the maxretrylimit we throw an exception that the server is unavailable
        }

        return null;

    }

    public async Task UpdateContact(HttpClient client, string contactInfo,string contactId)
    {
        //Here we get the freshDesk domain and api key so we can use their api. If we they aren't provided an exception will be thrown.
        string? freshDeskDomain = Environment.GetEnvironmentVariable("FreshDeskDomain");

        string? freshDeskApiKey = Environment.GetEnvironmentVariable("FRESHDESK_TOKEN");

        if (string.IsNullOrEmpty(freshDeskDomain))
        {
            throw new ArgumentNullException("FreshDeskDomain cannot be null");
        }

        if (string.IsNullOrEmpty(freshDeskApiKey))
        {
            throw new ArgumentNullException("FreshDeskKey cannot be null");
        }


        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{freshDeskApiKey}:x")));//Adding the authorization header so we can use the request.

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//Adding the accept header as required by FreshDesk API


        string urlEndpoint = $"https://{freshDeskDomain}.freshdesk.com/api/v2/contacts/{contactId}";//Creating the url for the specific user

        var bodyContent = new StringContent(contactInfo, Encoding.UTF8, "application/json");//Here we create the body for the request

        //Here we set up a logic to retry the request in case we get a server side error. The MaxRetryAttempts and RetryDelayMilliseconds are set in the Common project
        int retryAttempts = 0;
        HttpResponseMessage response = await client.PutAsync(urlEndpoint, bodyContent);

        while (retryAttempts != MaxRetryAttempts)
        {


            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("FreshDesk Contact was successfully updated");//If the request was successful we print out a message and end the while loop
                break;
            }
            else if (response.StatusCode >= HttpStatusCode.BadRequest &&
                     response.StatusCode < HttpStatusCode.InternalServerError)
            {
                throw new Exception($"Client error(Bad request): {response.RequestMessage}");//If the error comes from client side the program will throw and exception message and stop

            }
            else if (response.StatusCode >= HttpStatusCode.InternalServerError)
            {
                Console.WriteLine($"There was a server side error: {response.RequestMessage}");//If the error is coming from the server a message will appear and the while loop will try to make the request agian

            }

            retryAttempts++;
            Console.WriteLine("Retrying");

            Thread.Sleep(RetryDelayMilliseconds);

            response = await client.PutAsync(urlEndpoint, bodyContent);

        }

        if (retryAttempts == MaxRetryAttempts)
        {
            throw new Exception("Server is unavailable, please try again later");//In case we hit the Maximum retry attempts the program will throw an exception and stop
        }


    }
}