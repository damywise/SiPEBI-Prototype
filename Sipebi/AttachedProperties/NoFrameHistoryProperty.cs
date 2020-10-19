using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;

namespace Sipebi {
	/// <summary>
	/// The NoFrameHistory attached property for creating a <see cref="Frame"/> that nevr shows navigation
	/// and keeps the navigation history empty
	/// </summary>
	public class NoFrameHistoryProperty : BaseAttachedProperty<NoFrameHistoryProperty, bool> {
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			// Get the frame
			var frame = (sender as Frame);

			// Hides navigation bar
			frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

			// Clear history on navigate
			frame.Navigated += (ss, ee) => ((Frame)ss).NavigationService.RemoveBackEntry();
		}
	};
}
