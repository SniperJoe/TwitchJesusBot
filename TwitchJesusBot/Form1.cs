using SimpleInjector;
using System;
using System.Configuration;
using System.Windows.Forms;
using TwitchJesusBot.Interfaces;
using TwitchJesusBot.CommandsStorage;

namespace TwitchJesusBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _container = new Container();

        }

        public void Init(ICredentials credentials)
        {
            _container.RegisterInstance(credentials);
            _container.Register<ICommandsStorage, CommandsGetter>(Lifestyle.Singleton);
            var clientsFactoryRegistration = Lifestyle.Singleton.CreateRegistration<TwitchClientFactory>(_container);
            _container.AddRegistration<IClientFactory>(clientsFactoryRegistration);
            _container.AddRegistration<ITokenUpdateHandler>(clientsFactoryRegistration);
            _container.Register<ICommandsHandler, CommandsHandler>(Lifestyle.Singleton);
            _container.Register<TwitchTokenUpdater>(Lifestyle.Singleton);
            _container.Verify(VerificationOption.VerifyAndDiagnose);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
                Random rnd = new Random();
                var s = rnd.Next(1, 10);
                string message = "";
                for (int i = 1; i <= s; i++)
                    message += "nukeDog ";
            var commandsHandler = _container.GetInstance<ICommandsHandler>();
                commandsHandler.SendMessages(new[] { message }, ConfigurationManager.AppSettings.Get("default_channel"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var commandsHandler = _container.GetInstance<ICommandsHandler>();
            commandsHandler.SendMessages(new[] { "guit00 guit1 guit2" }, ConfigurationManager.AppSettings.Get("default_channel"));
        }

        private Container _container;
    }
}
