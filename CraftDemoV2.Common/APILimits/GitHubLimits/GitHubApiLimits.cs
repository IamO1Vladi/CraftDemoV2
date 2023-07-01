using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftDemoV2.Common.APILimits.GitHubLimits
{
    public static class GitHubApiLimits
    {

        public const int MaxRetryAttempts = 10;
        public const int RetryDelayMilliseconds = 500;

    }
}
