// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Client.iOS
{
    [Register ("Client_iOS10_PclViewController")]
    partial class Client_iOS_ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnGoAwait { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnGoSync { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnGoTask { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblResults { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtName { get; set; }

        [Action ("btnGoAwait_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnGoAwait_TouchUpInside (UIKit.UIButton sender);

        [Action ("btnGoSync_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnGoSync_TouchUpInside (UIKit.UIButton sender);

        [Action ("btnGoTask_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnGoTask_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnGoAwait != null) {
                btnGoAwait.Dispose ();
                btnGoAwait = null;
            }

            if (btnGoSync != null) {
                btnGoSync.Dispose ();
                btnGoSync = null;
            }

            if (btnGoTask != null) {
                btnGoTask.Dispose ();
                btnGoTask = null;
            }

            if (lblResults != null) {
                lblResults.Dispose ();
                lblResults = null;
            }

            if (txtName != null) {
                txtName.Dispose ();
                txtName = null;
            }
        }
    }
}