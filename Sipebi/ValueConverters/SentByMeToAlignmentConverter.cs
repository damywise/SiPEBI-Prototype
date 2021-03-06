using System;
using System.Globalization;
using System.Windows;

namespace Sipebi {
	/// <summary>
	/// A converter that takes in a boolean if a message was sent by me, and aligns to the right.
	/// Otherwise, aligns to the left
	/// </summary>
	public class SentByMeToAlignmentConverter : BaseValueConverter<SentByMeToAlignmentConverter> {
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (parameter == null)
				return (bool)value ? HorizontalAlignment.Right : HorizontalAlignment.Left;
			else //if any parameter is passed, the behavior will be reversed
				return (bool)value ? HorizontalAlignment.Left : HorizontalAlignment.Right;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}

}
