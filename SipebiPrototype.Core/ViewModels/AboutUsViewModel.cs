namespace SipebiPrototype.Core {
  /// <summary>
  /// A view model for about us page
  /// </summary>
  public class AboutUsViewModel : BaseViewModel {
    /// <summary>
    /// The string containing the information of the version and the latest built time
    /// </summary>
    public string VersionAndBuiltTime { get { return PH.SoftwareVersion; } }
  }
}
