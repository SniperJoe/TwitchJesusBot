using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json.Linq;
using TwitchLib.Client.Models;

namespace TwitchJesusBot
{
    class TwitchTokenUpdater
    {
        public TwitchTokenUpdater(string refreshToken, int updateFrequency, Action<string> applyNewToken)
        {
            _refreshToken = refreshToken;
            _newTokenApplier = applyNewToken;
            _timer = new Timer {Interval = 100000};
            _timer.Elapsed += UpdateTokenAsync;
            _timer.Enabled = true;
            _timer.Start();
        }

        private async void UpdateTokenAsync(object sender, EventArgs e)
        {
            var clientId = ConfigurationManager.AppSettings.Get("client_id");
            var baseUri = ConfigurationManager.AppSettings.Get("token_uri");
            var secret = ConfigurationManager.AppSettings.Get("secret");
            HttpClient httpClient = new HttpClient();
            var authBodyDictionary = new Dictionary<string, string>
            {
                ["client_id"] = clientId,
                ["client_secret"] = secret,
                ["refresh_token"] = _refreshToken,
                ["grant_type"] = "refresh_token",
            };
            var authBody = new FormUrlEncodedContent(authBodyDictionary);
            var authResponse = await httpClient.PostAsync(baseUri, authBody);
            var authResponseObject = JObject.Parse(await authResponse.Content.ReadAsStringAsync());
            var newRefreshToken = authResponseObject["refresh_token"]?.ToString();
            var accessToken = authResponseObject["access_token"]?.ToString();
            if (newRefreshToken != null && accessToken != null)
            {
                _refreshToken = newRefreshToken;
                _newTokenApplier(accessToken);
            }
        }

        private string _refreshToken;
        private readonly Timer _timer;
        private readonly Action<string> _newTokenApplier;
    }
}
