using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SipebiPrototype.Core;

namespace SipebiPrototype {
	/// <summary>
	/// Converts a string name to a service pulled from the IoC container
	/// </summary>
	public class IoCConverter : BaseValueConverter<IoCConverter> {
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			//Find the appropriate application page
			switch ((string)value) {
				case nameof(ApplicationViewModel):
					return IoC.AppVM;
        //return new LoginPage();
        //case nameof(WorkViewModel):
        //	return IoC.WorkVM;
        //case ApplicationPage.Register:
        //	return new RegisterPage();
        default:
					Debugger.Break();
					return null;
			}
		}

		//we do not need to convert back in this case
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
