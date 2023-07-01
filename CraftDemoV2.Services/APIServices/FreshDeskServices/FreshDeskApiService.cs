﻿using System.Net.Http.Headers;
using System.Text;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;
using CraftDemoV2.Services.APIServices.FreshDeskServices.Interfaces;

namespace CraftDemoV2.Services.APIServices.FreshDeskServices;

public class FreshDeskApiService:IFreshDeskApiService
{
    public async Task CreateContact(HttpClient client, string contactInfo)
    {


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
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{freshDeskApiKey}:x")));

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var bodyContent = new StringContent(contactInfo, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(urlEndpoint, bodyContent);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("FreshDesk Contact was successfully created");
        }
        else
        {
            Console.WriteLine("Failed to create FreshDesk contact, trying again");

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Trying again");
                Thread.Sleep(500);

                response = await client.PostAsync(urlEndpoint, bodyContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("FreshDesk Contact was successfully created");
                    break;
                }

            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Request failed with status code: {response.StatusCode}");
            }
        }
    }
}