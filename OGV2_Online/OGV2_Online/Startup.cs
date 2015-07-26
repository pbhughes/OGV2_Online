using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using OGV2_Online.Middleware;

[assembly: OwinStartup(typeof(OGV2_Online.Startup))]

namespace OGV2_Online
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            HttpConfiguration config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            appBuilder.Use<SimpleLogger>();

            appBuilder.UseWebApi(config); 
        }
    }
}
