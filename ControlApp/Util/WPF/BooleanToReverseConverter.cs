using System;
using Avalonia.Data.Converters;
using System.Globalization;
using Avalonia.Markup.Xaml;

using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;

namespace Nefarius.DsHidMini.ControlApp.Util.WPF
{
    public class BooleanToReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

         return !(bool?)value ?? true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         => !(value as bool?);
    }
}