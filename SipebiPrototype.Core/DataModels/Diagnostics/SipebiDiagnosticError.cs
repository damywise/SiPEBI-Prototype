using KBBI.Core;
using System.Collections.Generic;
using Extension.Extractor;

namespace SipebiPrototype.Core {
  /// <summary>
  /// The class representation for the error shown in SiPEBI
  /// </summary>
	public class SipebiDiagnosticError {
    /// <summary>
    /// The code for this error, by the code more explanation about this error is supposed to be obtained
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// The property to indicate if this element has error result
    /// </summary>
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorCode);

    /// <summary>
    /// The paragraph no where the original word division is located
    /// </summary>
    public int ParagraphNo => OriginalWordDivision == null ? -1 : OriginalWordDivision.ParagraphOffset;

    /// <summary>
    /// The element no where the original word division is located within the paragraph
    /// </summary>
    public int ElementNo => OriginalWordDivision == null ? -1 : OriginalWordDivision.ElementNo;

    /// <summary>
    /// String containing the original element before correction, to be assigned during the diagnosis
    /// </summary>
    public string OriginalElement { get; set; }

    /// <summary>
    /// String containing the corrected element after correction, to be assigned during the diagnosis
    /// </summary>
    public string CorrectedElement { get; set; }

    /// <summary>
    /// The original word division element related to this error
    /// </summary>
    public WordDivision OriginalWordDivision { get; set; }

    /// <summary>
    /// The original character position for this errorneous element, obtained from the <see cref="OriginalWordDivision"/>
    /// </summary>
    public int OriginalCharPosition => OriginalWordDivision == null ? -1 : OriginalWordDivision.CharPositionOffset;

    /// <summary>
    /// The character position after correction. Must be assigned after correction is made
    /// </summary>
    public int CorrectedCharPosition { get; set; }

    /// <summary>
    /// Flag to indicate if this error is ambiguous. True if this element, instead of an error, is an ambiguity
    /// </summary>
    public bool IsAmbiguous { get; set; }

    /// <summary>
    /// List to indicate the possible solutions for ambiguity
    /// </summary>
    public List<string> AmbiguousSolutions { get; set; }

    //corrected word TODO may not be available
    ///// <summary>
    ///// The corrected word division element related to this error
    ///// </summary>
    //public WordDivision CorrectedWordDivision { get; set; }
  }
}
