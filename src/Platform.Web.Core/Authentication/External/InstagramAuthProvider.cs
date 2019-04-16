using Newtonsoft.Json;
using Platform.Models.Facebook;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Authentication.External
{
    public class InstagramAuthProvider : ExternalAuthProviderApiBase
    {
        private static readonly HttpClient Client = new HttpClient();
        public const string Name = "Instagram";
        private static readonly List<string> redirects = new List<string> { "https://localhost:9090/", "https://localhost:9090" };
        public override async Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
        {
            string userAccessTokenResponse =await  GetUserToken(accessCode);
            if(String.IsNullOrEmpty(userAccessTokenResponse))
            {
                throw new ArgumentNullException("userAccessTokenResponse is null or empty");
            }
            var userAccessToken = JsonConvert.DeserializeObject<InstagramUserAccessToken>(userAccessTokenResponse);
            var str = userAccessToken.User.FullName.Trim().Split(" ", 2);

            var userInfo = userAccessToken.User;
            if (str.Length > 0)
            {
                userInfo.FirstName = str[0];
                if(str.Length > 1)
                {
                    userInfo.LastName = str[1];
                }
            }
           
            return new ExternalAuthUserInfo
            {
                Name = userInfo.FirstName,
                EmailAddress = userInfo.UserName,
                Surname = userInfo.LastName,
                Provider = InstagramAuthProvider.Name,
                ProviderKey = userInfo.Id.ToString()
            };

        }

        private async Task<string> GetUserToken(string accessCode)
        {
                    var multipartFormDataContent = new MultipartFormDataContent();
                    var values = new[]
                    {
                        new KeyValuePair<string, string>("client_id", ProviderInfo.ClientId),
                        new KeyValuePair<string, string>("client_secret", ProviderInfo.ClientSecret),
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                        new KeyValuePair<string, string>("redirect_uri", "https://localhost:9090/"),
                        new KeyValuePair<string, string>("code", accessCode),
                        new KeyValuePair<string, string>("hl", "en")
                         //other values
                    };
                    foreach (var keyValuePair in values)
                    {
                        multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                            String.Format("\"{0}\"", keyValuePair.Key));
                    }
                    var res = Client.PostAsync("https://api.instagram.com/oauth/access_token", multipartFormDataContent).Result;
                    var r = res.Content.ReadAsStringAsync().Result;
                    return r;
      
        }
    }
}
