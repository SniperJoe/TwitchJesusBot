using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TwitchJesusBot
{
    public class AuthController : ApiController
    {
        [Route("auth"), HttpGet]
        public async Task<string> Get([FromUri]string code)
        {
            HttpClient client = new HttpClient();
            var baseUri = ConfigurationManager.AppSettings.Get("token_uri");
            var clientId = ConfigurationManager.AppSettings.Get("client_id");
            var secret = ConfigurationManager.AppSettings.Get("secret");
            var redirectUri = ConfigurationManager.AppSettings.Get("redirect_uri");
            var authBodyDictionary = new Dictionary<string, string>
            {
                ["client_id"] = clientId,
                ["client_secret"] = secret,
                ["code"] = code,
                ["grant_type"] = "authorization_code",
                ["redirect_uri"] = redirectUri,
            };
            var authBody = new FormUrlEncodedContent(authBodyDictionary);
            var authResponse = await client.PostAsync(baseUri, authBody);
            var authResponseObject = JObject.Parse(await authResponse.Content.ReadAsStringAsync());
            var refreshToken = authResponseObject["refresh_token"]?.ToString();
            var accessToken = authResponseObject["access_token"]?.ToString();
            if (refreshToken != null && accessToken!=null && Int32.TryParse(authResponseObject["expires_in"]?.ToString(), out int tokenTTL))
            {
                var credentials = new Credentials
                {
                    StartupToken = accessToken,
                    RefreshToken = refreshToken,
                    TokenTTL = tokenTTL
                };
                Program.UpdateAuthToken(credentials);
                return "success";
            }
            return "fail";
        }

        public string _code;
    }
}
