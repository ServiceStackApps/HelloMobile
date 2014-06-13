using System;
using Funq;
using ServiceModel;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Text;

namespace Server.HttpListener
{
    public class AppHost : AppSelfHostBase
    {
        public AppHost()
            : base("Hello HttpListener Server", typeof(WebServices).Assembly) { }

        public override void Configure(Container container)
        {
            Plugins.Add(new CorsFeature());

            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                new IAuthProvider[] {
                    new CustomCredentialsAuthProvider(), 
                }));

            Routes.AddFromAssembly(typeof(WebServices).Assembly);

            SetConfig(new HostConfig {
                DebugMode = true
            });
        }
    }

    public class WebServices : Service
    {
        public object Any(Hello request)
        {
            var response = new HelloResponse { Result = "Hello, " + request.Name };

            if (Request.Files.Length > 0)
            {
                response.Result += ".\nFiles: {0}, name: {1}, size: {2} bytes".Fmt(Request.Files.Length, Request.Files[0].FileName, Request.Files[0].ContentLength);
            }

            return response;
        }

        public object Any(UploadFile request)
        {
            var response = new UploadFileResponse {
                Name = request.Name,
            };

            if (base.Request.Files.Length > 0)
            {
                var file = base.Request.Files[0];
                response.FileSize = file.ContentLength;
            }

            return response;
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

    class Program
    {
        static void Main(string[] args)
        {
            new AppHost()
                .Init()
                .Start("http://*:81/");

            "http://localhost:81/".Print();
            Console.ReadLine();
        }
    }
}
