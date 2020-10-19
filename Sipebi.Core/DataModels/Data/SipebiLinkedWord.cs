namespace Sipebi.Core {
  /// <summary>
  /// The class representation for words which can be linked to other words
  /// </summary>
	public class SipebiLinkedWord {
    /// <summary>
    /// The word for this linked word, for instance "di"
    /// </summary>
    public string Word { get; set; }

    /// <summary>
    /// The source for this linked word
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// The flag to indicate if this linked word is editable
    /// </summary>
    public bool IsEditable { get; set; }
  }
}
