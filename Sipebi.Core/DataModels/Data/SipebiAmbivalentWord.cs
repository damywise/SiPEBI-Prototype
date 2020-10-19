using System.Collections.Generic;

namespace Sipebi.Core {
  /// <summary>
  /// The class representation for ambiguous words
  /// </summary>
	public class SipebiAmbiguousWord {
    /// <summary>
    /// The word for this ambiguous word, for instance "menyucikan"
    /// </summary>
    public string Word { get; set; }

    /// <summary>
    /// The list of words which can be the possible corrections for this ambiguous word
    /// </summary>
    public List<string> AlternativeWords { get; set; }

    /// <summary>
    /// The source for this ambiguous word
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// The flag to indicate if this ambiguous word is editable
    /// </summary>
    public bool IsEditable { get; set; }
  }
}
