namespace Sipebi.Core {
  /// <summary>
  /// The class to represent diagnostic report item in SiPEBI
  /// </summary>
	public class SipebiDiagnosticReportItem {
    /// <summary>
    /// The error code of this diagnostic report item
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// The (long) error name of this diagnostic report item
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    /// The number of errors related to this diagnostic report item
    /// </summary>
    public int NoOfErrors { get; set; }
  }
}
