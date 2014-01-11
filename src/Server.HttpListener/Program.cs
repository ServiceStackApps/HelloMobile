using System;
using Funq;
using ServiceModel;
using ServiceStack;
using ServiceStack.Text;

namespace Server.HttpListener
{
    public class AppHost : AppHostHttpListenerBase
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
            return new HelloResponse { Result = "Hello, " + request.Name };
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
