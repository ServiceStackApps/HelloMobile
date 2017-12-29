ServiceStack Mobile and Desktop Apps
====================================

This projects contains a number of examples for consuming ServiceStack Typed Services from a variety of different native Mobile and Desktop Apps developed with C#/.NET. Using C# to develop native Mobile and Desktop Apps provides a number of benefits including maximum reuse of your source code across both multiple client apps and reusing shared functionality, libraries, knowledge reuse, development environment and development workflows between client and server. 

This reusability is enhanced in ServiceStack Apps which are able to reuse DTOs in the Server's ServiceModel.dll that's used to define the Services contract with, in clients Apps to provide a highly productive, terse, end-to-end typed API without any additional custom build tools, code-gen or any other artificial machinery using just `ServiceModel.dll` DTOs and any of the available highly performant [C#/.NET generic Service Clients](http://docs.servicestack.net/csharp-client).

## Mobile and Desktop Apps

![Supported Client Platforms](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/splash-900.png)

This project contains multiple versions of the same App demonstrating a number of different calling conventions and service integrations for each of the following platforms:

  - WPF
  - UWP
  - Xamarin.iOS
  - Xamarin.Android
  - Xamarin.OSX
  - Xamarin.Forms
    - iOS
    - Android
    - UWP

Whilst the event handlers can vary, the source code used in each App to call Services remains exactly the same.

### Native SDKs and Development Environments

This project only focuses on C# Mobile and Desktop Apps, if you instead prefer to utilize the native SDK's and development environment in each platform, use [Add ServiceStack Reference](http://docs.servicestack.net/add-servicestack-reference) to generate Typed DTOs in [Java](http://docs.servicestack.net/java-add-servicestack-reference) and [Kotlin](http://docs.servicestack.net/kotlin-add-servicestack-reference) for calling ServiceStack Services in Android Apps, use [Swift](http://docs.servicestack.net/swift-add-servicestack-reference) for development of native iOS or OSX Apps or use [TypeScript](http://docs.servicestack.net/typescript-add-servicestack-reference) for calling Services in [React Native, Node.js or Web Apps](https://github.com/ServiceStackApps/typescript-server-events).

Each platform provides a similar development experience as C# using just the generated DTOs and the generic Service Client available in each platform which utilize the same method names and message-based API (idiomatic to each platform) to maximize knowledge reuse and simplify porting efforts, if needed.

C# Clients can also use [C# Add ServiceStack Reference](http://docs.servicestack.net/csharp-add-servicestack-reference) to generate Typed DTOs as an alternative to sharing the Server DTOs in `ServiceModel.dll` to eliminate binary coupling. The source code for both solutions remains exactly the same.

## Install ServiceStack Client

You can install ServiceStack's Service Clients in your own C# projects by installing the **ServiceStack.Client** NuGet package: 

    PM> Install-Package ServiceStack.Client

Alternatively you can instead use `JsonHttpClient` from:

    PM> Install-Package ServiceStack.HttpClient

Which as it's based on Microsoft's async HttpClient can be [configured to be used with ModernHttpClient](http://docs.servicestack.net/csharp-client#jsonhttpclient) which provides a thin wrapper around iOS's native `NSURLSession` or `OkHttp` client on Android, offering improved performance and stability for mobile connectivity. 

Configure all `JsonHttpClient` instances to use ModernHttpClient's `NativeMessageHandler` with:

```csharp
JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler()
```

### [Cache Aware Service Clients](http://docs.servicestack.net/cache-aware-clients)

When [caching is enabled on Services](http://docs.servicestack.net/http-caching), the Cache-aware Service Clients can dramatically improve performance by eliminating server requests entirely as well as reducing bandwidth for re-validated requests. They also offer an additional layer of resiliency as re-validated requests that result in Errors will transparently fallback to using pre-existing locally cached responses. For bandwidth-constrained environments like Mobile Apps they can dramatically improve the User Experience.

The Cache-Aware clients implement the full `IServiceClient` interface so they should be an easy drop-in enhancement for existing Apps:

```csharp
IServiceClient client = new JsonServiceClient(baseUrl).WithCache(); 

//equivalent to:
IServiceClient client = new CachedServiceClient(new JsonServiceClient(baseUrl));
```

Likewise for the HttpClient-based `JsonHttpClient`:

```csharp
IServiceClient client = new JsonHttpClient(baseUrl).WithCache(); 

//equivalent to:
IServiceClient client = new CachedHttpClient(new JsonHttpClient(baseUrl));
```

## ServiceStack Server App

Each App calls the simple ServiceStack Server [WebServices.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Server.Common/WebServices.cs) implementation below:

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

As a demonstration of ServiceStack's versatility on the server the same shared implementation above can be run on any of .NET's popular HTTP hosting configurations:

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

The difference between a **.NET Framework v4.7** and a **.NET Core 2.0** ASP.NET Core App is in [Server.NetCoreFx.csproj](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Server.NetCoreFx/Server.NetCoreFx.csproj) where it needs to reference "ServiceStack.Core" NuGet package to force using the **.NET Standard 2.0** version of ServiceStack which contains support for hosting ASP.NET Core Apps.

### [Server.AspNet](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Server.AspNet)

The same source code is also used for hosting classic ASP.NET Web Applications:

```csharp
public class AppHost : AppHostBase
{
    public AppHost() : base(nameof(Server.AspNet), typeof(WebServices).Assembly) { }
    public override void Configure(Container container) => SharedAppHost.Configure(this);
}
```

The difference is how ServiceStack's AppHost is initialized where in ASP.NET it's initialized in 
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

Where in ASP.NET Core Apps it's initialized in the `Startup` class in [Program.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Server.NetCore/Program.cs)

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

For Self-Hosting .NET Framework HttpListener Apps the difference is needing to inherit from `AppSelfHostBase`:

```csharp
public class AppHost : AppSelfHostBase
{
    public AppHost() : base(nameof(Server.HttpListener), typeof(WebServices).Assembly) {}
    public override void Configure(Container container) => SharedAppHost.Configure(this);
}
```

### Run the Server App

All Hosts run the same ServiceStack Application on the same **http://localhost:5000** port. You can run your preferred Host by selecting **Set as Startup project** on the project's right-click context menu then hitting `Ctrl+F5` to run it.

Alternatively for .NET Core Apps you can run it from the command-line at the projects folder with:

    $ dotnet run

Whilst you can double-click the compiled **Server.HttpListener.exe** to run the Self-Hosting HttpListener in the background.

After you've started your prefered host, check that it's up and running by going to: `http://localhost:5000` in a web browser.

## Client Apps

Once you have a server running you can run any of the included Client Apps by selecting it as the **StartUp Project** in the context menu and hitting `Ctrl+F5` to run.

Each of the Client Apps have the same functionality demonstrating popular examples use-cases of calling ServiceStack Services:

## WPF

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/wpf.png)

Each Client App uses the same source code of sending Typed DTOs in the **ServiceModel.dll** to call Services utilizing different APIs and features in ServiceStack Service Clients, visible in WPF's [MainWindow.xaml.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.Wpf/MainWindow.xaml.cs) implementation below:

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

The Sync API shows an example of the simplest API call you can make with Service Clients by sending a Request DTO and receiving a typed Response DTO:

```csharp
var client = CreateClient();
var response = client.Get(new Hello { Name = "Sync" });
lblResults.Text = response.Result;
```

In this instance it sends a HTTP GET Request to the `/hello?Name=Sync` Service with the `Accept: application/json` HTTP Header so it returns a JSON Response that's deserialized into the `HelloResponse` DTO. 

As `Get` makes a synchronous I/O call you'll want to avoid invoking it from the main UI Thread to avoid blocking the UI.

## UWP

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/uwp.png)

UWP's [MainPage.xaml.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.UWP/MainPage.xaml.cs) source code is exactly the same as WPF's above, it even uses the same [XAML markup](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.UWP/MainPage.xaml) used to create the UI was reused from [WPF's XAML](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.Wpf/MainWindow.xaml):

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

The screenshot shows an example of making non-blocking requests which requires adding the `async` keyword on the event handler signature so you can use C#'s `await` keyword when calling the Service Clients `*Async` APIs:

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

The Service Clients has an equivalent Async API for every Sync API differentiated by a `*Async` suffix.

## Xamarin.Android

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/android.png)

Developing for iOS or Android requires using Xamarin tools which can be installed in VS.NET using the Visual Studio Installer. The integrated Xamarin Designers provides a familiar development experience for developing Android Apps as other .NET Windows Desktop Apps where you can drag UI elements from the toolbox and position them in the page designer to develop the layout of your page. But instead of generating XAML it generates UI's using [Android's XML Layout](https://developer.android.com/guide/topics/ui/declaring-layout.html). 

It also follows a similar development model to Android with Java where instead of being able to double-click the button to add a click event handler you would need to find the button element in the loaded Activity Layout and manually register the click event handler, e.g:

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

The other difference since the App is being run in Android's Emulator is that you would need to use Android's loopback address `http://10.0.2.2` in order to call a Service running on `localhost`:

```csharp
private const string BaseUrl = Config.UseAndroidLoopback;
public IServiceClient CreateClient() => new JsonServiceClient(BaseUrl);
```

### Shared Gateway

The Shared Gateway shows an example of reusing shared app logic across all Client Apps with the [Shared.Client](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Shared.Client) project 
which contains an example of a high-level gateway that provides a facade over using Typed DTOs with the `ServiceStack.Client` NuGet package directly. The [Shared.Client.csproj](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Shared.Client/Shared.Client.csproj) is also multi-targeted to support and is used in both .NET Standard 2.0 and .NET Framework clients.

## Xamarin.iOS

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/ios.png)

