using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using ServiceStack;
using ServiceModel;

namespace Client.iOS
{
	public partial class Client_iOSViewController : UIViewController
	{
		JsonServiceClient client;

		public Client_iOSViewController () : base ("Client_iOSViewController", null)
		{
			IosPclExportClient.Configure ();
			client = new JsonServiceClient ("http://10.0.0.8:81/");
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		partial void btnSync_Click (NSObject sender)
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

		partial void btnAsync_Click (NSObject sender)
		{
			client.GetAsync(new Hello { Name = txtName.Text })
				.Success(response => lblResults.Text = response.Result)
				.Error(ex => lblResults.Text = ex.ToString());
		}

		partial void btnAwait_Click (NSObject sender)
		{
			AwaitClick();
		}

		private async void AwaitClick()
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

