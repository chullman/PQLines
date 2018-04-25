using PQLines.iOS.CustomRenderers;
using PQLines.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Themes;

[assembly: ExportRenderer(typeof (ThemedNavigationPage), typeof (ThemedNavigationPageRenderer))]

namespace PQLines.iOS.CustomRenderers
{
    public class ThemedNavigationPageRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);


            if (e.OldElement == null)
            {
                var nativeNavPage = e.NewElement as ThemedNavigationPage;
                if (nativeNavPage != null)
                {
                    ProlificTheme.Apply(UILabel.Appearance);
                    nativeNavPage.BarBackgroundColor = Color.FromRgba(0, 44, 119, 255);
r
                    nativeNavPage.BarTextColor = Color.White;
                }
            }
        }
    }
}