The additional constraint for developing iOS Apps in Visual Studio is needing to have a Mac with Xcode installed which is what's used to build your App and run the iOS Simulator or if preferred you can deploy and run your App directly on the device.

This also means that when running your iOS App it's either running in the iOS Simulator on your Mac or remotely on your iOS Device. In both cases you'll need to replace [Config.UseNetworkIp](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/ServiceModel/Config.cs) with your local Network IP Address which the iOS App uses to call back into your local pc where your ServiceStack Server is running: 

```csharp
private const string BaseUrl = Config.UseNetworkIp;
public IServiceClient CreateClient() => new JsonServiceClient(BaseUrl);
```

Xamarin also includes a designer in Visual Studio to develop your iOS Layouts using Interface Builder's storyboards where you'll be able to drog UI elements from the toolbox and lay them out to develop your UI. Unlike Android you'll be able to double-click the button to generate an event handler where you can fill in your implementation:

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

This shows an example of making authenticated requests with ServiceStack's Service clients where the client initially needs to Authenticate by sending a populated `Authenticate` Request DTO. If successful it establishes an Authenticated Session on the Server and returns [Session Cookies](http://docs.servicestack.net/sessions) which are populated on the Service Client instance so they're included in subsequent requests to make Authenticate Requests to protected Services like `HelloAuth`.

