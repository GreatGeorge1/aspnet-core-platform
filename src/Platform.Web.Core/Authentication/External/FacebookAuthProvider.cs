using System;
using System.Net.Http;
using System.Threading.Tasks;
using Abp.UI;
using Newtonsoft.Json;
using Platform.Models.Facebook;

namespace Platform.Authentication.External
{
    public class FacebookAuthProvider: ExternalAuthProviderApiBase
    {
        private static readonly HttpClient Client = new HttpClient();
        public const string Name = "Facebook";
        public override async Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
        {
            //gen app access token
            var appAccessTokenResponse = await Client.GetStringAsync("https://graph.facebook.com/oauth/access_token" +
              "?client_id=" + ProviderInfo.ClientId +
              "&client_secret=" + ProviderInfo.ClientSecret +
              "&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            //validate user access token
            var token = appAccessToken.AccessToken;
            var userAccessTokenValidationResponse = await Client.GetStringAsync("https://graph.facebook.com/v3.3/debug_token" +
                "?input_token="+ accessCode +
                "&access_token="+ appAccessToken.AccessToken);
            //var userAccessTokenValidationResponse = await Client.GetStringAsync("https://graph.facebook.com/v3.3/debug_token?access_token=EAAGbWQgDCZCABAKOKmlLRAdR3ryZCA6mALfhM3OrvY4L5s8iK1bZCQwDvSZC28znN5IdYMiglSTmgV0FTVLkWP5j8zTS4TaY7wbyGoo1XchXqEwVrDADn4LNMImeZBqbkPKoXnBzeQmPMEGJcU7zc28vfqZB3jZAtylLStVq05pO2K1DXQNXKlBO6A53HhfnlH8Yekl2xLNIYjZAoNCaTCboqegHPHy7HZAlcmeXfpwl1SP8QaagfuPgS&acces_token=452281665522672|RgsEKeMOqWp72sNdiC2H615Jcvs&input_token=EAAGbWQgDCZCABAOMHp7BLrcACt4YYYn6ZCZA3nvhBHMHkZCSY06RdDgtToh8ZChs6FMiKCSoDU3g9eLPdL6Wd0O1Od5suig7sOE7dZBeludllCRWR0ZCIAJX86TWd2148aA5ZAMXxsCRZC06lzIoTSsO9NZAdn5w0rVBqmoUipUuiASxB2wCpUvGGOWaKKi4KUSKrAJq0dPxY7IQZDZD");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);
            if (!userAccessTokenValidation.Data.IsValid)
            {
                throw new UserFriendlyException("login_failure Invalid facebook token.");
            }

            //get userinfo
            var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v3.3/me?fields=id,email,first_name,last_name&access_token={accessCode}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);
            if (string.IsNullOrEmpty(userInfo.Email))
            {
                userInfo.Email = $"facebook@{userInfo.Id}";
            }

            var name = $"{userInfo.FirstName} {userInfo.LastName}";
            
            return new ExternalAuthUserInfo
            {
                Name = name,
                EmailAddress = userInfo.Email,
                Surname=userInfo.LastName,
                Provider=Name,
                ProviderKey=userInfo.Id.ToString()
            };

        }
    }
}