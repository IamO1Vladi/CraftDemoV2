using CraftDemoV2.API.RequestModels.FreshDeskModels;
using CraftDemoV2.API.ResponseModels.FreshDeskModels;
using CraftDemoV2.API.ResponseModels.GitHubModels.Users;

namespace CraftDemoV2.Services.APIServices.FreshDeskServices.Interfaces;

public interface IFreshDeskApiService
{

    public Task CreateContact(HttpClient client, string contactInfo);

    public Task<FreshDeskResponseContactModel> GetContact (HttpClient client, string contactId);

    public Task UpdateContact(HttpClient client, string contactInfo,string contactId);
}