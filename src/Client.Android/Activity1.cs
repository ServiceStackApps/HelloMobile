using System;
using Android.App;
using Android.Widget;
using Android.OS;
using ServiceModel;
using ServiceStack;
using Shared.Client;

namespace Client.Android
{
    [Activity(Label = "Client.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        private const string BaseUrl = Config.UseAndroidLoopback;
        public IServiceClient CreateClient() => new JsonServiceClient(BaseUrl);

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var btnSync = FindViewById<Button>(Resource.Id.btnSync);
            var btnAsync = FindViewById<Button>(Resource.Id.btnAsync);
            var btnGateway = FindViewById<Button>(Resource.Id.btnGateway);
            var btnAuth = FindViewById<Button>(Resource.Id.btnAuth);
            var btnJwt = FindViewById<Button>(Resource.Id.btnJwt);
            var btnEncrypted = FindViewById<Button>(Resource.Id.btnEncrypted);
            var lblResults = FindViewById<TextView>(Resource.Id.lblResults);

            btnSync.Click += delegate
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

            btnAsync.Click += async delegate
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

            btnGateway.Click += async delegate
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

            btnAuth.Click += async delegate
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
            };

            btnJwt.Click += async delegate
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

            btnEncrypted.Click += async delegate
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

