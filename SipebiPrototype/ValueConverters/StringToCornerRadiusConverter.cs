using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace SipebiPrototype {
  /// <summary>
  /// A converter that takes in a string and returns a <see cref="CornerRadius"/>
  /// </summary>
  public class StringToCornerRadiusConverter : BaseValueConverter<StringToCornerRadiusConverter> {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      CornerRadius cornerRadius = new CornerRadius(0);
      if (value == null || !(value is string))
        return cornerRadius;
      string par = (string)value;
      List<string> pars = par.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
      float result;
      if (pars.Any(x => !float.TryParse(x, out result))) //invalid
        return cornerRadius;
      if (pars.Count != 1 && pars.Count != 4) //invalid too
        return cornerRadius;
      if (pars.Count == 1) {
        result = float.Parse(pars[0]);
        cornerRadius.TopLeft = result;
        cornerRadius.TopRight = result;
        cornerRadius.BottomRight = result;
        cornerRadius.BottomLeft = result;
        return cornerRadius;
      }
      cornerRadius.TopLeft = float.Parse(pars[0]);
      cornerRadius.TopRight = float.Parse(pars[1]);
      cornerRadius.BottomRight = float.Parse(pars[2]);
      cornerRadius.BottomLeft = float.Parse(pars[3]);
      return cornerRadius;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

}
