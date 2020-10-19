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
  /// The MonitorPassword attached property for a <see cref="PasswordBox"/>
  /// </summary>
  public class MonitorPasswordProperty : BaseAttachedProperty<MonitorPasswordProperty, bool> {
    //This is to access the actual "OnPropertyValueChanged" callback event needed when the property is changed
    //The base "OnPropertyValueChanged" must be readonly static by Vanilla WPF
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
      var passwordBox = (sender as PasswordBox);
      if (passwordBox == null) return; //we can do nothing if there isn't password box (or if it isn't password box)

      //remove previous event, no matter whether the property is set to true or false, the previous event must be removed
      //It doesn't matter if there isn't previous event to be removed, it won't break.
      passwordBox.PasswordChanged -= PasswordBox_PasswordChanged; //so that we won't add multiple events

      //if the caller set the monitor password to true, then listening to the event
      //Otherwise, we don't listen to the PasswordChanged event
      if ((bool)e.NewValue) { 
        HasTextProperty.SetValue(passwordBox); //then the hasText value will be changed accordingly
        passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
      }
    }

    //So now, this is the event
    /// <summary>
    /// Fired when the password box's password value changes
    /// </summary>
    /// <param name="sender">The <see cref="PasswordBox"/> object</param>
    /// <param name="e"></param>
    private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) {
      //every time the password box changes, we also want to update the HasText property
      //Set the attached HasText value
      HasTextProperty.SetValue((PasswordBox)sender);
    }
  };

  /// <summary>
  /// The HasText attached property for a <see cref="PasswordBox"/>
  /// </summary>
  public class HasTextProperty : BaseAttachedProperty<HasTextProperty, bool> {
    /// <summary>
    /// Set the HasText property based on if the caller <see cref="PasswordBox"/> has any text
    /// </summary>
    /// <param name="sender">The <see cref="PasswordBox"/> object</param>
    public static void SetValue(DependencyObject sender) {
      SetValue(sender, (sender as PasswordBox)?.SecurePassword.Length > 0); //This SetValue is the [base] SetValue
    }
  };
}
