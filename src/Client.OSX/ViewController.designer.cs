// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Client.OSX
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        AppKit.NSTextField lblResults { get; set; }

        [Action ("btnAsync_Click:")]
        partial void btnAsync_Click (Foundation.NSObject sender);

        [Action ("btnAuth_Click:")]
        partial void btnAuth_Click (Foundation.NSObject sender);

        [Action ("btnEncrypted_Click:")]
        partial void btnEncrypted_Click (Foundation.NSObject sender);

        [Action ("btnGateway_Click:")]
        partial void btnGateway_Click (Foundation.NSObject sender);

        [Action ("btnJwt_Click:")]
        partial void btnJwt_Click (Foundation.NSObject sender);

        [Action ("btnSync_Click:")]
        partial void btnSync_Click (Foundation.NSObject sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (lblResults != null) {
                lblResults.Dispose ();
                lblResults = null;
            }
        }
    }
}
