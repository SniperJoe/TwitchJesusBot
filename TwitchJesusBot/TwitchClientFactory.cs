using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchJesusBot.Interfaces;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TwitchJesusBot
{
    class TwitchClientFactory : ITokenUpdateHandler, IClientFactory
    {
        public TwitchClientFactory(ICredentials credentials)
        {
            this._token = credentials.StartupToken;
            this._readyClients = new List<TwitchClient>();
        }

        private async Task<TwitchClient> InitClient()
        {
            var twitchUsername = ConfigurationManager.AppSettings.Get("username");
            ConnectionCredentials credentials = new ConnectionCredentials(twitchUsername, _token);

            var client = new TwitchClient();
            var defaultChannel = ConfigurationManager.AppSettings.Get("default_channel");
            client.Initialize(credentials, defaultChannel);
            await client.ConnectAsync();
            
            return client;
        }

        public async Task<List<TwitchClient>> GetClients(int clientCount)
        {
            while (clientCount > _readyClients.Count)
            {
                _readyClients.Add(await InitClient());
            }
            return _readyClients.GetRange(0, clientCount);
        }

        public async void UpdateToken(string newToken)
        {
            this._token = newToken;
            var twitchUsername = ConfigurationManager.AppSettings.Get("username");
            ConnectionCredentials credentials = new ConnectionCredentials(twitchUsername, _token);
            for (int i = 0; i < _readyClients.Count; i++)
            {
                var channelsWereJoined = _readyClients[i].JoinedChannels;
                await _readyClients[i].DisconnectAsync();
                _readyClients[i].SetConnectionCredentials(credentials);
                await _readyClients[i].ConnectAsync();
                foreach (var joinedChannel in channelsWereJoined)
                {
                    await _readyClients[i].JoinChannelAsync(joinedChannel.Channel);
                }
            }
        }

        private string _token;
        private readonly List<TwitchClient> _readyClients;
        private readonly TwitchTokenUpdater _tokenUpdater;
    }
}
