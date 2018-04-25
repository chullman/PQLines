using Android.Graphics.Drawables;
using PQLines.Droid.CustomRenderers;
using PQLines.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof (ThemedNavigationButton), typeof (ThemedNavigationButtonRenderer))]

namespace PQLines.Droid.CustomRenderers
{
    public class ThemedNavigationButtonRenderer : ButtonRendererWithNavFix
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);


            if (e.OldElement == null)
            {
                var button = Control;

                // See ButtonRendererWithNavFix.cs
                CustomClickHandler(button);

                // Applies a "blue" gradient to our Android navigation buttons
                var gradientDrawable = new GradientDrawable();

                gradientDrawable.SetOrientation(GradientDrawable.Orientation.TlBr);

                gradientDrawable.SetColors(new int[] {Color.Argb(255, 20, 143, 194), Color.Argb(255, 0, 44, 119)});
                gradientDrawable.SetGradientType(GradientType.RadialGradient);
                gradientDrawable.SetGradientRadius(300);
                gradientDrawable.SetGradientCenter(0.5f, 0);
                button.SetBackgroundDrawable(gradientDrawable);


                // Sets a small drop shadow to the buttons' text
                button.SetShadowLayer(1, 1, 1, Color.Black);
            }
        }
    }
}