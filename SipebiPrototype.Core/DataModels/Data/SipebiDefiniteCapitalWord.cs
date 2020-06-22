namespace SipebiPrototype.Core {
  /// <summary>
  /// The class representation for words which have to be written in capital
  /// </summary>
	public class SipebiDefiniteCapitalWord {
    /// <summary>
    /// The word for this definite capital word, for instance "Januari"
    /// </summary>
    public string Word { get; set; }

    /// <summary>
    /// The source for this definite capital word
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// The flag to indicate if this definite capital word is editable
    /// </summary>
    public bool IsEditable { get; set; }
  }
}
