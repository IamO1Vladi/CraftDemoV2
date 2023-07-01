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
