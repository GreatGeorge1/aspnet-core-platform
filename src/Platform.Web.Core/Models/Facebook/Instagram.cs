using Newtonsoft.Json;

namespace Platform.Models.Facebook
{
    internal class InstagramUserAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("user")]
        public InstagramUser User { get; set; }
    }

    internal class InstagramUser
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    

  

}
