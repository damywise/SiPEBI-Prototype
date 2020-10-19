namespace Sipebi.Core {
  /// <summary>
  /// The design-time data for a <see cref="DiagnosticListItemViewModel"/>
  /// </summary>
  public class DiagnosticListItemDesignModel : DiagnosticListItemViewModel {
    #region Singleton
    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static DiagnosticListItemDesignModel Instance { get; } = new DiagnosticListItemDesignModel();
    #endregion

    #region Constructor
    /// <summary>
    /// Default constructor
    /// </summary>
    public DiagnosticListItemDesignModel() {
      DiagnosticNo = "2222";
      ParagraphNo = "3333";
      ElementNo = "145";
      ErrorCode = "[00001]";
      Error = "Error Long Long Long Long Name 00001";
      ErrorExplanation = "Explanation Long Long Long Long Long Long Long Long Long 00001";
      OriginalElement = "Original Long Long Long Long Long Long 00001";
      CorrectedElement = "Corrected Long Long Long Long Long Long 00001";
      //IsHeader = true;
    }
    #endregion
  }
}
