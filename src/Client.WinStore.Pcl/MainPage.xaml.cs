using System;
using ServiceStack;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using ServiceModel;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Client.WinStore.Pcl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly JsonServiceClient client;

        public MainPage()
        {
            this.InitializeComponent();
            //WinStorePclExportClient.Configure();
            client = new JsonServiceClient("http://localhost:81/");
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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

        private async void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await client.PostAsync(new Authenticate
                {
                    provider = "credentials",
                    UserName = "user",
                    Password = "pass",
                });

                var response = await client.GetAsync(new HelloAuth { Name = "Secure " + txtName.Text });

                lblResults.Text = response.Result;
            }
            catch (Exception ex)
            {
                lblResults.Text = ex.ToString();
            }
        }
    }
}
