using System.Windows;

namespace SipebiPrototype {
  /// <summary>
  /// A base class to run any animation method when a boolean is set to true
  /// and reverse animation when set to false
  /// </summary>
  /// <typeparam name="Parent"></typeparam>
  public abstract class AnimateBaseProperty<Parent> : BaseAttachedProperty<Parent, bool>
    where Parent : BaseAttachedProperty<Parent, bool>, new() {

    #region public properties
    /// <summary>
    /// A flag indicating if this is the first time this property has been loaded
    /// </summary>
    public bool FirstLoad { get; set; } = true;
    #endregion

    public override void OnValueUpdated(DependencyObject sender, object value) {
      // Get the framework element
      if (!(sender is FrameworkElement element))
        return;

      //Don't fire if the value doesn't change, except on the first load, so we need to check if it is the first time it is run
      if (sender.GetValue(ValueProperty) == value && !FirstLoad)
        return;

      //On First load...
      if (FirstLoad) {
        //Creates a single self-unhookable event
        //for the elements loaded event
        RoutedEventHandler onLoaded = null; //this null initialization has to be put
        onLoaded = (ss, ee) => {
          //unhook ourselves
          element.Loaded -= onLoaded;

          //Do desired animation
          DoAnimation(element, (bool)value);

          //No longer in first load
          FirstLoad = false;
        };

        //Hook into the loaded event of the element
        element.Loaded += onLoaded;
      } else //else, just do the desired animation
        DoAnimation(element, (bool)value);
    }

    /// <summary>
    /// The animation method that is fired when the value changes
    /// </summary>
    /// <param name="element">the element</param>
    /// <param name="value">the new value</param>
    protected virtual void DoAnimation(FrameworkElement element, bool value) {

    }
  }

  /// <summary>
  /// Animates a framework element sliding it in from the left on show
  /// and sliding out to the left on hide
  /// </summary>
  public class AnimateSlideInFromLeftProperty : AnimateBaseProperty<AnimateSlideInFromLeftProperty> {
    protected override async void DoAnimation(FrameworkElement element, bool value) {
      if (value)
        //Animate in
        await element.SlideAndFadeInFromLeftAsync(FirstLoad ? 0 : 0.3f, keepMargin: false); //first load doesn't need animation
      else
        //Animate out
        await element.SlideAndFadeOutToLeftAsync(FirstLoad ? 0 : 0.3f, keepMargin: false);
    }
  }

  /// <summary>
  /// Animates a framework element sliding up from the bottom on show
  /// and sliding out to the bottom on hide
  /// </summary>
  public class AnimateSlideInFromBottomProperty : AnimateBaseProperty<AnimateSlideInFromBottomProperty> {
    protected override async void DoAnimation(FrameworkElement element, bool value) {
      if (value)
        //Animate in
        await element.SlideAndFadeInFromBottomAsync(FirstLoad ? 0 : 0.3f, keepMargin: false); //first load doesn't need animation
      else
        //Animate out
        await element.SlideAndFadeOutToBottomAsync(FirstLoad ? 0 : 0.3f, keepMargin: false);
    }
  }

  /// <summary>
  /// Animates a framework element fading in on show
  /// and fading out on hide
  /// </summary>
  public class AnimateFadeInProperty : AnimateBaseProperty<AnimateFadeInProperty> {
    protected override async void DoAnimation(FrameworkElement element, bool value) {
      if (value)
        // Animate in
        await element.FadeInAsync(FirstLoad ? 0 : 0.3f);
      else
        // Animate out
        await element.FadeOutAsync(FirstLoad ? 0 : 0.3f);
    }
  }
}
