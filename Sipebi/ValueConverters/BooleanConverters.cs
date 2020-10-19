using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Sipebi {
  /// <summary>
  /// A converter that takes in a boolean and returns a <see cref="Visibility"/>. True means "Hidden" and False means "Visible".
  /// That is strange, at least up to video 6...
  /// </summary>
  public class BooleanToVisibilityConverter : BaseValueConverter<BooleanToVisibilityConverter> {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return parameter == null ?
        ((bool)value ? Visibility.Hidden : Visibility.Visible) :
        ((bool)value ? Visibility.Visible : Visibility.Hidden);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  /// <summary>
  /// A boolean to visibility converter which make more sense. True means visible if parameter is not given.
  /// </summary>
  public class GoodBooleanToVisibilityConverter : BaseValueConverter<GoodBooleanToVisibilityConverter> {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      bool val = (bool)value;
      if (parameter == null) //default behavior
        return val ? Visibility.Visible : Visibility.Hidden; //if the value is true it means visible      
      string par = ((string)parameter).ToLower().Trim();
      switch (par) {
        case "inverse":
          return val ? Visibility.Hidden : Visibility.Visible;
        case "collapsed":
          return val ? Visibility.Visible : Visibility.Collapsed;
        case "inverse-collapsed":
          return val ? Visibility.Collapsed : Visibility.Visible;
      }
      return Visibility.Visible;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }


  /// <summary>
  /// A boolean of black vs other color converter. When value is false, it is black. Otherwise, create color from the parameter
  /// </summary>
  public class BooleanToColorConverter : BaseValueConverter<BooleanToColorConverter> {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      bool val = (bool)value;
      if (parameter == null || !val)
        return Application.Current.FindResource("WordDeeperRedBrush"); //by default, it is a red brush
      //return new SolidColorBrush((Color)(new ColorConverter().ConvertFrom($"#000000"))); //by default, it is black
      return (SolidColorBrush)parameter;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }


  public class BooleanToVersionConverter : BaseValueConverter<BooleanToVersionConverter>{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      => "Versi: " + ((bool)value ? "Pengguna" : "Editor");
    
    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
