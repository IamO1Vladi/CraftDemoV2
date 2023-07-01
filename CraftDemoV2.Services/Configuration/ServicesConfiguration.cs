using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.Data;
using CraftDemoV2.Services.APIServices.FreshDeskServices;
using CraftDemoV2.Services.APIServices.FreshDeskServices.Interfaces;
using CraftDemoV2.Services.APIServices.GitHubServices;
using CraftDemoV2.Services.APIServices.GitHubServices.Interfaces;
using CraftDemoV2.Services.BusinessServices;
using CraftDemoV2.Services.BusinessServices.Interfaces;
using CraftDemoV2.Services.DataBaseServices.GitHubUsersDataBase;
using CraftDemoV2.Services.DataBaseServices.GitHubUsersDataBase.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CraftDemoV2.Services.Configuration
{
    public static class ServicesConfiguration
    {

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMainService, MainService>();
            services.AddScoped<IGitHubDataBaseService, GitHubDataBaseService>();
            services.AddDbContext<CraftDemoV2DbContext>();
            services.AddScoped<IFreshDeskApiService, FreshDeskApiService>();
            services.AddScoped<IGitHubApiService,GitHubApiService>();

        }

    }
}
