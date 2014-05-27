using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using ServiceModel;
using ServiceStack;
using ServiceStack.Text;

namespace Client.Sl5
{
    public partial class MainPage : UserControl
    {
        private readonly JsonServiceClient client;

        public MainPage()
        {
            InitializeComponent();
            client = new JsonServiceClient("http://localhost:81/");
        }

        private void btnAsync_Click(object sender, RoutedEventArgs e)
        {
            client.GetAsync(new Hello { Name = txtName.Text })
                .Success(r => lblResults.Content = r.Result)
                .Error(ex => lblResults.Content = ex.ToString());
        }

        private async void btnAwait_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await client.GetAsync(new Hello { Name = txtName.Text });
                lblResults.Content = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.Content = ex.ToString();
            }
        }

        private async void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await client.PostAsync(new Authenticate
                {
                    provider = "credentials",
                    UserName = "user",
                    Password = "pass"
                });

                var response = await client.GetAsync(new HelloAuth { Name = "Secure " + txtName.Text });

                lblResults.Content = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.Content = ex.ToString();
            }
        }

        private async void btnTest_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
