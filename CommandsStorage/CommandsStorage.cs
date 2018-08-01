using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchJesusBot.Interfaces;
using TwitchJesusBot.Models;

namespace TwitchJesusBot.CommandsStorage
{
    public class CommandsGetter : ICommandsStorage
    {
        public IEnumerable<Command> GetCommands()
        {
            var commandsFilePath = ConfigurationManager.AppSettings.Get("CommandsFile");
            //return GetCommandsFromDB();
            return GetCommandsFromFile(commandsFilePath);
        }

        private IEnumerable<Command> GetCommandsFromFile(string commandsFilePath)
        {
            var commands = new List<Command>();
            if (commandsFilePath != null)
            {
                var commandsLines = System.IO.File.ReadAllLines(commandsFilePath);
                foreach (var line in commandsLines)
                {
                    var commandAndReaction = line.Split(',');
                    commands.Add(new Command
                    {
                        Key = commandAndReaction[0],
                        Reactions = commandAndReaction[1].Split(';')
                    });
                }
            }
            return commands;
        }

        public Dictionary<string, Dictionary<string, string>> GetEventReactions()
        {
            var eventsFilePath = ConfigurationManager.AppSettings.Get("EventsFile");
            var events = new Dictionary<string, Dictionary<string, string>>();
            if (eventsFilePath != null)
            {
                var eventsLines = System.IO.File.ReadAllLines(eventsFilePath);
                foreach (var line in eventsLines)
                {
                    var channelEventReaction = line.Split(',');
                    events[channelEventReaction[0]][channelEventReaction[1]] = channelEventReaction[2];
                }
            }
            return events;
        }

        public void SaveCommandsFromFile(string fileName)
        {
            var commands = GetCommandsFromFile(fileName);
            SaveCommandsToDB(commands);
        }

        private void SaveCommandsToDB(IEnumerable<Command> commands)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Command> GetCommandsFromDB()
        {
            throw new NotImplementedException();
        }
    }
}
