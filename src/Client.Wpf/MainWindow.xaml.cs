using System;
using System.Windows;
using ServiceModel;
using ServiceStack;
using Shared.Client;

namespace Client.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
