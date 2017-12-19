using System;
using System.Drawing;

using Foundation;
using UIKit;
using ServiceStack;
using ServiceModel;
using System.Threading.Tasks;

namespace Client.iOS
{
    public partial class Client_iOS_ViewController : UIViewController
	{
		const string BaseUrl = "http://test.servicestack.net/";

		JsonServiceClient client;

		static bool UserInterfaceIdiomIsPhone => UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;

	    public Client_iOS_ViewController(IntPtr handle) : base(handle)
		{
			client = new JsonServiceClient(BaseUrl);
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
		}

		#endregion

		partial void btnGoSync_TouchUpInside(UIButton sender)
		{
			Console.WriteLine("btnGoSync_TouchUpInside");
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

		async Task DoAwaitAsync()
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

		partial void btnGoAwait_TouchUpInside(UIButton sender)
		{
			Console.WriteLine("btnGoAwait_TouchUpInside");
			DoAwaitAsync();
		}

		partial void btnGoTask_TouchUpInside(UIButton sender)
		{
			Console.WriteLine("btnGoAsync_TouchUpInside");
			client.GetAsync(new Hello { Name = txtName.Text })
				.Success(response => lblResults.Text = response.Result)
				.Error(ex => lblResults.Text = ex.ToString());
		}
	}
}

