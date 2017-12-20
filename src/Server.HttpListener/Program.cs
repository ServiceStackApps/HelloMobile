using System;
using Funq;
using ServiceModel;
using ServiceStack;
using ServiceStack.Text;

namespace Server.HttpListener
{
    public class AppHost : AppSelfHostBase
    {
        public AppHost() : base(nameof(Server.HttpListener), typeof(WebServices).Assembly) {}
        public override void Configure(Container container) => SharedAppHost.Configure(this);
    }

    class Program
    {
        static void Main(string[] args)
        {
            new AppHost()
                .Init()
                .Start(Config.ListeningOn);

            Config.BaseUrl.Print();
            Console.ReadLine();
        }
    }
}
