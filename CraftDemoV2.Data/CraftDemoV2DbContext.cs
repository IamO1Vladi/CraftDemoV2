using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraftDemoV2.Data.Models.GitHubModels;
using Microsoft.EntityFrameworkCore;
using static CraftDemoV2.Data.Configuration.DbContextConfiguration;

namespace CraftDemoV2.Data
{
    //Here is the main information about the database. We have set it up to use a sqlserver with the connection string coming from the configuration class.
    //You can add new tables or edit relationships with FluentAPI
    public class CraftDemoV2DbContext : DbContext 
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(ConnectionString);
        }

        public DbSet<GitHubUser> GitHubUsersAddedAsContacts { get; set; } = null!;
    }
}
