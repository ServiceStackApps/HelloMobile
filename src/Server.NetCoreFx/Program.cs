using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Funq;
using ServiceModel;
using ServiceStack;

namespace Server.NetCoreFx
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base(nameof(Server.NetCoreFx), typeof(WebServices).Assembly) { }
        public override void Configure(Container container) => SharedAppHost.Configure(this);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? Config.ListeningOn)
                .Build();

            host.Run();
        }
    }

    public class Startup
    {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseServiceStack(new AppHost());
        }
    }
}
