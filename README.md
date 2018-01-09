ServiceStack Mobile and Desktop Apps
====================================

This projects contains several examples for consuming ServiceStack Typed Services from different native Mobile and Desktop Apps developed with .NET. Using C# to develop native Mobile and Desktop Apps provides a number of benefits including maximum reuse of your investments across multiple Client Apps where they're able to reuse shared functionality, libraries, knowledge, development workflow and environment in both Client and Server Apps.

This reusability is enhanced with ServiceStack which provides a highly productive development workflow which lets you reuse the same POCO DTOs used to define the Services contract with, in Clients Apps to provide a terse, end-to-end typed API without any additional custom build tools, code-gen or any other artificial machinery, by using just the DTOs in the shared `ServiceModel.dll` with any of the available highly performant [.NET generic Service Clients](http://docs.servicestack.net/csharp-client) which encourages development of [resilient message-based Services](http://docs.servicestack.net/what-is-a-message-based-web-service) for enabling [highly decoupled](http://docs.servicestack.net/service-gateway) and easily [substitutable and mockable](http://docs.servicestack.net/csharp-client#built-in-clients) Service Integrations.

## Mobile and Desktop Apps

![Supported Client Platforms](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/splash-900.png)

This project contains multiple versions of the same App demonstrating a number of different calling conventions, service integrations and reuse possibilities for each of the following platforms:

  - WPF
  - UWP
  - Xamarin.Android
  - Xamarin.iOS
  - Xamarin.OSX
  - Xamarin.Forms
    - iOS
    - Android
    - UWP

Whilst the event handler registrations in each App can vary, the source code used to call ServiceStack Services remains the same.

### Native SDKs and Development Environments

This project focuses solely on Mobile and Desktop Apps developed with C#, if you would prefer to utilize the native SDK's and development environment and language of each platform, you can instead use the [Add ServiceStack Reference](http://docs.servicestack.net/add-servicestack-reference) feature to generate Typed DTOs for calling ServiceStack Services in Android Apps using either [Java](http://docs.servicestack.net/java-add-servicestack-reference) and [Kotlin](http://docs.servicestack.net/kotlin-add-servicestack-reference), or use the [Swift](http://docs.servicestack.net/swift-add-servicestack-reference) support in development of native iOS or OSX Apps or [TypeScript](http://docs.servicestack.net/typescript-add-servicestack-reference) for calling Services in [React Native, Node.js or Web Apps](https://github.com/ServiceStackApps/typescript-server-events).

Each platform provides a similar development experience as C# using just the generated DTOs and the generic Service Client available in each platform which utilize the same method names and message-based API idiomatic to each platform to maximize knowledge reuse and simplify any porting efforts, if needed.

C# Clients can also use [C# Add ServiceStack Reference](http://docs.servicestack.net/csharp-add-servicestack-reference) to generate Typed DTOs as an alternative to sharing the Server DTOs in `ServiceModel.dll` to eliminate binary coupling. In either case the source code for both solutions remains exactly the same.

## Install ServiceStack Client

ServiceStack's Service Clients can be used in .NET v4.5+ or .NET Standard 2.0 platforms where it can be installed from the [ServiceStack.Client](https://www.nuget.org/packages/ServiceStack.Client) NuGet package: 

    PM> Install-Package ServiceStack.Client

Alternatively you can instead use `JsonHttpClient` from [ServiceStack.HttpClient](https://www.nuget.org/packages/ServiceStack.HttpClient):

    PM> Install-Package ServiceStack.HttpClient

Which as it's based on Microsoft's async HttpClient can be [configured to be used with ModernHttpClient](http://docs.servicestack.net/csharp-client#jsonhttpclient) to provide a thin wrapper around iOS's native `NSURLSession` or `OkHttp` client on Android, offering improved performance and stability for mobile connectivity. 

All `JsonHttpClient` instances can be configured to use ModernHttpClient's `NativeMessageHandler` with:

```csharp
JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler()
```

### [Cache Aware Service Clients](http://docs.servicestack.net/cache-aware-clients)

When [HTTP Caching is enabled on Services](http://docs.servicestack.net/http-caching), the "Cache-aware" Service Clients can dramatically improve performance by eliminating server requests entirely as well as reducing bandwidth for re-validated requests. They also offer an additional layer of resiliency as re-validated requests that result in Errors will transparently fallback to using pre-existing locally cached responses. For bandwidth-constrained environments like Mobile Apps they can dramatically improve the User Experience.

Cache-Aware clients implement the full `IServiceClient` API making them an easy drop-in enhancement for existing Apps:

```csharp
IServiceClient client = new JsonServiceClient(baseUrl).WithCache(); 

//Equivalent to:
IServiceClient client = new CachedServiceClient(new JsonServiceClient(baseUrl));
```

Likewise for the HttpClient-based `JsonHttpClient`:

```csharp
IServiceClient client = new JsonHttpClient(baseUrl).WithCache(); 

//Equivalent to:
IServiceClient client = new CachedHttpClient(new JsonHttpClient(baseUrl));
```

## ServiceStack Server App

Each App calls the same simple ServiceStack Server [WebServices.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Server.Common/WebServices.cs) implementation that thanks to ServiceStack's versatility, can be hosted on any of .NET's popular HTTP Server hosting configurations:

### [Server.NetCore](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Server.NetCore)

The AppHost for hosting the ServiceStack Services in a ASP.NET Core 2.0 App:

```csharp
public class AppHost : AppHostBase
{
    public AppHost() : base(nameof(Server.NetCore), typeof(WebServices).Assembly) { }
    public override void Configure(Container container) => SharedAppHost.Configure(this);
}
```

### [Server.NetCoreFx](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Server.NetCoreFx)

The same source code can be used to run a ServiceStack ASP.NET Core App on the **.NET Framework**:

```csharp
public class AppHost : AppHostBase
{
    public AppHost() : base(nameof(Server.NetCoreFx), typeof(WebServices).Assembly) { }
    public override void Configure(Container container) => SharedAppHost.Configure(this);
}
```

The difference between a **.NET Framework v4.7** and a **.NET Core 2.0** ASP.NET Core App is in [Server.NetCoreFx.csproj](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Server.NetCoreFx/Server.NetCoreFx.csproj) where it needs to reference **ServiceStack.Core** NuGet package to force using the **.NET Standard 2.0** version of ServiceStack which contains the support for hosting ASP.NET Core Apps.

### [Server.AspNet](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Server.AspNet)

The same source code is also used for hosting classic ASP.NET Web Applications:

```csharp
public class AppHost : AppHostBase
{
    public AppHost() : base(nameof(Server.AspNet), typeof(WebServices).Assembly) { }
    public override void Configure(Container container) => SharedAppHost.Configure(this);
}
```

The ASP.NET Host also needs to be configured to [use a wildcard mapping in IIS Express](https://stackoverflow.com/a/8569259/85785) to accommodate access from Mobile platforms that need to use a Remote IP to access the locally running ServiceStack Instance.

The difference between .NET Core is how ServiceStack's AppHost is initialized, where in ASP.NET it's initialized in 
[Global.asax](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Server.AspNet/Global.asax.cs):

```csharp
public class Global : System.Web.HttpApplication
{
    protected void Application_Start(object sender, EventArgs e)
    {
        new AppHost().Init();
    }
}
```

Whereas in ASP.NET Core Apps it's initialized in the `Startup` class in [Program.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Server.NetCore/Program.cs)

```csharp
public class Startup
{
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseServiceStack(new AppHost());
    }
}
```

### [Server.HttpListener](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Server.HttpListener)

To host in a .NET Framework Self-Hosting HttpListener App, the AppHost just needs to inherit from `AppSelfHostBase`:

```csharp
public class AppHost : AppSelfHostBase
{
    public AppHost() : base(nameof(Server.HttpListener), typeof(WebServices).Assembly) {}
    public override void Configure(Container container) => SharedAppHost.Configure(this);
}
```

### Shared ServiceStack Services

All AppHost's above are configured to run the same Web Services implementation below:

```csharp
public static class SharedAppHost
{
    public static void Configure(IAppHost appHost)
    {
        appHost.Config.DefaultRedirectPath = "/metadata";

        appHost.Plugins.Add(new CorsFeature());

        appHost.Plugins.Add(new AuthFeature(() => new AuthUserSession(),
            new IAuthProvider[] {
                new CustomCredentialsAuthProvider(),
                new JwtAuthProvider
                {
                    AuthKeyBase64 = Config.JwtAuthKeyBase64,
                    RequireSecureConnection = false,
                },
            }));

        appHost.Plugins.Add(new EncryptedMessagesFeature
        {
            PrivateKeyXml = Config.PrivateKeyXml
        });
    }

    public class CustomCredentialsAuthProvider : CredentialsAuthProvider
    {
        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            return userName == "user" && password == "pass";
        }
    }
}

public class WebServices : Service
{
    public object Any(Hello request) => new HelloResponse { Result = "Hello, " + request.Name };
}

[Authenticate]
public class AuthServices : Service
{
    public object Any(HelloAuth request)
    {
        return new HelloResponse { Result = "Hello, " + request.Name };
    }
}
```

That are all configured to listen on the same **http://localhost:5000** address.

### Run the Server App

You can run your preferred Host by selecting **Set as Startup project** on the project's right-click context menu, then running it with `Ctrl+F5`.

Alternatively for .NET Core Apps you can run it from the command-line, from the projects folder with:

    $ dotnet run

Whilst the Self-Hosting HttpListener Host can be run in a Console App in the background by double-clicking the compiled **Server.HttpListener.exe**.

After you've started your prefered host, check that it's up and running by going to: `http://localhost:5000` in a web browser.

## Client Apps

Once you have a server running you can run any of the included Client Apps by selecting it as the **StartUp Project** in the context menu and running it with `Ctrl+F5`.

Each of the Client Apps have the same functionality demonstrating a number of popular use-cases scenarios for accessing ServiceStack Services:

## WPF

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/wpf.png)

All Apps follow the same approach to call Services by sending a populated Request DTOs in the **ServiceModel.dll** that's sent using different APIs and features in ServiceStack Service Clients. 

Here is WPF's complete [MainWindow.xaml.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.Wpf/MainWindow.xaml.cs) implementation below:

```csharp
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
```

### Sync calls

The Sync API shows an example of the simplest Service Clients call you can perform, by sending a Request DTO and receiving a typed Response DTO:

```csharp
var client = CreateClient();
var response = client.Get(new Hello { Name = "Sync" });
lblResults.Text = response.Result;
```

In this example it sends a HTTP `GET` Request to the `/hello?Name=Sync` endpoint with the `Accept: application/json` HTTP Header so the server returns its Response in JSON which is deserialized into the `HelloResponse` DTO. 

> As `Get()` makes a synchronous I/O call you'll want to avoid invoking it from the main UI Thread so it doesn't block the UI.

## UWP

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/uwp.png)

UWP's [MainPage.xaml.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.UWP/MainPage.xaml.cs) source code is exactly the same as WPF's above, which even uses the same [XAML markup](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.UWP/MainPage.xaml) for its UI, copied from [WPF's XAML](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.Wpf/MainWindow.xaml):

```xml
<Grid Margin="20,20,0,100">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>
    <Grid.RowDefinitions>
        <RowDefinition Height="45" />
        <RowDefinition Height="*" />
    </Grid.RowDefinitions>

    <StackPanel Grid.Row="0" Orientation="Horizontal">
        <StackPanel.Resources>
            <Style TargetType="{x:Type Button}">
                <Setter Property="Margin" Value="0,0,10,0"/>
                <Setter Property="Padding" Value="10,0,10,0"/>
                <Setter Property="FontSize" Value="24"/>
            </Style>
        </StackPanel.Resources>
        <Button x:Name="btnSync" Content="Sync" Click="btnSync_Click" />
        <Button x:Name="btnAsync" Content="Async" Click="btnAsync_Click" />
        <Button x:Name="btnGateway" Content="Gateway" Click="btnGateway_Click" />
        <Button x:Name="btnAuth" Content="Auth" Click="btnAuth_Click" />
        <Button x:Name="btnJwt" Content="JWT" Click="btnJwt_Click" />
        <Button x:Name="btnEncrypted" Content="Encrypted" Click="btnEncrypted_Click" />
    </StackPanel>

    <TextBlock Grid.Row="1" x:Name="lblResults" FontSize="36" 
               HorizontalAlignment="Center" VerticalAlignment="Center" />

</Grid>
```

### Async APIs

UWP's screenshot shows an example of a non-blocking request that differs from the sync API with the addition of the `async` keyword in its method signature so it can use C#'s `await` feature when invoking the Service Clients `GetAsync()` API:

```csharp
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
```

All Service Clients have an equivalent Async for every Sync API that's differentiated by an `*Async` suffix.

## Xamarin.Android

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/android.png)

Developing for iOS or Android requires usage of Xamarin tooling which can be installed in VS.NET using its Visual Studio Installer. The integrated Xamarin Designers provides a familiar development experience for developing Android Apps as found in other .NET Windows Desktop Apps where you can drag UI elements from the toolbox and position them using the designer to construct the layout of your screen. But instead of generating XAML it generates UI's using [Android's XML Layout](https://developer.android.com/guide/topics/ui/declaring-layout.html). 

It follows a parallel development model to Java Android where instead of double-clicking the button to add a click event handler you'll need to resolve the button element in the loaded Activity Layout to register the click event handler, e.g:

```csharp
var btnGateway = FindViewById<Button>(Resource.Id.btnGateway);
...

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
```

Another difference is since the App is being run from Android's Emulator, it needs to use Android's `10.0.2.2` loopback address in order to call Services running on `localhost`:

```csharp
private const string BaseUrl = Config.UseAndroidLoopback; //http://10.0.2.2:5000
public IServiceClient CreateClient() => new JsonServiceClient(BaseUrl);
```

### Shared Gateway

The Shared Gateway shows an example of reusing shared app logic across all Client Apps with the [Shared.Client](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Shared.Client) project which contains an example of a high-level gateway that provides a facade over using Typed DTOs and Service Clients directly. The [Shared.Client.csproj](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Shared.Client/Shared.Client.csproj) is also multi-targeted and used to support both .NET Standard 2.0 and .NET Framework clients.

## Xamarin.iOS

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/ios.png)

An additional constraint for developing iOS Apps in Visual Studio is needing to have a Mac with Xcode installed which is what's used to build your App and run it in the iOS Simulator or if preferred it can be deployed and run directly on your iOS device.

This also means that when running your iOS App it's either running from the iOS Simulator on your Mac or remotely on your iOS Device. In either case you'll need to replace [Config.UseNetworkIp](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/ServiceModel/Config.cs) with your local Network IP Address which the iOS App uses to call back into your local pc where your ServiceStack Server is running: 

```csharp
private const string BaseUrl = Config.UseNetworkIp; //http://10.0.x.x:5000
public IServiceClient CreateClient() => new JsonServiceClient(BaseUrl);
```

Xamarin also includes a designer in Visual Studio to develop your iOS Layouts using Interface Builder's storyboards where you'll be able to drag UI elements from the toolbox and lay them out to create your UI. Unlike Android you'll be able to double-click the button to generate an event handler where you can fill in your implementation:

```csharp
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
```

### Authenticated Requests

This shows an example of performing authenticated requests with ServiceStack's Service clients where the client initially needs to Authenticate by sending a populated `Authenticate` Request DTO which if successful will establish an Authenticated Session on the Server that's referenced from the returned [Session Cookies](http://docs.servicestack.net/sessions) that are populated on the Service Client instance so they're included in subsequent requests where they'll perform Authenticated Requests that can be used to call protected Services like `HelloAuth`.

The server implementation used to facilitate the above request uses a naive Custom `CredentialsAuthProvider` that's validated against a hard coded Username and Password and enabled by registering it with [ServiceStack's AuthFeature](http://docs.servicestack.net/authentication-and-authorization) plugin:

```csharp
public class CustomCredentialsAuthProvider : CredentialsAuthProvider
{
    public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
    {
        return userName == "user" && password == "pass";
    }
}

Plugins.Add(new AuthFeature(() => new AuthUserSession(),
    new IAuthProvider[] {
        new CustomCredentialsAuthProvider()
    }));
```

Once configured the `[Authenticate]` [Filter Attribute](http://docs.servicestack.net/filter-attributes) can be added at the class or individual method level to protect Services:

```csharp
[Authenticate]
public class AuthServices : Service
{
    public object Any(HelloAuth request)
    {
        return new HelloResponse { Result = "Hello, " + request.Name };
    }
}
```

### Simple and Versatile Auth and OAuth Providers

ServiceStack's [Auth Feature](http://docs.servicestack.net/authentication-and-authorization) includes a number of different Authentication options that supports persistence to a variety of different back ends, e.g. ServiceStack's OAuth Providers makes it easy to authenticate using the built-in OAuth SDK options in client platforms to enable [Integrated Facebook, Twitter and Google Logins](https://github.com/ServiceStackApps/AndroidJavaChat#integrated-facebook-twitter-and-google-logins).

## OSX

Xamarin also extends the reach of C# to enable development of macOS Desktop Applications using [Visual Studio for Mac](https://www.visualstudio.com/vs/visual-studio-mac/) which was used to develop the [Client.OSX](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Client.OSX) Desktop App:

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/osx.png)

Whilst many of the concepts are similar to developing an iOS Application, it does require using XCode and its Interface Builder on your Mac to create UIs where changes need to be "synced" back to your C# project. A typical example is [adding an Action](https://developer.xamarin.com/guides/mac/getting_started/hello,_mac/#Adding_an_Action) in Xcode which needs to be saved before switching back to your C# Project where you can implement the `partial` method that Xamarin generates in the [ViewController.designer.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.OSX/ViewController.designer.cs) backing file:

```csharp
async partial void btnJwt_Click(NSObject sender)
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

        lblResults.StringValue = response.Result;
    }
    catch (Exception ex)
    {
        lblResults.StringValue = ex.ToString();
    }
}
```

### JWT Auth

This shows an example of authenticating using ServiceStack's stateless [JWT AuthProvider](http://docs.servicestack.net/jwt-authprovider) which allows you to Authenticate with an different central Auth Server to return an Authenticated Users Session encapsulated in a stateless JWT Token that can then be used to make authenticated requests on different servers configured with the same Auth Key. Support for JWT is integrated in all ServiceStack's Service Clients where you can populate its `BearerToken` property to have the JWT sent as a Bearer Token in the HTTP `Authorization` Header sent of each request.

ServiceStack's JWT Auth Provider works symbiotically with other registered Auth Providers where it can create a JWT Token of a User Session that was Authenticated with a different Auth Provider by having them both registered in the `AuthFeature` plugin, e.g:

```csharp
Plugins.Add(new AuthFeature(() => new AuthUserSession(),
    new IAuthProvider[] {
        new CustomCredentialsAuthProvider(),
        new JwtAuthProvider
        {
            AuthKeyBase64 = Config.JwtAuthKeyBase64,
            RequireSecureConnection = false,
        },
    }));
```

The above configuration enables Authentication using Username / Password credentials which if successful returns the `AuthenticateResponse` populated with a JWT Token of the Users Session in the `BearerToken` property. If more flexibility is needed you can also [convert an existing Authenticated Session into a JWT Token](http://docs.servicestack.net/jwt-authprovider#convert-sessions-to-tokens).

## Xamarin.Forms

Xamarin.Forms enables maximum reuse possible in C# Apps by letting you develop and share a Single UI across all your Apps.

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/xamarin.forms-reuse.png)

For Basic Apps with Simple UI's like this no specialization was needed as you can define both the UI and its complete implementation in a shared [Client.XamarinForms](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Client.XamarinForms/Client.XamarinForms) project using an Xamarin.Forms [MainPage.xaml](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.XamarinForms/Client.XamarinForms/MainPage.xaml) markup to develop its UI using an XAML dialect that closely resembles the XAML used in UWP and WPF Apps:

```xml
<Grid Margin="10,20,0,100">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>
    <Grid.RowDefinitions>
        <RowDefinition Height="50" />
        <RowDefinition Height="*" />
    </Grid.RowDefinitions>

    <StackLayout Grid.Row="0" Orientation="Horizontal">
        <Button x:Name="btnSync" Text="Sync" />
        <Button x:Name="btnAsync" Text="Async" />
        <Button x:Name="btnGateway" Text="Gateway" />
        <Button x:Name="btnAuth" Text="Auth" />
        <Button x:Name="btnJwt" Text="JWT" />
        <Button x:Name="btnEncrypted" Text="Encrypted" />
    </StackLayout>

    <Label Grid.Row="1" x:Name="lblResults" Text="" FontSize="36"
           HorizontalTextAlignment="Center" VerticalTextAlignment="Center" />

</Grid>
```

Since [MainPage.xaml.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.XamarinForms/Client.XamarinForms/MainPage.xaml.cs) is run on each of Xamarin.Forms Android, iOS and UWP Apps, it also needs to make concessions for resolving the appropriate `BaseUrl` of the ServiceStack Server for each platform:

```csharp
private static string BaseUrl = Device.RuntimePlatform == Device.Android ?
    Config.UseAndroidLoopback
    : Device.RuntimePlatform == Device.iOS ?
    Config.UseNetworkIp :
    Config.BaseUrl;

public IServiceClient CreateClient() => new JsonServiceClient(BaseUrl);
```

Events in Xamarin.Forms are registered by assigning a delegate to the `Clicked` event of each button:

```csharp
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
```

This example shows how you can use ServiceStack's [Encrypted Messaging feature](http://docs.servicestack.net/encrypted-messaging) to make Encrypted API calls over an insecure HTTP connection by using the [Encrypted Service Client](http://docs.servicestack.net/encrypted-messaging#encrypted-service-client) that's configured with the Public RSA Key used in the ServiceStack Server.

To enable Encrypted Messaging on the Server you just need to register the `EncryptedMessagesFeature` Plugin with the Private RSA Key you'd like to use:

```csharp
Plugins.Add(new EncryptedMessagesFeature
{
    PrivateKeyXml = Config.PrivateKeyXml
});
```

Which now lets you accept Encrypted API Requests in all Platforms the .NET Service Clients can run on:

### XamarinForms iOS

No changes were needed in [Client.XamarinForms.iOS](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Client.XamarinForms/Client.XamarinForms.iOS) which runs the vanilla XamarinForms iOS App without any additional modifications:

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/xamarinforms.ios.png)

### XamarinForms UWP

The [Client.XamarinForms.UWP](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Client.XamarinForms/Client.XamarinForms.UWP) project also runs the XamarinForms UI as-is without any specialization:

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/xamarinforms.uwp.png)

### XamarinForms Android

The same XamarinForms App running on [Client.XamarinForms.Android](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Client.XamarinForms/Client.XamarinForms.Android):

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/xamarinforms.android.png)

#### Linking Assemblies 

There's currently an issue in Xamarin.Forms Android Apps referencing .NET Standard 2.0 libraries preventing Android Apps from starting that until it's resolved [can be worked around](https://github.com/xamarin/AndroidSupportComponents/issues/75#issuecomment-352647321) by changing **Linking** behavior in the projects **Android Options** to link `SDK and User Assemblies`.

As the serialization libraries makes heavy use of reflection they'll need to excluded from being linked by adding them to **Skip Linking Assemblies** text box:

    ServiceStack.Text;ServiceStack.Client;ServiceModel

An alternative solution to be able to link ServiceStack .dll's is to paste a copy of [JsAot.cs](https://github.com/ServiceStack/ServiceStack.Text/blob/master/src/ServiceStack.Text/JsAot.cs) into your XamarinForms common project and have it call `ServiceStack.JsAot.Run();` on Startup (e.g. in App.xaml.cs) so it can give static type hints to Xamarin's AOT compiler and prevent internal classes from being erased during linking. This will allow the AOT compiler to trim the size of ServiceStack libraries but may cause issues at runtime, e.g. this approach works for all API calls except Encrypted Messaging which requires the skipping linking the assemblies solution above.
