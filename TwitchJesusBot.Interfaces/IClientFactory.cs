using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;

namespace TwitchJesusBot.Interfaces
{
    public interface IClientFactory
    {
        Task<List<TwitchClient>> GetClients(int amount);
    }
}
