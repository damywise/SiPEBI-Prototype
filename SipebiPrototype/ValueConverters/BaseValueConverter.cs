using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace SipebiPrototype {
  /// <summary>
  /// A base value converter that allows direct XAML usage
  /// </summary>
  /// <typeparam name="T">The type of this value converter</typeparam>
  //https://stackoverflow.com/questions/4737970/what-does-where-t-class-new-mean
  //That is a constraint on the generic parameter T. It must be a class (reference type) and must have a public parameter-less default constructor.
  public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter 
    where T : class, new() { //new() is needed so that T can be made with [new T()]

    #region Private members
    /// <summary>
    /// A single static instance of this value converter
    /// </summary>
    private static T Converter = null;
    #endregion

    #region Markup Extension Methods
    /// <summary>
    /// Provides a static instance of the value converter
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider) {
      //When provide value is called, either takes from the single static class if it exists or create a new one if it does not
      return Converter ?? (Converter = new T());
    }
    #endregion

    #region Value Converter Methods

    /// <summary>
    /// The method to convert one type to another
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    /// <summary>
    /// The method to convert a value back to it's source type
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    #endregion
  }
}
