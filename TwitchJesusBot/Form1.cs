using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TwitchJesusBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void Init(string authToken, string refreshToken, int tokenTTL)
        {
            _commandsHandler = new CommandsHandler(authToken, refreshToken, tokenTTL);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
                Random rnd = new Random();
                var s = rnd.Next(1, 10);
                string message = "";
                for (int i = 1; i <= s; i++)
                    message += "nukeDog ";
                _commandsHandler.SendMessages(message, ConfigurationManager.AppSettings.Get("default_channel"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _commandsHandler.SendMessages("guit00 guit1 guit2", ConfigurationManager.AppSettings.Get("default_channel"));
        }

        private CommandsHandler _commandsHandler;
    }
}
