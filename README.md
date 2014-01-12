Hello ServiceStack!
===================

This project shows a number of different .NET clients applications communicating with ServiceStack back-end services
using the Sync, Task-based Async and C# async/await APIs, available in [ServiceStack's typed .NET clients](https://github.com/ServiceStack/ServiceStack/wiki/C%23-client).

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
  - ServiceStack.Client.Pcl
  	- PCL Profiles: iOS, Android, Windows8, .NET 4.5
  	- Custom builds: Silverlight 5
 - ServiceStack.Text.Pcl
  	- PCL Profiles: iOS, Android, Windows8, .NET 4.5
  	- Custom builds: Silverlight 5
 
Your DTO projects only need to reference **ServiceStack.Interfaces.Pcl** package whilst the service clients are contained within
the **ServiceStack.Client.Pcl** NuGet package.    	

As described above, only **ServiceStack.Interfaces.Pcl** supports most of the available PCL profiles. 
Although this alone still enables great re-use thanks to ServiceStack's design of having most providers implementing interfaces, 
which combined with DTO's having minimal dependencies, only a reference to ServiceStack.Interfaces is required to share any 
higher-level functionality that consumes ServiceStack services across most platforms. 

A greater level of binary-level re-usabiliy is enabled between iOS, Android, Windows8, .NET 4.5 platforms which are also 
able to share concrete implementations and extension methods in their own applications portable libraries. 

Other Supported platforms may still achieve source-level code-reuse by creating a stub project (compiled for their platform) 
and linking to the existing source files. 

## Run the ServiceStack Host

All client examples below expect to connect a ServiceStack service instance hosted on **http://localhost:81**. 

Also included in the repo is an ASP.NET and HttpListener simple ServiceStack Hosts, both with CORS enabled (required for Silverlight clients).

The easiest way to start a ServiceStack host is to build the **Server.HttpListener** project, then double-click on the resulting **Server.HttpListener.exe** binary, 
or for OSX/Linux using mono:

    sudo mono Server.HttpListener.exe

Check that it's up and running by going to: `http://localhost:81` in a web browser.

## Xamarin.Android

