using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Owin.Hosting;
using TwitchJesusBot.Interfaces;

namespace TwitchJesusBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (WebApp.Start<Startup>(url: "http://localhost:27003/"))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(_mainForm = new Form1());
            };
        }

        public static void UpdateAuthToken(ICredentials credentials)
        {
            _mainForm.Init(credentials);
        }

        private static Form1 _mainForm;
    }
}
