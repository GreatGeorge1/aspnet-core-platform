using System;
using Newtonsoft.Json;

namespace Platform.Models.Google
{
    public class User
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        public string OauthSubject { get; set; }
       // public string
    }
}