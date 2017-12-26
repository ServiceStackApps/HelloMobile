using System;
using AppKit;
using Foundation;

using ServiceModel;
using Shared.Client;
using ServiceStack;

namespace Client.OSX
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        private const string BaseUrl = Config.UseNetworkIp;
        public IServiceClient CreateClient() => new JsonServiceClient(BaseUrl);

        partial void btnSync_Click(Foundation.NSObject sender)
        {
            try
            {
                var client = CreateClient();
                var response = client.Get(new Hello { Name = "Sync" });
                lblResults.StringValue = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.StringValue = ex.ToString();
            }
        }

        async partial void btnAsync_Click(NSObject sender)
        {
            try
            {
                var client = CreateClient();
                var response = await client.GetAsync(new Hello { Name = "Async" });
                lblResults.StringValue = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.StringValue = ex.ToString();
            }
        }

        async partial void btnGateway_Click(NSObject sender)
        {
            try
            {
                var gateway = new SharedGateway(BaseUrl);
                var greeting = await gateway.SayHello("Gateway");
                lblResults.StringValue = greeting;
            }
            catch (Exception ex)
            {
                lblResults.StringValue = ex.ToString();
            }
        }

        async partial void btnAuth_Click(NSObject sender)
        {
            try
            {
                var client = CreateClient();
                await client.PostAsync(new Authenticate
                {
                    provider = "credentials",
                    UserName = "user",
                    Password = "pass",
                });

                var response = await client.GetAsync(new HelloAuth { Name = "Auth" });

                lblResults.StringValue = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.StringValue = ex.ToString();
            }
        }

        async partial void btnJwt_Click(NSObject sender)
        {
            try
            {
                var authClient = CreateClient();
                var authResponse = await authClient.PostAsync(new Authenticate
                {
                    provider = "credentials",
                    UserName = "user",
                    Password = "pass",
                });

                var client = new JsonServiceClient(BaseUrl)
                {
                    BearerToken = authResponse.BearerToken //JWT
                };

                var response = await client.GetAsync(new HelloAuth { Name = "JWT Auth" });

                lblResults.StringValue = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.StringValue = ex.ToString();
            }
        }

        async partial void btnEncrypted_Click(NSObject sender)
        {
            try
            {
                var client = (IJsonServiceClient)CreateClient();
                var encryptedClient = client.GetEncryptedClient(Config.PublicKeyXml);

                var response = await encryptedClient.SendAsync(new Hello { Name = "Encrypted Client" });

                lblResults.StringValue = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.StringValue = ex.ToString();
            }
        }
    }
}
