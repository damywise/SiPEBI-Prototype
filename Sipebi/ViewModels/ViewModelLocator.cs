using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sipebi.Core;

namespace Sipebi {
	/// <summary>
	/// Locates view modesl from the IoC for use in binding in XAML files
	/// </summary>
	public class ViewModelLocator {
		#region public properties
		/// <summary>
		/// Singleton instance of the locator
		/// </summary>
		public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

		/// <summary>
		/// The application view model
		/// </summary>
		public static ApplicationViewModel ApplicationViewModel => IoC.AppVM;
		#endregion
	}
}
