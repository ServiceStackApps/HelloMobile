using Foundation;
using System;
using ServiceModel;
using ServiceStack;
using Shared.Client;
using UIKit;

namespace Client.iOS
{
    public partial class Client_iOS_ViewController : UIViewController
    {
        public Client_iOS_ViewController (IntPtr handle) : base (handle)
        {
        }

        private const string BaseUrl = "http://10.0.0.33:2000/"; //TODO: replace with your IP Address
        public IServiceClient CreateClient() => new JsonServiceClient(BaseUrl);

        partial void BtnSync_TouchUpInside(UIButton sender)
        {
            try
            {
                var client = CreateClient();
                var response = client.Get(new Hello { Name = "Sync" });
                lblResults.Text = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.Text = ex.ToString();
            }
        }

        async partial void BtnAsync_TouchUpInside(UIButton sender)
        {
            try
            {
                var client = CreateClient();
                var response = await client.GetAsync(new Hello { Name = "Async" });
                lblResults.Text = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.Text = ex.ToString();
            }
        }

        async partial void BtnGateway_TouchUpInside(UIButton sender)
        {
            try
            {
                var gateway = new SharedGateway(BaseUrl);
                var greeting = await gateway.SayHello("Gateway");
                lblResults.Text = greeting;
            }
            catch (Exception ex)
            {
                lblResults.Text = ex.ToString();
            }
        }

        async partial void BtnAuth_TouchUpInside(UIButton sender)
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

                lblResults.Text = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.Text = ex.ToString();
            }
        }

        async partial void BtnJwt_TouchUpInside(UIButton sender)
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

                lblResults.Text = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.Text = ex.ToString();
            }
        }

        async partial void BtnEncypted_TouchUpInside(UIButton sender)
        {
            try
            {
                var client = (IJsonServiceClient)CreateClient();
                var encryptedClient = client.GetEncryptedClient(Config.PublicKeyXml);

                var response = await encryptedClient.SendAsync(new Hello { Name = "Encrypted Client" });

                lblResults.Text = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.Text = ex.ToString();
            }
        }
    }
}