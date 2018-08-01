using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchJesusBot.CommandsStorage;
using TwitchJesusBot.Interfaces;
using TwitchJesusBot.Models;

namespace TwitchJesusBot
{
    class CommandsHandler : ICommandsHandler
    {
        public CommandsHandler(ICommandsStorage commandsStorage, IClientFactory clientFactory)
        {
            this._commandsStorage = commandsStorage;
            this._commands = _commandsStorage.GetCommands();
            this._clientFactory = clientFactory;
            InitPrimaryClient();
        }

        private async void InitPrimaryClient()
        {
            this._primaryClient = (await _clientFactory.GetClients(1))[0];
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
                var commandAndReactions = _commands.FirstOrDefault(c => c.Key == args.ChatMessage.Message);
                if (commandAndReactions != null)
                {
                    SendMessages(commandAndReactions.Reactions, args.ChatMessage.Channel);
                }
            };
            /*foreach (var channelEventsPair in _events)
            {
                foreach (var eventReactionPair in channelEventsPair.Value)
                {
                    SetHandler(
                        _primaryClient, 
                        (o, args) => { _primaryClient.SendMessage(channelEventsPair.Key, eventReactionPair.Value); }, 
                        eventReactionPair.Key);
                }
            }*/
        }

        public async void SendMessages(string[] messagesList, string channel)
        {
            var clients = await _clientFactory.GetClients(messagesList.Length);
            for (int i = 0; i < messagesList.Length; i++)
            {
                await clients[i].JoinChannelAsync(channel);
            }
            for (int  i = 0; i< messagesList.Length; i++)
            {
                await clients[i].SendMessageAsync(channel, messagesList[i]);
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

        private ICommandsStorage _commandsStorage;
        private IEnumerable<Command> _commands;
        private readonly IClientFactory _clientFactory;
        private TwitchClient _primaryClient;
    }
}
