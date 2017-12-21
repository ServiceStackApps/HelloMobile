using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceModel;
using ServiceStack;
using ServiceStack.Text;
using Shared.Client;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Client.XamarinForms
{
	public partial class MainPage : ContentPage
	{
	    private static string BaseUrl = Device.RuntimePlatform == Device.Android ? 
            Config.UseAndroidLoopback 
            : Device.RuntimePlatform == Device.iOS ? 
            Config.UseNetworkIp : 
            Config.BaseUrl;

        public IServiceClient CreateClient() => new JsonServiceClient(BaseUrl);

        public MainPage()
		{
			InitializeComponent();

            btnSync.Clicked += delegate
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
            };

            btnAsync.Clicked += async delegate
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
            };

            btnGateway.Clicked += async delegate
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
            };

            btnAuth.Clicked += async delegate
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
                    Console.WriteLine(ex);
                    lblResults.Text = ex.ToString();
                }
            };

            btnJwt.Clicked += async delegate
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
            };

            btnEncrypted.Clicked += async delegate
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
            };
        }
	}
}
