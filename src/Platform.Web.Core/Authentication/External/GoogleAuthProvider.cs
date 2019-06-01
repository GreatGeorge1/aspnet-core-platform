using System.Threading;
using System.Threading.Tasks;
using Google.Apis;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;

namespace Platform.Authentication.External
{
    public class GoogleAuthProvider:ExternalAuthProviderApiBase
    {
        public const string Name = "Google";
        /// <summary>
        /// The flow is as follows
        /// 1) We have the access code. We send it off to get a TokenResponse, which contains access and id tokens.
        /// 2) we use the id token and send it for validation, which will also give us back the user details.
        /// (as long as we have the profile scope)
        /// </summary>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        public override async Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
        {
            var flow = (new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets() { ClientId = this.ProviderInfo.ClientId, ClientSecret = this.ProviderInfo.ClientSecret },
                Scopes = new[] { "email profile openid" },
            }));


            //TokenResponse credential = await flow.ExchangeCodeForTokenAsync("user-id", accessCode, "postmessage", CancellationToken.None);
            //var idtokenpayload = await GoogleJsonWebSignature.ValidateAsync(credential.IdToken);
            var idtokenpayload = await GoogleJsonWebSignature.ValidateAsync(accessCode);
            var name = $"{idtokenpayload.GivenName} {idtokenpayload.FamilyName}";

            return new ExternalAuthUserInfo()
            {
                EmailAddress = idtokenpayload.Email,
                Name = idtokenpayload.Name,
                Provider = this.ProviderInfo.Name,
                ProviderKey = idtokenpayload.Subject,
            };

        }

    }
}