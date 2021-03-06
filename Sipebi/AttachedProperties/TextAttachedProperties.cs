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
	/// Focuses (keyboard focus) this element on load
	/// </summary>
	public class IsFocusedProperty : BaseAttachedProperty<IsFocusedProperty, bool> {
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			//If we don't have a control, return
			if (!(sender is Control control))
				return;

			// Focus this control once loaded
			control.Loaded += (s, se) => control.Focus();
		}
	};
}
