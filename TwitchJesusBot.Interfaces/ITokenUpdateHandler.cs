﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchJesusBot.Interfaces
{
    public interface ITokenUpdateHandler
    {
        void UpdateToken(string newToken);
    }
}
