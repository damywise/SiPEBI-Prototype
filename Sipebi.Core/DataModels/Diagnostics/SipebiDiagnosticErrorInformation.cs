namespace Sipebi.Core {
  /// <summary>
  /// The class representation for the error information shown in SiPEBI
  /// </summary>
	public class SipebiDiagnosticErrorInformation {
    /// <summary>
    /// The code for this error
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// The (long) name of this error
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    /// The explanation correspond to this error
    /// </summary>
    public string ErrorExplanation { get; set; }

    /// <summary>
    /// The indicator on which SiPEBI version the error started to get corrected
    /// </summary>
    public string AppearOnVersion { get; set; }
  }
}
