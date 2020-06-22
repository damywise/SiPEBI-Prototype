using System.Collections.Generic;

namespace SipebiPrototype.Core {
	public class PH {
    /// <summary>
    /// The string for the software version of SiPEBI
    /// </summary>
    public static string SoftwareVersion { get; set; }

    /// <summary>
    /// The string for the default open file dialog directory
    /// </summary>
    public static string DefaultOpenFileDialogDirectory { get; set; }

    /// <summary>
    /// The string for the default save file dialog directory for corrected text
    /// </summary>
    public static string DefaultCorrectedTextSaveFileDialogDirectory { get; set; }

    /// <summary>
    /// The string for the default save file dialog directory for analysis result
    /// </summary>
    public static string DefaultAnalysisResultSaveFileDialogDirectory { get; set; }

    /// <summary>
    /// The string content for the latest opened file for working
    /// </summary>
    public static string LatestOpenedFileString { get; set; }

    /// <summary>
    /// The file name for the latest opened file for working
    /// </summary>
    public static string LatestOpenedFileName { get; set; }

    /// <summary>
    /// The list of default error codes in the <see cref="SipebiDiagnosticError"/>
    /// </summary>
    public static List<string> DefaultErrorCodes = new List<string> {
      DH.KbbiInformalWordDiagnosticErrorCode,
      DH.SipebiTiedWordDiagnosticErrorCode,
      DH.SipebiDefiniteCapitalWordDiagnosticErrorCode,
      DH.SipebiMarkNotAttachedDiagnosticErrorCode,
      DH.SipebiAmbiguousWordDiagnosticErrorCode,
      DH.SipebiPlaceWordDiagnosticErrorCode,
      DH.SipebiConjunctionSubordinativeWordDiagnosticErrorCode,
      DH.SipebiDetachedMarkDiagnosticErrorCode,
      DH.SipebiDetachedWordDiagnosticErrorCode,
    };
  }
}
