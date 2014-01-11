using System;
using System.Windows;
using ServiceModel;
using ServiceStack;

namespace Client.Wpf.Pcl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly JsonServiceClient client;

        public MainWindow()
        {
            InitializeComponent();
            //Net40PclExportClient.Configure();
            client = new JsonServiceClient("http://localhost:81/");
        }

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = client.Get(new Hello { Name = txtName.Text });
                lblResults.Text = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.Text = ex.ToString();
            }
        }

        private void btnAsync_Click(object sender, RoutedEventArgs e)
        {
            client.GetAsync(new Hello { Name = txtName.Text })
                .Success(r => lblResults.Text = r.Result)
                .Error(ex => lblResults.Text = ex.ToString());
        }

        private async void btnAwait_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await client.GetAsync(new Hello { Name = txtName.Text });
                lblResults.Text = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.Text = ex.ToString();
            }
        }
    }
}
