using System;
using System.Windows;
using System.Windows.Controls;

namespace SipebiPrototype {
  public class PasswordBoxProperties {
    //we pass a callback in the metadata here to listen to the password changed event
    public static readonly DependencyProperty MonitorPasswordProperty =
  DependencyProperty.RegisterAttached("MonitorPassword", typeof(bool), typeof(PasswordBoxProperties), new PropertyMetadata(false, OnMonitorPasswordChanged));

    //it will fire whenever a property is attached
    private static void OnMonitorPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      var passwordBox = (d as PasswordBox);
      if (passwordBox == null) return; //we can do nothing if there isn't password box

      //remove previous event
      passwordBox.PasswordChanged -= PasswordBox_PasswordChanged; //so that we won't add multiple events

      if ((bool)e.NewValue) { //if the value changed
        SetHasText(passwordBox); //then the hasText value will be changed accordingly
        passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
      }
    }

    //So now, this is the event
    private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) {
      //every time the password box changes, we also want to update the HasText property
      SetHasText((PasswordBox)sender);
    }

    //Not that unlike the Getter and Setter in the HasText, Getter and Setter accessor for the SetMonitor are "public"
    //This is to ensure the XAML code can access this attachedproperty
    public static void SetMonitorPassword(PasswordBox element, bool value) {
      element.SetValue(MonitorPasswordProperty, value);
    }

    public static bool GetMonitorPassword(PasswordBox element) {
      return (bool)element.GetValue(MonitorPasswordProperty);
    }

    //The item below is what we actually want to do - that is, a basic getter and setter where MVVM understands, but what a long-winded way to do it!
    //public bool HasText { get; set; } = false;
    public static readonly DependencyProperty HasTextProperty = 
      DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(PasswordBoxProperties), new PropertyMetadata(false));

    private static void SetHasText(PasswordBox element) {
      element.SetValue(HasTextProperty, element.SecurePassword.Length > 0); //this is where we "attach" the property to the PasswordBox!
    }

    private static bool GetHasText(PasswordBox element) {
      return (bool)element.GetValue(HasTextProperty);
    }
    //Now we need to attach this property in the PasswordBox we use!
  }
}
