using DeltaTrack.Models;
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace DeltaTrack.Helpers.Converters
{
    public class RouteToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new SolidColorBrush(RouteHelper.GetColorByRoute((CtaRoutes)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class RouteToFontColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new SolidColorBrush(RouteHelper.GetFontColorByRoute((CtaRoutes)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
