using System;
using System.IO;
using System.Net;
using System.Threading;
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
            client = new JsonServiceClient("http://localhost:2000/");
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
            //Make all access to UI components in UI Thread, i.e. before entering bg thread.
            var name = txtName.Text;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    var client = new JsonServiceClient("http://localhost:2000/")
                    {
                        //this tries to access UI component which is invalid in bg thread
                        ShareCookiesWithBrowser = false
                    };
                    var fileStream = new MemoryStream("content body".ToUtf8Bytes());
                    var response = client.PostFileWithRequest<SendFileResponse>(
                        fileStream, "file.txt", new SendFile { Name = name });

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        lblResults.Content = "File Size: {0} bytes".Fmt(response.FileSize);
                    });
                }
                catch (Exception ex)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        lblResults.Content = ex.ToString();
                    });
                }
            });
        }
    }
}
