namespace SipebiPrototype.Core {
  /// <summary>
  /// The design-time data for a <see cref="FormalWordListItemViewModel"/>
  /// </summary>
  public class FormalWordListItemDesignModel : FormalWordListItemViewModel {
    #region Singleton
    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static FormalWordListItemDesignModel Instance { get; } = new FormalWordListItemDesignModel();
    #endregion

    #region Constructor
    /// <summary>
    /// Default constructor
    /// </summary>
    public FormalWordListItemDesignModel() {
      FormalWord = "abadiah";
      InformalWord = "abadiat";
      IsEditable = true;
    }
		#endregion
	}
}
