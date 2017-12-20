using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ServiceModel;
using ServiceStack;
using Shared.Client;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Client.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private const string BaseUrl = Config.BaseUrl;
        public IServiceClient CreateClient() => new JsonServiceClient(BaseUrl);

        private void btnSync_Click(object sender, RoutedEventArgs e)
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

        private async void btnAsync_Click(object sender, RoutedEventArgs e)
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

        private async void btnGateway_Click(object sender, RoutedEventArgs e)
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

        private async void btnAuth_Click(object sender, RoutedEventArgs e)
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

        private async void btnJwt_Click(object sender, RoutedEventArgs e)
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

        private async void btnEncrypted_Click(object sender, RoutedEventArgs e)
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
