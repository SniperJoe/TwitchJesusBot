using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchJesusBot.Models;

namespace TwitchJesusBot.Interfaces
{
    public interface ICommandsStorage
    {
        IEnumerable<Command> GetCommands();
        void SaveCommandsFromFile(string fileName);
    }
}
