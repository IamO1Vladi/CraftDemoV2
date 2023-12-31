﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CraftDemoV2.API.ResponseModels.GitHubModels.Users
{
    //This model is used to create an object from the response coming from the GitHubAPI
    public class GitHubGetUserModel
    {
        [JsonProperty("login")]
        public string UserName { get; set; } = null!;

        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("name")]
        public string? FullName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("location")]
        public string? Location { get; set; }

        [JsonProperty("bio")]
        public string? Bio { get; set; }

        [JsonProperty("avatar_url")]
        public string? AvatarUrl { get; set; }

        [JsonProperty("twitter_username")]
        public string? TwitterUserName { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

    }
}
