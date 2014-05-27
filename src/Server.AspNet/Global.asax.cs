using System;
using System.Collections.Generic;
using Funq;
using ServiceModel;
using ServiceStack;
using ServiceStack.Auth;

namespace Server.AspNet
{
    public class AppHost : AppHostBase
    {
        public AppHost()
            : base("Hello ASP.NET Server", typeof(WebServices).Assembly) { }

        public override void Configure(Container container)
        {
            Plugins.Add(new CorsFeature());

            Plugins.Add(new AuthFeature(() => new AuthUserSession(), 
                new IAuthProvider[] {
                    new CustomCredentialsAuthProvider(), 
                }));

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

    [Authenticate]
    public class AdminServices : Service
    {
        public object Any(HelloAuth request)
        {
            var response = new HelloResponse { Result = "Hello, " + request.Name };
            return response;
        }
    }

    public class CustomCredentialsAuthProvider : CredentialsAuthProvider
    {
        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            return userName == "user" && password == "pass";
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