using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchJesusBot.Interfaces
{
    public interface ICredentials
    {
        string StartupToken { get; set; }
        string RefreshToken { get; set; }
        int TokenTTL { get; set; }
    }
}
