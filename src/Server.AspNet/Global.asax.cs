using System;
using Funq;
using ServiceStack;

namespace Server.AspNet
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base(nameof(Server.AspNet), typeof(WebServices).Assembly) { }
        public override void Configure(Container container) => SharedAppHost.Configure(this);
    }

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }
    }
}