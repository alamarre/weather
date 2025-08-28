using System.Web.Http;
using Owin;

namespace WeatherApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            Configure(config);
            app.UseWebApi(config);
        }

        public static void Configure(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
    }
}
