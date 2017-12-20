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
    [Register ("Client_iOS_ViewController")]
    partial class Client_iOS_ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAsync { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAuth { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnEncypted { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnGateway { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnJwt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSync { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblResults { get; set; }

        [Action ("BtnAsync_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnAsync_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnAuth_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnAuth_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnEncypted_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnEncypted_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnGateway_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnGateway_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnJwt_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnJwt_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnSync_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnSync_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnAsync != null) {
                btnAsync.Dispose ();
                btnAsync = null;
            }

            if (btnAuth != null) {
                btnAuth.Dispose ();
                btnAuth = null;
            }

            if (btnEncypted != null) {
                btnEncypted.Dispose ();
                btnEncypted = null;
            }

            if (btnGateway != null) {
                btnGateway.Dispose ();
                btnGateway = null;
            }

            if (btnJwt != null) {
                btnJwt.Dispose ();
                btnJwt = null;
            }

            if (btnSync != null) {
                btnSync.Dispose ();
                btnSync = null;
            }

            if (lblResults != null) {
                lblResults.Dispose ();
                lblResults = null;
            }
        }
    }
}