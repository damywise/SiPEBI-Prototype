namespace Sipebi.Core {
  /// <summary>
  /// The class representation for words representing place
  /// </summary>
	public class SipebiPlaceWord {
    /// <summary>
    /// The word for this place word, for instance "tempat"
    /// </summary>
    public string Word { get; set; }

    /// <summary>
    /// The source for this place word
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// The flag to indicate if this place word is editable
    /// </summary>
    public bool IsEditable { get; set; }
  }
}
