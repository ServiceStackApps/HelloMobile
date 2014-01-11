using System;
using Funq;
using ServiceModel;
using ServiceStack;

namespace Server.AspNet
{
    public class AppHost : AppHostBase
    {
        public AppHost()
            : base("Hello ASP.NET Server", typeof(WebServices).Assembly) { }

        public override void Configure(Container container)
        {
            Plugins.Add(new CorsFeature());

            Routes.AddFromAssembly(typeof(WebServices).Assembly);
        }
    }

    public class WebServices : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }
    }


    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }
    }
}