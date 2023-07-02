using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftDemoV2.API.ResponseModels.FreshDeskModels
{
    //This model is used to convert the responses from the FreshDesk API to a object that can be used
    public class FreshDeskResponseContactModel
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

        [JsonProperty("id")]
        public string Id { get; set; }


    }
}
