using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace AlertAsyncSample
{
    public partial class AlertAsyncSampleViewController : UIViewController
    {
        public AlertAsyncSampleViewController() : base("AlertAsyncSampleViewController", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
            
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }

        partial void ShowAlertButtonTapped (NSObject sender) {
            Task.Factory.StartNew(async () => {
                bool accepted = await ShowAlert("Info", "Do you really...?");
                ResultLabel.Text = string.Format("Selected button {0}", accepted ? "Accept" : "Cancel");
            });
        }

        public Task<bool> ShowAlert(string title, string message) {
            var tcs = new TaskCompletionSource<bool>();

            UIApplication.SharedApplication.InvokeOnMainThread(new NSAction(() =>
                {
                    UIAlertView alert = new UIAlertView(title, message, null,
                         NSBundle.MainBundle.LocalizedString("Cancel", "Cancel"),
                         NSBundle.MainBundle.LocalizedString("OK", "OK"));
                    alert.Clicked += (sender, buttonArgs) => tcs.SetResult(buttonArgs.ButtonIndex != alert.CancelButtonIndex);
                    alert.Show();
                }));

            return tcs.Task;
        }
    }
}

