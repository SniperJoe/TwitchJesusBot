using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TwitchJesusBot
{
    public static class TwitchClientExtensions 
    {
        public static async Task ConnectAsync(this TwitchClient client)
        {
            await Task.Run(() =>
            {
                var connected = false;
                var connectedHandler = new EventHandler<OnConnectedArgs>((sender, args) => {
                    connected = true;
                });
                client.OnConnected += connectedHandler;
                client.Connect();
                while (!connected)
                    ;
                client.OnConnected -= connectedHandler;
            });
        }

        public static async Task DisconnectAsync(this TwitchClient client)
        {
            await Task.Run(() =>
            {
                var connected = true;
                var disconnectedHandler = new EventHandler<OnDisconnectedArgs>((sender, args) => {
                    connected = false;
                });
                client.OnDisconnected+= disconnectedHandler;
                client.Disconnect();
                while (connected)
                    ;
                client.OnDisconnected -= disconnectedHandler;
            });
        }

        public static async Task JoinChannelAsync(this TwitchClient client, string channel)
        {
            if (client.JoinedChannels.All(ch => ch.Channel != channel))
            {
                await Task.Run(() =>
                {
                    var joined = false;
                    var joinedHandler = new EventHandler<OnJoinedChannelArgs>((sender, args) => {
                        if (args.Channel == channel)
                        {
                            joined = true;
                        }
                    });
                    client.OnJoinedChannel += joinedHandler;
                    client.JoinChannel(channel);
                    while (!joined)
                        ;
                    client.OnJoinedChannel -= joinedHandler;
                });
            }
        }

        private static async Task WaitConnected(TwitchClient client)
        {
            await Task.Run(() =>
            {
                while (!client.IsConnected)
                    ;
            });
        }
    }
}
