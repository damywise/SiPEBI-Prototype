using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace SipebiPrototype {
  /// <summary>
  /// A converter that takes in an RGB string such as FF00FF and converts it to a WPF color.
  /// </summary>
  public class StringRGBToColorConverter : BaseValueConverter<StringRGBToColorConverter> {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      string val = (string)value;
      if (val.StartsWith("#"))
        return (Color)(new ColorConverter().ConvertFrom($"{value}"));
      return (Color)(new ColorConverter().ConvertFrom($"#{value}"));
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

}
