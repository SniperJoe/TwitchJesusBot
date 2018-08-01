using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchJesusBot.Interfaces;

namespace TwitchJesusBot
{
    internal class Credentials : ICredentials
    {
        public string StartupToken { get; set; }
        public string RefreshToken { get; set; }
        public int TokenTTL { get; set; }
    }
}
