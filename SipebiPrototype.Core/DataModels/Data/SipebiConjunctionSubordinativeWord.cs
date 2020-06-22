namespace SipebiPrototype.Core {
  /// <summary>
  /// The class representation for conjunction subordinative words
  /// </summary>
	public class SipebiConjunctionSubordinativeWord {
    /// <summary>
    /// The word for this place word, for instance "sehingga"
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
