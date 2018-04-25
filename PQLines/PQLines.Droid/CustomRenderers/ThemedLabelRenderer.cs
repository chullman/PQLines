using Android.Graphics;
using PQLines.Droid.CustomRenderers;
using PQLines.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof (ThemedFontLabel), typeof (ThemedLabelRenderer))]

namespace PQLines.Droid.CustomRenderers
{
    public class ThemedLabelRenderer : LabelRenderer
    {
        // To set the buttons' text font for Android

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var nativeLabel = e.NewElement;
                if (nativeLabel != null)
                {
                    // With reference to: http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/fonts/
                    var label = Control;
                    var font = Typeface.CreateFromAsset(Forms.Context.Assets, "GillSans-BoldItalic.ttf"); // This ttf string probably shouldn't be hardcoded here...
                    label.Typeface = font;
                }
            }
        }
    }
}