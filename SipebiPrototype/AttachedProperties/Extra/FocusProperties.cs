﻿using System.Windows;

namespace SipebiPrototype {
  /// <summary>
  /// The attached property to control a UI to get focused when flagged to
  /// </summary>
  public class GetFocusedProperty : BaseAttachedProperty<GetFocusedProperty, bool> {
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
      var uie = (UIElement)sender;
      if ((bool)e.NewValue) {
        uie.Focus(); // Don't care about false values.
      }
    }
  };

  ////Example taken from https://stackoverflow.com/questions/1356045/set-focus-on-textbox-in-wpf-from-view-model-c
  ////Credit: https://stackoverflow.com/users/125351/anvaka
  //public static class FocusExtension {
  //  public static bool GetIsFocused(DependencyObject obj) {
  //    return (bool)obj.GetValue(IsFocusedProperty);
  //  }

  //  public static void SetIsFocused(DependencyObject obj, bool value) {
  //    obj.SetValue(IsFocusedProperty, value);
  //  }

  //  public static readonly DependencyProperty IsFocusedProperty =
  //      DependencyProperty.RegisterAttached(
  //          "IsFocused", typeof(bool), typeof(FocusExtension),
  //          new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

  //  private static void OnIsFocusedPropertyChanged(
  //      DependencyObject d,
  //      DependencyPropertyChangedEventArgs e) {
  //    var uie = (UIElement)d;
  //    if ((bool)e.NewValue) {
  //      uie.Focus(); // Don't care about false values.
  //    }
  //  }
  //}
}
