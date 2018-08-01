using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;

namespace TwitchJesusBot
{
    class CommandsHandler
    {
        public CommandsHandler(string token, string tokenRefresh, int tokenTTL)
        {
            this._commandsStorage = new CommandsStorage();
            this._commands = _commandsStorage.GetCommands();
            this._events = _commandsStorage.GetEventReactions();
            this._clientFactory = new TwitchClientFactory(token, tokenRefresh, tokenTTL);
            InitPrimaryClient();
        }

        private async void InitPrimaryClient()
        {
            this._primaryClient = (await _clientFactory.GetClientsAsync(1))[0];
            var channels = ConfigurationManager.AppSettings.Get("channels");
            if (channels != null)
            {
                var channelsList = channels.Split(',');
                foreach (var channel in channelsList)
                {
                    await _primaryClient.JoinChannelAsync(channel);
                }
            }
            this._primaryClient.OnMessageReceived += (sender, args) =>
            {
                if (_commands.ContainsKey(args.ChatMessage.Message))
                {
                    SendMessages(_commands[args.ChatMessage.Message], args.ChatMessage.Channel);
                }
            };
            foreach (var channelEventsPair in _events)
            {
                foreach (var eventReactionPair in channelEventsPair.Value)
                {
                    SetHandler(
                        _primaryClient, 
                        (o, args) => { _primaryClient.SendMessage(channelEventsPair.Key, eventReactionPair.Value); }, 
                        eventReactionPair.Key);
                }
            }
        }

        public async void SendMessages(string messages, string channel)
        {
            var messagesList = messages.Split(';');
            var clients = await _clientFactory.GetClientsAsync(messagesList.Length);
            for (int i = 0; i < messagesList.Length; i++)
            {
                await clients[i].JoinChannelAsync(channel);
            }
            for (int  i = 0; i< messagesList.Length; i++)
            {
                clients[i].SendMessage(channel, messagesList[i]);
            }
        }

        private void SetHandler(TwitchClient client, Action<object, EventArgs> handler, string eventName)
        {
            var eventInfo = typeof(TwitchClient).GetEvent(eventName);
            if (eventInfo != null)
            {
                eventInfo.AddEventHandler(client, handler);
            }
        }

        private CommandsStorage _commandsStorage;
        private Dictionary<string, string> _commands;
        private Dictionary<string, Dictionary<string, string>> _events;
        private readonly TwitchClientFactory _clientFactory;
        private TwitchClient _primaryClient;
    }
}
