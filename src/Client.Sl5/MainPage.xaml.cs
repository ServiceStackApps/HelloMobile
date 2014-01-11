using System;
using System.Windows;
using System.Windows.Controls;
using ServiceModel;
using ServiceStack;

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

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
