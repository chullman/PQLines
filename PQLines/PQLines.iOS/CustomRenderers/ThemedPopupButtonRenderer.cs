using System.Drawing;
using CoreGraphics;
using PQLines.iOS.CustomRenderers;
using PQLines.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Themes;

[assembly: ExportRenderer(typeof (ThemedPopupButton), typeof (ThemedPopupButtonRenderer))]

namespace PQLines.iOS.CustomRenderers
{
    public class ThemedPopupButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            var button = Control;

            if (e.OldElement == null)
            {
                // Sets the buttons' gray gradient by using the "Prolific Theme" https://components.xamarin.com/view/prolifictheme and applying a gray tint over it
                button.SetBackgroundImage(
                    TintImage(ProlificTheme.SharedTheme.RegularButtomImage, UIColor.Gray.CGColor), UIControlState.Normal);

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

        // To apply a gray color tint over the default ProlificTheme.SharedTheme.RegularButtomImage blue color
        // With reference to: http://stackoverflow.com/a/7377827
        private UIImage TintImage(UIImage imageName, CGColor tintColor)
        {
            var originalImage = imageName;
            UIImage coloredImage;

            UIGraphics.BeginImageContext(originalImage.Size);
            using (var context = UIGraphics.GetCurrentContext())
            {
                context.TranslateCTM(0, originalImage.Size.Height);
                context.ScaleCTM(1.0f, -1.0f);

                var rect = new CGRect(0, 0, originalImage.Size.Width, originalImage.Size.Height);

                // Draw black background to preserve color of transparent pixels
                context.SetBlendMode(CGBlendMode.Normal);
                context.SetFillColor(UIColor.Black.CGColor);
                context.FillRect(rect);

                // Draw original image
                context.SetBlendMode(CGBlendMode.Normal);
                context.DrawImage(rect, originalImage.CGImage);

                // Tint image (removes any alpha) - the original image luminosity is preserved
                context.SetBlendMode(CGBlendMode.Color);
                context.SetFillColor(tintColor);
                context.FillRect(rect);

                // Mask by alpha values of original image
                context.SetBlendMode(CGBlendMode.DestinationIn);
                context.DrawImage(rect, originalImage.CGImage);
            }

            coloredImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return coloredImage;
        }
    }
}