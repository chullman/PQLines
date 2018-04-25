using Xamarin.Forms;

namespace PQLines.Views
{
    // Intercepted by custom renderer, ThemedNavigationButtonRenderer.cs
    // Our "blue" navigation buttons

    public class ThemedNavigationButton : Button
    {
        public static Color ButtonBackgroundColor
        {
            get { return Color.Transparent; }
        }

        public static Color ButtonTextColor
        {
            get { return Color.White; }
        }

        public static double ButtonFontSize
        {
            get { return 16; }
        }

        public static FontAttributes ButtonFontAttributes
        {
            get { return FontAttributes.Bold; }
        }
    }
}