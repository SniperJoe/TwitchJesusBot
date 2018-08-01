using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Owin.Hosting;

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
            string baseAddress = "http://localhost:27003/";
            _startedApp = WebApp.Start<Startup>(url: baseAddress);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(_mainForm = new Form1());
        }

        public static void UpdateAuthToken(string authToken, string refreshToken, int tokenTTL)
        {
            _mainForm.Init(authToken, refreshToken, tokenTTL);
        }

        private static IDisposable _startedApp;
        private static Form1 _mainForm;

    }
}
