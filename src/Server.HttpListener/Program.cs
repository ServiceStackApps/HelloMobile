using System;
using Funq;
using ServiceModel;
using ServiceStack;
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

            Routes.AddFromAssembly(typeof(WebServices).Assembly);
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
