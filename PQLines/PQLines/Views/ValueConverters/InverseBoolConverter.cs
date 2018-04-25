using System;
using System.Globalization;
using Xamarin.Forms;

namespace PQLines.Views.ValueConverters
{
    public class InverseBoolConverter : IValueConverter
    {
        // Currently used by properties in HomePage to do the inverse of a specific property's value
        // I.e. If the ActivityIndicator overlay (loading icon) is visible, hide the controls underneath.

        // Source: https://forums.xamarin.com/discussion/36714/how-to-in-a-binding-in-xaml

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }
    }
}