The server implementation used to facilitate the above request uses a naive Custom `CredentialsAuthProvider` which is validated against a hard coded Username and Password and registered with [ServiceStack's AuthFeature](http://docs.servicestack.net/authentication-and-authorization) plugin to enable Authentication:

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

Once configured you can protect Services at the class or individual method level with the `[Authenticate]` Request Filter Attribute:

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

## OSX

Xamarin also extends the reach of C# to also allow developing macOS Desktop Applications using [Visual Studio for Mac](https://www.visualstudio.com/vs/visual-studio-mac/) on your Mac which was used to develop the [Client.OSX](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Client.OSX) AppKit Desktop App:

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/osx.png)

Whilst many of the concepts are similar to developing an iOS Application, it does require using a different IDE and XCode's Interface Builder for developing UIs where changes are "synced" back to your C# project where after [adding an Action](https://developer.xamarin.com/guides/mac/getting_started/hello,_mac/#Adding_an_Action) in Xcode you'll need to save then switch back to your C# Project where you can implement the partial method that was generated in the [ViewController.designer.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.OSX/ViewController.designer.cs) backing file:

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

This shows an exmaple of authenticating using ServiceStack's stateless [JWT AuthProvider](http://docs.servicestack.net/jwt-authprovider) where you can Authenticate with an different central Auth Server to return the Users Session encapsulated in a stateless JWT Token that can then be used to make authenticated requests with different servers configured with the same Auth Key. Support for JWT is integrated in all ServiceStack's Service Clients where you can populate the `BearerToken` property to have the JWT sent as a Bearer Token in the HTTP Authorization in each request.