[![Android Screenshot](https://raw2.github.com/ServiceStack/Hello/master/screenshots/clients-android.png)](https://github.com/ServiceStack/Hello/tree/master/src/Client.Android.Pcl)

The Android example is contained in the [Client.Android.Pcl](https://github.com/ServiceStack/Hello/tree/master/src/Client.Android.Pcl) project.

Xamarin.Android makes creating Android applicaitons with C# pretty trivial where you can visually design the UI from within VS.NET 
using the in-built visual designer. The worst part about developing for Android is the very slow turn around times when running your application
through the Android emulators. For this reason I recommend configuring and doing most of your development with the much faster 
[x86 Emulator](http://docs.xamarin.com/guides/android/deployment,_testing,_and_metrics/configuring_the_x86_emulator/).

Xamarin's [Getting Started tutorial](http://docs.xamarin.com/guides/android/getting_started/hello,_world/) provides a great walkthrough 
and overview of the concepts required in creating a simple Android app. 

To creat our simple Hello World example, it as just a matter of:

  1. Creating a new **Android Application** project
  2. Double-clicking `Resources\Layout\Main.axml` to bring up the visual designer
  3. Dragging the Label (TextView), TextBox (EditText) and Button Widgets onto the canvas to create the UI
  4. Providing an id for each widget, e.g in the format `@+id/txtName`

After building the project the id's are materialized into C# constants, available in the `Resource.Id.*` nested classes which you can use 
in the custom `Activity.OnCreate()` method in your [Activity1.cs](https://github.com/ServiceStack/Hello/blob/master/src/Client.Android.Pcl/Activity1.cs) 
to access the UI controls, e.g:

```csharp
var btnSync = FindViewById<Button>(Resource.Id.btnSync);
var btnAsync = FindViewById<Button>(Resource.Id.btnAsync);
var btnAwait = FindViewById<Button>(Resource.Id.btnAwait);
var txtName = FindViewById<EditText>(Resource.Id.txtName);
var lblResults = FindViewById<TextView>(Resource.Id.lblResults);
```

With references to the controls you can begin to hook up custom handlers to the exposed C# events.

For our example we're just going to explore the different call-styles available for consuming ServiceStack webservices.
To do this we just need create an instance of a `JsonServiceClient` and give it the base url where ServiceStack is hosted:

```csharp
var client = new JsonServiceClient("http://10.0.2.2:81/");
```

As the Android emulator is considered to be running on a different device, to refer to the loopback IP (127.0.0.1) on our local development machine 
we need to use the special **10.0.2.2** alias. Other special device IP's can be found in [Andorid's documentation](http://developer.android.com/tools/devices/emulator.html#emulatornetworking).

### Calling ServiceStack services

From then on calling ServiceStack services is the same as any other [C# Client](https://github.com/ServiceStack/ServiceStack/wiki/C%23-client), 
E.g. you can call the Sync APIs with:

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

This registers a click handler on the 'Sync' button that uses the JsonServiceClient to make a synchronous call to the **Hello** ServiceStack WebService. 
The returned typed 'HelloResponse' response can be directly assigned to the **lblResults** UI widget.

#### Using C#'s async/await feature

Commonly you should prefer making asynchronous network calls to keep the UI responsive whilst waiting for the response. 
Thanks to C#'s async/await feature converting to async is trivial which just involves adding the **async** keyword modifier to the delegate signature 
and then awaiting the `GetAsync` method on the Service Client, e.g:

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

#### Using manual Task extensions

Some compiler magic is used to make the above async code work which has the disadvantage that the `async` keyword needs to propogated up in all call-sites.
Given this you may prefer instead to make async calls with the Promise-like call-style API also avaialble on the returned `Task<T>` response. 

We've added some custom task extensions to make this as easy as:

```csharp
btnAsync.Click += delegate {
    client.GetAsync(new Hello { Name = txtName.Text })
        .Success(response => lblResults.Text = response.Result)
        .Error(ex => lblResults.Text = ex.ToString());
};
```

The `Success` extension method unwraps the response for successful calls whilst the `Error` extension method unwraps single exceptions from any thrown
AggregateException, otherwise it passes it through untouched.


## Xamarin.iOS

[![iPhone Screenshot](https://raw2.github.com/ServiceStack/Hello/master/screenshots/clients-ios.png)](https://github.com/ServiceStack/Hello/tree/master/src/Client.iOS)


The [iOS Client](https://github.com/ServiceStack/Hello/tree/master/src/Client.iOS) is the only project in this solution not created with VS.NET. 

Whilst it's possible to develop 
[iOS Apps in VS.NET](http://docs.xamarin.com/guides/ios/getting_started/introduction_to_xamarin_ios_for_visual_studio/), 
it requires a configured build server running on OSX fot it to work. 

As OSX is always required and it has less moving parts, I recommend developing iOS Apps with Xamarin Studio on OSX which has the advantage of being able to use 
XCode's native Interface Builder to design your UI.

If this is the first time using Xamarin Studio then you'd want to first 
[install the NuGet Addin for MonoDevelop](http://barambani.wordpress.com/2013/10/07/add-nuget-package-manager-and-servicestack-to-xamarin-studio-projects-2/)
which will allow you to add NuGet packages to your project as you're used to within VS.NET. From there just search and add the **ServiceStack.Client.Pcl**
NuGet package to your iOS project.

As they have with Android, Xamarin have a good [getting started with Xamarin.iOS tutorial](http://docs.xamarin.com/guides/ios/getting_started/hello,_world/) 
which goes through a simple example of creating an iOS App. 

To create our simple Hello World app:

  1. Create a new iPhone **Single View Application** project
  2. Double-click on the file ending with `*ViewController.xib` to open it in Xcode's Interface Builder
  3. Drag the Label, TextBox and Button Widgets onto the iPhone canvas to create the UI
  4. Open Interface builder in [split screen mode](http://docs.xamarin.com/guides/ios/getting_started/hello,_world/#Outlets_Actions_Defined) to view the UI and the `*.h` file side-by-side 
  5. Ctrl + click TextBox UI and Results Label UI elements onto the top of Obj-c `*.h` file to [create new Outlets](http://docs.xamarin.com/guides/ios/getting_started/hello,_world/#Adding_an_Outlet)
  6. Ctrl + click the Buttons and drag into the middle of the Obj-c `*.h` file to [create new Actions](http://docs.xamarin.com/guides/ios/getting_started/hello,_world/#Adding_an_Action) for each button

After hooking up each UI Widget, save the file and switch back to Xamarin Studio to see each element available in the code-behind 
[ViewController.designer.cs](https://github.com/ServiceStack/Hello/blob/master/src/Client.iOS/Client_iOSViewController.designer.cs) file. 
Any Outlets defined are exposed as properties whilst any Actions are available as partial methods.

With all the elements and actions in place you can start add your C# implementation in your main [*ViewController.cs](https://github.com/ServiceStack/Hello/blob/master/src/Client.iOS/Client_iOSViewController.cs) file.


