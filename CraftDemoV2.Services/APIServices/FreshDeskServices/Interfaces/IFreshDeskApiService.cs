using CraftDemoV2.API.ResponseModels.GitHubModels.Users;

namespace CraftDemoV2.Services.APIServices.FreshDeskServices.Interfaces;

public interface IFreshDeskApiService
{

    public Task CreateContact(HttpClient client, string contactInfo);

}