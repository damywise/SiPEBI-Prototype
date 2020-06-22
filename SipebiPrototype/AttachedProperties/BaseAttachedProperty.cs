using System;
using System.ComponentModel;
using System.Windows;

namespace SipebiPrototype {
	/// <summary>
	/// A base attached property to replace the Vanilla WPF attached property
	/// </summary>
	/// <typeparam name="Parent">The parent class to be attached property</typeparam>
	/// <typeparam name="Property">The type of this attached property (for example, boolean)</typeparam>
	public abstract class BaseAttachedProperty<Parent, Property>
		where Parent : new() {
		#region Public Events
		/// <summary>
		/// Fired when the value changes
		/// </summary>
		public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

		/// <summary>
		/// Fired when the value updates, even when the value is the same
		/// </summary>
		public event Action<DependencyObject, object> ValueUpdated = (sender, value) => { };
		#endregion

		#region Public Properties
		/// <summary>
		/// A singleton instance of our parent class
		/// </summary>
		public static Parent Instance { get; private set; } = new Parent();
		#endregion

		#region Attached Property Definitions
		/// <summary>
		/// The attached property for this class (always named "Value", but is ditinguished by the class name. Smart!)
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.RegisterAttached(
				"Value", 
				typeof(Property), 
				typeof(BaseAttachedProperty<Parent, Property>),
				new PropertyMetadata(
					default(Property),
					new PropertyChangedCallback(OnValuePropertyChanged),
					new CoerceValueCallback(OnValuePropertyUpdated) //to ensure the OnValuePropertyChanged will always be fired though it is the same value
					));

		/// <summary>
		/// The callback event when the <see cref="ValueProperty"/> is changed. This, unfortunately, cannot be changed because of its "static" type.
		/// Thus, internally, it is made to run two functions: OnValueChanged (parent could-be-overridden method) and ValueChanged (internal base/this method/action)
		/// </summary>
		/// <param name="d">The UI element that had it's property changed</param>
		/// <param name="e">The arguments for the event</param>
		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			//Call the parent function
			(Instance as BaseAttachedProperty<Parent, Property>)?.OnValueChanged(d, e); //this part is a ahead of the video 6...

			//Call event listeners
			(Instance as BaseAttachedProperty<Parent, Property>)?.ValueChanged(d, e); //this part is a ahead of the video 6...
		}

		/// <summary>
		/// The callback event when the <see cref="ValueProperty"/> is changed, even if it is the same value. 
		/// </summary>
		/// <param name="d">The UI element that had it's property changed</param>
		/// <param name="value">The arguments for the event</param>
		private static object OnValuePropertyUpdated(DependencyObject d, object value) {
			//Call the parent function
			(Instance as BaseAttachedProperty<Parent, Property>)?.OnValueUpdated(d, value); //this part is a ahead of the video 6...

			//Call event listeners
			(Instance as BaseAttachedProperty<Parent, Property>)?.ValueUpdated(d, value); //this part is a ahead of the video 6...

			//value not changed
			return value;
		}

		/// <summary>
		/// Get the attached property
		/// </summary>
		/// <param name="d">The element to get the property from</param>
		/// <returns></returns>
		public static Property GetValue(DependencyObject d) => (Property)d.GetValue(ValueProperty);

		/// <summary>
		/// Set the attached property
		/// </summary>
		/// <param name="d">The element to get the property from</param>
		/// <param name="value">The value to set the property to</param>
		public static void SetValue(DependencyObject d, Property value) => d.SetValue(ValueProperty, value);
		#endregion

		#region Event Methods
		/// <summary>
		/// The method that is called when any attached property of this type is changed
		/// </summary>
		/// <param name="sender">The UI element that this property was changed for</param>
		/// <param name="e">The arguments for this event</param>
		public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

		/// <summary>
		/// The method that is called when any attached property of this type is changed
		/// </summary>
		/// <param name="sender">The UI element that this property was changed for</param>
		/// <param name="value">The arguments for this event</param>
		public virtual void OnValueUpdated(DependencyObject sender, object value) { }
		#endregion
	}
}
