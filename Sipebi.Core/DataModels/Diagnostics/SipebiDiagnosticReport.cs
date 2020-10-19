using System;
using System.Collections.Generic;
using System.Linq;

namespace Sipebi.Core {
  /// <summary>
  /// The class to represent diagnostic report in SiPEBI
  /// </summary>
	public class SipebiDiagnosticReport {
    /// <summary>
    /// List of <see cref="SipebiDiagnosticReportItem"/> associated with this report
    /// </summary>
    public List<SipebiDiagnosticReportItem> Items { get; set; }

    /// <summary>
    /// List of <see cref="SipebiDiagnosticError"/> associated with this report
    /// </summary>
    public List<SipebiDiagnosticError> Errors { get; set; } = new List<SipebiDiagnosticError>();

    /// <summary>
    /// The original text before correction
    /// </summary>
    public string OriginalText { get; set; }

    /// <summary>
    /// The corrected text
    /// </summary>
    public string CorrectedText { get; set; }

    /// <summary>
    /// The length of the original text before correction
    /// </summary>
    public int OriginalTextLength => OriginalText == null ? 0 : OriginalText.Length;

    /// <summary>
    /// The length of the corrected text
    /// </summary>
    public int CorrectedTextLength => CorrectedText == null ? 0 : CorrectedText.Length;

    /// <summary>
    /// The number of paragraphs corrected
    /// </summary>
    public int NoOfParagraphs { get; set; }

    /// <summary>
    /// The number of elements corrected
    /// </summary>
    public int NoOfElements { get; set; }

    /// <summary>
    /// The start time of the correction
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// The end time of the correction
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// The execution time of the correction
    /// </summary>
    public TimeSpan ExecutionTime => EndTime - StartTime;

    /// <summary>
    /// The number of correction contained in this report
    /// </summary>
    public int NoOfCorrection => HasError ? Errors.Count : 0;

    /// <summary>
    /// The number of ambiguity obtained by this report
    /// </summary>
    public int NoOfAmbiguous => HasError ? Errors.Count(x => x.ErrorCode == DH.SipebiAmbiguousWordDiagnosticErrorCode) : 0;

    /// <summary>
    /// Flag to indicate if this report item has an error in the report
    /// </summary>
    public bool HasError => Items != null && Errors != null && Items.Count > 0 && Errors.Count > 0;

    /// <summary>
    /// Flag to indicate if this report has a valid report (procedure) being run (though maybe no error is found)
    /// </summary>
    public bool HasReport => !string.IsNullOrWhiteSpace(CorrectedText);

    /// <summary>
    /// Default constructor
    /// </summary>
    public SipebiDiagnosticReport() { }

    /// <summary>
    /// The function to create report from the given <see cref="List{T}"/>(s) of <see cref="SipebiDiagnosticError"/> and <see cref="SipebiDiagnosticErrorInformation"/>
    /// </summary>
    /// <param name="errors">The <see cref="List{T}"/> of <see cref="SipebiDiagnosticError"/></param>
    /// <param name="errorInformation">The <see cref="List{T}"/> of <see cref="SipebiDiagnosticErrorInformation"/></param>
    /// <returns>True if the report creation is successful</returns>
    public bool CreateReportItems(List<SipebiDiagnosticError> errors, List<SipebiDiagnosticErrorInformation> errorInformation) {
      Errors = errors;
      if (errors == null) {
        Items?.Clear();
        Items = null;
        return false;
      }
      if (Items == null)
        Items = new List<SipebiDiagnosticReportItem>();
      var errorGroups = errors.GroupBy(x => x.ErrorCode);
      foreach(var errorGroup in errorGroups) {
        SipebiDiagnosticErrorInformation ei = errorInformation.FirstOrDefault(x => x.ErrorCode == errorGroup.Key);
        if (ei == null)
          continue;
        SipebiDiagnosticReportItem item = new SipebiDiagnosticReportItem {
          ErrorCode = errorGroup.Key,
          Error = ei.Error,
          NoOfErrors = errorGroup.Count(),
        };
        Items.Add(item);
      }
      return true;
    }
  }
}
