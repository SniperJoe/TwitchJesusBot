using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TwitchJesusBot
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "Auth",
                routeTemplate: "{controller}/",
                defaults: new { controller = "Auth", access_token="dsfgfdsg"}
            );

            appBuilder.UseWebApi(config);
            RedirectUser();
        }

        private void RedirectUser()
        {
            var clientId = ConfigurationManager.AppSettings.Get("client_id");
            var redirectUri = ConfigurationManager.AppSettings.Get("redirect_uri");
            var scope = ConfigurationManager.AppSettings.Get("scope");
            var baseUri = ConfigurationManager.AppSettings.Get("auth_uri");
            var requestUri =
                $"{baseUri}?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope={scope}";
            System.Diagnostics.Process.Start(requestUri);
        }
    }
}
