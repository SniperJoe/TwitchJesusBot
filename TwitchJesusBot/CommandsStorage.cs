using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchJesusBot
{
    class CommandsStorage
    {
        public Dictionary<string, string> GetCommands()
        {
            var commandsFilePath = ConfigurationManager.AppSettings.Get("CommandsFile");
            var commands = new Dictionary<string, string>();
            if (commandsFilePath != null)
            {
                var commandsLines = System.IO.File.ReadAllLines(commandsFilePath);
                foreach (var line in commandsLines)
                {
                    var commandAndReaction = line.Split(',');
                    commands[commandAndReaction[0]] = commandAndReaction[1];
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
    }
}
