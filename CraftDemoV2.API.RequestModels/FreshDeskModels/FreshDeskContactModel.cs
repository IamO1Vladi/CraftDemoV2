using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CraftDemoV2.API.RequestModels.FreshDeskModels
{
    public class FreshDeskContactModel
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("twitter_id")]
        public string? TwitterId { get; set; }

        [JsonProperty("unique_external_id")]
        public string? UniqueExternalId { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

    }
}
