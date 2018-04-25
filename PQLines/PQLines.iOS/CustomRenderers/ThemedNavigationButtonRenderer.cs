using System.Drawing;
using CoreGraphics;
using PQLines.iOS.CustomRenderers;
using PQLines.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Themes;

[assembly: ExportRenderer(typeof (ThemedNavigationButton), typeof (ThemedNavigationButtonRenderer))]

namespace PQLines.iOS.CustomRenderers
{
    public class ThemedNavigationButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            var button = Control;


            if (e.OldElement == null)
            {
                // Sets the buttons' blue gradient by using the "Prolific Theme" https://components.xamarin.com/view/prolifictheme
                button.SetBackgroundImage(ProlificTheme.SharedTheme.RegularButtomImage, UIControlState.Normal);

                // Sets the buttons' text drop shadow
                button.TitleLabel.Layer.ShadowColor = UIColor.Black.CGColor;
                button.TitleLabel.Layer.ShadowRadius = 1.0f;
                button.TitleLabel.Layer.ShadowOpacity = 1.0f;
                button.TitleLabel.Layer.ShadowOffset = CGSize.Empty;

                // Sets the buttons' drop shadow
                button.Layer.CornerRadius = 0.5f;
                button.Layer.ShadowColor = UIColor.Black.CGColor;
                button.Layer.ShadowOpacity = 1.0f;
                button.Layer.ShadowRadius = 5.0f;
                button.Layer.ShadowOffset = new SizeF(5f, 5f);
            }
        }
    }
}