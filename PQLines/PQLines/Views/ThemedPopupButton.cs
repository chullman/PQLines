using Xamarin.Forms;

namespace PQLines.Views
{
    // Intercepted by custom renderer, ThemedPopupButtonRenderer.cs
    // Our "gray" popup buttons

    public class ThemedPopupButton : Button
    {
        public static Color ButtonBackgroundColor
        {
            get { return Color.White; }
        }

        public static Color ButtonTextColor
        {
            get { return Color.White; }
        }

        public static Color ButtonBorderColor
        {
            get { return Color.Black; }
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