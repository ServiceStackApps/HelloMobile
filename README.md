Hello ServiceStack!
===================

This project shows a number of different .NET clients communicating with a back-end ServiceStack HttpListener or ASP.NET Host server 
using the Sync, Task-based Async and C# async/await APIs in [ServiceStack's typed .NET clients](https://github.com/ServiceStack/ServiceStack/wiki/C%23-client).

### Client Applications

This project contains example applications on the following platforms:

  - Xamarin.iOS
  - Xamarin.Android
  - Windows Store 
  - WPF app using .NET 4.5 PCL support
  - Silverlight 5

## Portable Class Library support

Most clients make use of ServiceStack's new PCL support which are contained in the following NuGet packages:

  - ServiceStack.Interfaces.Pcl
  	- PCL Profiles: iOS, Android, Windows8, .NET 4.5, Silverlight5, WP8
  - ServiceStack.Text.Pcl
  	- PCL Profiles: iOS, Android, Windows8, .NET 4.5
  	- Custom builds: Silverlight 5
  - ServiceStack.Client.Pcl
  	- PCL Profiles: iOS, Android, Windows8, .NET 4.5
  	- Custom builds: Silverlight 5

As described above, only **ServiceStack.Interfaces.Pcl** supports most of the PCL profiles available. 
Although this alone still enables great re-use thanks to ServiceStack's design of having most providers implementing interfaces, 
which combined with DTO's only requiring a reference to ServiceStack.Interfaces allows sharing of any higher-level functionality 
utilizing the Service Client interfaces and DTO's across most platforms. 

A greater level of binary-level re-usabiliy is enabled between iOS, Android, Windows8, .NET 4.5 platforms which are also share 
concrete implementations like extension methods in their applications portable libraries. 

Other Supported platforms may still achieve code-reuse with source-level compatibility by creating a stub project 
(compiled for their platform) and linking to the existing source files. 

## Run the ServiceStack Host

All client examples below expect to connect a ServiceStack service instance hosted on **http://localhost:81**. 

Included in the repo is both a simple ASP.NET and HttpListener ServiceStack Hosts, both with CORS enabled (required for Silverlight clients).

The easiest way to start a ServiceStack host is to build the **Server.HttpListener** project and double-click on the resulting **Server.HttpListener.exe** binary, 
or for OSX/Linux using mono:

    sudo mono Server.HttpListener.exe

To check that it's running go to: `http://localhost:81` in a web browser.

## Xamarin.Android

[![Android Screenshot](https://raw2.github.com/ServiceStack/Hello/master/screenshots/clients-android.png)](https://github.com/ServiceStack/Hello/tree/master/src/Client.Android.Pcl)

The Android example is contained in the [Client.Android.Pcl](https://github.com/ServiceStack/Hello/tree/master/src/Client.Android.Pcl) project.

Xamarin.Android makes creating Android applicaitons with C# pretty trivial in which you can visually design the UI from within VS.NET 
using the in-built visual designer. The worst part about developing for Android are the very slow turn around times or running your application
through the Android emulators. For this reason I recommend configuring and doing most of your development with the much faster 
[x86 Emulator](http://docs.xamarin.com/guides/android/deployment,_testing,_and_metrics/configuring_the_x86_emulator/).

Xamarin's [Getting Started tutorial](http://docs.xamarin.com/guides/android/getting_started/hello,_world/) provides a great walkthrough 
and covering the concepts required in creating a simple Android app. 

For our simple Hello World example it's simply a matter of:

  1. Double-clicking `Resources\Layout\Main.axml` to bring up the visual designer
  2. Dragging the Label (TextView), TextBox (EditText) and Button Widgets onto the canvas
  3. Providing an id for each widget, e.g in the format `@+id/txtName`

After building the project the id's are materialized into C# constants in the `Resource.Id.*` nested classes which you can use 
in the custom `Activity.OnCreate()` method in your [Activity1.cs](https://github.com/ServiceStack/Hello/blob/master/src/Client.Android.Pcl/Activity1.cs) 
to access the UI controls, e.g:

```csharp
var btnSync = FindViewById<Button>(Resource.Id.btnSync);
var btnAsync = FindViewById<Button>(Resource.Id.btnAsync);
var btnAwait = FindViewById<Button>(Resource.Id.btnAwait);
var txtName = FindViewById<EditText>(Resource.Id.txtName);
var lblResults = FindViewById<TextView>(Resource.Id.lblResults);
```

With references to the controls you can begin to hook up custom handlers with the exposed C# events.

For our example we're just going to explore the different call-styles available for calling the same ServiceStack web service.
To do this we just need create an instance of a `JsonServiceClient` and give it the base url where ServiceStack is hosted:

```csharp
var client = new JsonServiceClient("http://10.0.2.2:81/");
```

As the Android emulator is considered to be running on a different device, to refer to the loopback IP (127.0.0.1) on our local development machine 
we need to use the special **10.0.2.2** alias. Other special device IP's can be found in [Andorid's documentation](http://developer.android.com/tools/devices/emulator.html#emulatornetworking).

### Calling ServiceStack services

From then on calling ServiceStack services is the same [C# Client](https://github.com/ServiceStack/ServiceStack/wiki/C%23-client), e.g. you can use the Sync APIs:

```csharp
btnSync.Click += delegate {
    try {
        var response = client.Get(new Hello { Name = txtName.Text });
        lblResults.Text = response.Result;
    }
    catch (Exception ex) {
        lblResults.Text = ex.ToString();
    }
};
```

This registers a click handler to the 'Sync' button which uses the JsonServiceClient to make a synchronous call to the **Hello** ServiceStack WebService. 
The client returns a typed 'HelloResponse' response which can be assigned directly to the **lblResults** UI widget.

#### Using C#'s async/await feature

Commonly you would want to make asynchronous network calls to keep the UI responsive whilst you're waiting for the response. 
Thanks to C#'s async/await feature converting it is trivial which just involves adding the **async** keyword modifier to the delegate signature 
and then awaiting the equivalent `GetAsync` method on the Service Client, e.g:

```csharp
btnAwait.Click += async delegate {
    try {
        var response = await client.GetAsync(new Hello { Name = txtName.Text });
        lblResults.Text = response.Result;
    }
    catch (Exception ex) {
        lblResults.Text = ex.ToString();
    }
};
```

#### Using manual Task handlers

A bit of compiler magic is used to make the above async code work which has the disadvantage that the `async` keyword needs to propogated up in all call-sites.
For these reasons you may prefer to instead make async calls with a Promise-like API that's also avaialble on the returned `Task<T>` response. 

We've added some of our own task extensions to make this a little easier, e.g:

```csharp
btnAsync.Click += delegate {
    client.GetAsync(new Hello { Name = txtName.Text })
        .Success(response => lblResults.Text = response.Result)
        .Error(ex => lblResults.Text = ex.ToString());
};
```

## Xamarin.iOS

[![iPhone Screenshot](https://raw2.github.com/ServiceStack/Hello/master/screenshots/clients-ios.png)](https://github.com/ServiceStack/Hello/tree/master/src/Client.iOS)


