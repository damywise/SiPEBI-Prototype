using System;
using System.Globalization;

namespace SipebiPrototype {
  /// <summary>
  /// A converter that takes in a date and converts it to a user friendly message read time
  /// </summary>
  public class MaxHeightConverter : BaseValueConverter<MaxHeightConverter> {
    public override object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null) {
      //double pctHeight = (double)parameter;
      int testHeight, heightReduction = 100; //default value;
      if(parameter != null) {
        string par = (string)parameter;
        bool result = int.TryParse(par, out testHeight);
        if (result)
          heightReduction = testHeight;
      }

      //if ((pctHeight <= 0.0) || (pctHeight > 100.0))
      //  throw new Exception("MaxHeightConverter expects parameter in the range (0,100]");

      return (double)value - heightReduction;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
