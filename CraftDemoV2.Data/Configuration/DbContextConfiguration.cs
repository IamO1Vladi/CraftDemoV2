﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftDemoV2.Data.Configuration
{
    //Here you can add or edit the configuration used for the dbcontext of the database
    public static class DbContextConfiguration
    {

        public const string ConnectionString= "Server=DESKTOP-7UJVTEO\\SQLEXPRESS;Database=CraftDemoV2;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;";

    }
}