ServiceStack's JWT Auth Provider works symbiotically with other registered Auth Providers where you can retrieve the JWT Token of a User Session Authenticated using a different Auth Provider by having them registered in the `AuthFeature` plugin:

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

The above configuration enables Authentication using Username / Password credentials and if successful return the `AuthenticateResponse` populated with a JWT Token of the Users Session in the `BearerToken` property. When needed you can also [convert an existing Authenticated Session into a JWT Token](http://docs.servicestack.net/jwt-authprovider#convert-sessions-to-tokens).

## Xamarin.Forms

Xamarin.Forms enables the maximum reuse possible in C# Apps by letting you develop and share a Single UI across all your mobile Apps.

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/xamarin.forms-reuse.png)

For Basic Apps with Simple UI's like this no specialization was needed and you can define both the UI and its implementation in a shared [Client.XamarinForms](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Client.XamarinForms/Client.XamarinForms) project using an Xamarin.Forms [MainPage.xaml](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.XamarinForms/Client.XamarinForms/MainPage.xaml) markup to develop your UI using an XAML dialect that closely resembles the XAML used in UWP and WPF Apps:

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

Whilse the [MainPage.xaml.cs](https://github.com/ServiceStackApps/HelloMobile/blob/master/src/Client.XamarinForms/Client.XamarinForms/MainPage.xaml.cs) contains the shared implementation of the view which also needs to add concessions for resolving the `BaseUrl` of the ServiceStack Server Instance when it's running on the different Xamarin.Forms Android, iOS and UWP Apps:

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

This example shows how you can use ServiceStack's [Encrypted Messaging feature](http://docs.servicestack.net/encrypted-messaging) to make Encrypted API calls over an insecure HTTP connection by using an [Encrypted Service Client](http://docs.servicestack.net/encrypted-messaging#encrypted-service-client) configured with the Public RSA Key used in the remote ServiceStack Server.

To enable Encrypted Messaging in ServiceStack on the Server you just need to register the `EncryptedMessagesFeature` Plugin with the Private RSA Key you'd like to use:

```csharp
Plugins.Add(new EncryptedMessagesFeature
{
    PrivateKeyXml = Config.PrivateKeyXml
});
```

Which then lets you accept Encrypted API Requests in all Platforms the C#/.NET Service Clients run on:

### XamarinForms iOS

No changes were needed in [Client.XamarinForms.iOS](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Client.XamarinForms/Client.XamarinForms.iOS) which can run the vanilla XamarinForms iOS App without any specializations:

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/xamarinforms.ios.png?)

### XamarinForms Android

The same XamarinForms App running on [Client.XamarinForms.Android](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Client.XamarinForms/Client.XamarinForms.Android):

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/xamarinforms.android.png)

#### Linking Assemblies 

There's currently an issue in Xamarin.Forms Android Apps referencing .NET Standard 2.0 libraries which prevents Android Apps from starting which until it's resolved [you can workaround](https://github.com/xamarin/AndroidSupportComponents/issues/75#issuecomment-352647321) by changing **Linking** behavior in the projects **Android Options** to link `SDK and User Assemblies`.

As the serialization libraries makes heavy use of reflection they'll need to excluded from being linked by adding them to **Skip Linking Assemblies** text box:

    ServiceStack.Text;ServiceStack.Client;ServiceModel

An alternative solution to be able to link ServiceStack .dll's is to paste a copy of [JsAot.cs](https://github.com/ServiceStack/ServiceStack.Text/blob/master/src/ServiceStack.Text/JsAot.cs) into your XamarinForms common project and have it call `ServiceStack.JsAot.Run();` on Startup (e.g. in App.xaml.cs) so it can give static type hints to Xamarin's AOT compiler and prevent internal classes from being erased. This will allow the AOT compiler to trim the size of ServiceStack libraries but can cause issues at runtime, e.g. this approach works for all API calls except for Encrypted Messaging which requires skippping linking the assemblies above.

### XamarinForms UWP

The [Client.XamarinForms.UWP](https://github.com/ServiceStackApps/HelloMobile/tree/master/src/Client.XamarinForms/Client.XamarinForms.UWP) project also runs the XamarinForms App without any modifications:

![](https://raw.githubusercontent.com/ServiceStackApps/HelloMobile/master/screenshots/xamarinforms.uwp.png)

