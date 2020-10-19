namespace Sipebi {
  /// <summary>
  /// The IsBusy attached property for anything that wants to flag if control is busy
  /// </summary>
  public class IsBusyProperty : BaseAttachedProperty<IsBusyProperty, bool> { };

  /// <summary>
  /// The CornerRadius attached property for anything that wants to control its corner radius
  /// </summary>
  public class CornerRadiusProperty : BaseAttachedProperty<CornerRadiusProperty, string> { };

  /// <summary>
  /// The color property for anything that wants to have some controlled original color (such as for choosing animation color's purpose)
  /// </summary>
  public class OriginalColorProperty : BaseAttachedProperty<OriginalColorProperty, string> { };

  /// <summary>
  /// The color property for anything that wants to have some controlled changed color (such as for choosing animation color's purpose)
  /// </summary>
  public class ChangedColorProperty : BaseAttachedProperty<ChangedColorProperty, string> { };
}
