// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace Client.iOS
{
	[Register ("Client_iOSViewController")]
	partial class Client_iOSViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel lblResults { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtName { get; set; }

		[Action ("btnAsync_Click:")]
		partial void btnAsync_Click (MonoTouch.Foundation.NSObject sender);

		[Action ("btnAwait_Click:")]
		partial void btnAwait_Click (MonoTouch.Foundation.NSObject sender);

		[Action ("btnSync_Click:")]
		partial void btnSync_Click (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (txtName != null) {
				txtName.Dispose ();
				txtName = null;
			}

			if (lblResults != null) {
				lblResults.Dispose ();
				lblResults = null;
			}
		}
	}
}
