using System;
using System.Diagnostics;
using CoreGraphics;
using Foundation;
using PQLines.iOS.CustomRenderers;
using PQLines.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof (ThemedContentPage), typeof (ThemedContentPageRenderer))]

namespace PQLines.iOS.CustomRenderers
{
    // Intercepts ThemedContentPage to apply a renderer to it

    // This custom renderer implementation is currently used to correctly scale the background image (currently BackgroundPortrait.png, or BackgroundLandscape.png)
    // to the dimensions of the screen (or more specifically, the dimensions of the page)

    public class ThemedContentPageRenderer : ContentPageRendererWithNavFix
    {
        private ThemedContentPage _nativeContPage;

        private UIInterfaceOrientation _currentDeviceOrientation;

        private CGSize _CGSizePortrait;
        private CGSize _CGSizeLandscape;

        // To correctly re-scale the background image as soon as the content page has loaded/changed
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
           
            if (e.OldElement == null)
            {

                _nativeContPage = e.NewElement as ThemedContentPage;

                if (_nativeContPage == null) 
                    return;

                CorrectlyScaleBackgroundImageToScreen();

            }
        }

        // To correctly re-scale the background image after the device's orientation has changed, but previous pages in the navigation stack have their background
        // image still set to a different orientation
        // (i.e. used to rescale an image after a PopAsync call)
        public override void ViewDidAppear(bool animated)
        {
            CorrectlyScaleBackgroundImageToScreen();

            base.ViewDidAppear(animated);
        }
        

        // To correctly re-scale the background image after the device's orientation has changed
        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);

            CorrectlyScaleBackgroundImageToScreen();
        }

        // The method to conduct the image rescaling (currently either BackgroundPortrait.png or BackgroundLandscape.png) to the iOS device's screen size
        // With reference to: https://forums.xamarin.com/discussion/comment/129041/#Comment_129041
        private void ScaleImageToDimensions(string image, CGSize size)
        {
            
            if (!(String.IsNullOrEmpty(image)))
            {
                try
                {
                    UIGraphics.BeginImageContext(View.Frame.Size);
                    var imageToScale = UIImage.FromFile(image);
                    //imageToScale = imageToScale.Scale(View.Frame.Size);
                    imageToScale = imageToScale.Scale(size);
                    View.BackgroundColor = UIColor.FromPatternImage(imageToScale);
                }
                catch (Exception)
                {
                    Debug.WriteLine("ERROR: Image specified is unable to be scaled to view frame");
                    throw;
                }
            }
            else
            {
                throw new Exception("ERROR: No image specified to attempt to scale");
            }

        }

        private void CorrectlyScaleBackgroundImageToScreen()
        {
            // Probably not the best approach, but we will use the current orientation of the status bar to determine the entire page's orientation
            _currentDeviceOrientation = UIApplication.SharedApplication.StatusBarOrientation;

            if ((_currentDeviceOrientation == UIInterfaceOrientation.LandscapeLeft) || (_currentDeviceOrientation == UIInterfaceOrientation.LandscapeRight))
            {
                if (_CGSizeLandscape.IsEmpty)
                {
                    _CGSizeLandscape = View.Frame.Size;
                }
                try
                {
                    ScaleImageToDimensions(_nativeContPage.GetLandscapeBackgroundImage(), _CGSizeLandscape);
                }
                catch (Exception)
                {
                    Debug.WriteLine("ERROR: Unable to scale specified landscape image to specified size - check the arguments passed");
                    throw;
                }
                
            }
            else
            {
                if (_CGSizePortrait.IsEmpty)
                {
                    _CGSizePortrait = View.Frame.Size;
                }
                try
                {
                    ScaleImageToDimensions(_nativeContPage.GetPortraitBackgroundImage(), _CGSizePortrait);
                }
                catch (Exception)
                {
                    Debug.WriteLine("ERROR: Unable to scale specified portrait image to specified size - check the arguments passed");
                    throw;
                }
                
            }
        }

        
    }
}