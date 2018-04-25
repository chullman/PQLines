using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.iOS;

namespace PQLines.iOS.CustomRenderers
{

    // iOS has an issue, whereby if the user rapidly presses the back button on the nav bar multiple times in quick succession, before the navigation animation completes, 
    // pages aren't popped correctly from the nav stack
    // As an easy fix, we will manually hide and unhide the back button when the page navigation animation starts and completes, respectively.

    // All page renderers (e.g. ThemedContentPageRenderer) should be using this class, instead of "Page Renderer"

    public class ContentPageRendererWithNavFix : PageRenderer
    {
        
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            ParentViewController.NavigationItem.SetHidesBackButton(true, false);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (ParentViewController.NavigationItem.HidesBackButton)
            {
                // Quite hacky - but need to place an artificial delay here to help prevent the page popping issue on quick navigation
                Thread.Sleep(500);

                ParentViewController.NavigationItem.SetHidesBackButton(false, false);
            }
        }
    }
}