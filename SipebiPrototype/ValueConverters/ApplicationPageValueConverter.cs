using System;
using System.Diagnostics;
using System.Globalization;
using SipebiPrototype.Core;

namespace SipebiPrototype {
	/// <summary>
	/// Converts the <see cref="ApplicationPage"/> to an actual view/page
	/// </summary>
	public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter> {
		public override object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null) {
			//Find the appropriate application page
			switch ((ApplicationPage)value) {
				case ApplicationPage.Login:
					return new LoginPage();
				case ApplicationPage.Work:
					return new WorkPage();
				case ApplicationPage.Register:
					return new RegisterPage();
        case ApplicationPage.FormalWord:
          return new FormalWordPage();
        case ApplicationPage.NotAvailable:
          return new NotAvailablePage();
        case ApplicationPage.NotAvailable2:
          return new NotAvailablePage2();
        case ApplicationPage.FormalWordKbbi:
          return new FormalWordKbbiPage();
        case ApplicationPage.AboutUs:
          return new AboutUsPage();
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
