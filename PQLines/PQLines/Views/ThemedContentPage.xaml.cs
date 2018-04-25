using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PQLines.Views
{
    public partial class ThemedContentPage : ContentPage
    {
        private const string _backgroundPortrait = "BackgroundPortrait.png";
        private const string _backgroundLandscape = "BackgroundLandscape.png";

        public ThemedContentPage()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            // Swap the background image between BackgroundPortrait.png and BackgroundLandscape.png depending on what orientation the device is in
            // For iOS devices specifically, we will be relying on our custom renderer (see ThemedContentPageRenderer.cs) to handle this instead of this shared code
            // because annoyingly, iOS does not properly scale the background image to the dimensions of the screen
            // (note: WinPhone currently remains untested with this)
            Device.OnPlatform(
                iOS: null,
                Android: () => SetBackgroundImagePerOrientation(width, height),
                WinPhone: () => SetBackgroundImagePerOrientation(width, height)
            );
        }

        // Swap the background image depending on current device orientation
        private void SetBackgroundImagePerOrientation(double width, double height)
        {
            Debug.WriteLine("Forms: setting background image");
            BackgroundImage = width > height
                ? _backgroundLandscape
                : _backgroundPortrait;
        }

        public string GetPortraitBackgroundImage()
        {
            return _backgroundPortrait;
        }

        public string GetLandscapeBackgroundImage()
        {
            return _backgroundLandscape;
        }

        public Thickness ButtonGridPadding
        {
            get { return new Thickness(10, 0, 10, 0); }
        }

        public LayoutOptions ButtonGridVertOptions
        {
            get { return LayoutOptions.Center; }
        }

        public LayoutOptions ButtonGridHorzOptions
        {
            get { return LayoutOptions.Center; }
        }

        public double ButtonGridRowSpacing
        {
            get { return 10; }
        }

        public double ButtonGridColumnSpacing
        {
            get { return 10; }
        }

        public double ButtonGridRowDefHeight
        {
            get { return 60; }
        }

        public double ButtonGridColumnDefWidth
        {
            get { return 280; }
        }
    }
}