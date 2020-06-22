﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace SipebiPrototype {
  /// <summary>
  /// A converter that takes in an RGB string such as FF00FF and converts it to a WPF brush.
  /// That is strange, at least up to video 6...
  /// </summary>
  public class StringRGBToBrushConverter : BaseValueConverter<StringRGBToBrushConverter> {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return (SolidColorBrush)(new BrushConverter().ConvertFrom($"#{value}"));
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

}
