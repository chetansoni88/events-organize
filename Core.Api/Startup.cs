using Owin;
using System.Web.Http;

namespace Core.Api
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });

            app.UseWebApi(config);
        }
    }
}