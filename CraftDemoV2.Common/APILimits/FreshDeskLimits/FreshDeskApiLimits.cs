using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftDemoV2.Common.APILimits.FreshDeskLimits
{
    //Here you can add or edit the current limits placed on the API request from client side
    public static class FreshDeskApiLimits
    {

        public const int MaxRetryAttempts = 10;
        public const int RetryDelayMilliseconds = 500;

    }
}
