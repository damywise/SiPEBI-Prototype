namespace SipebiPrototype.Core {
	public class FormalWordItem {
    /// <summary>
    /// The informal word to be corrected
    /// </summary>
		public string InformalWord { get; set; }

    /// <summary>
    /// The correction for the <see cref="InformalWord"/>
    /// </summary>
		public string FormalWord { get; set; }

    /// <summary>
    /// The source of this formal word item
    /// NOTE: source from KBBI is unique
    /// </summary>
		public string Source { get; set; }

    /// <summary>
    /// The string to indicate the error code used for diagnostic purpose
    /// </summary>
    public string DiagnosticErrorCode { get; set; }

    /// <summary>
    /// The flag to indicate if this item is editable.
    /// NOTE: item taken directly from KBBI is not editable.
    /// NOTE: must be designed this way to support automatic SQL database item convertion to .Net data type
    /// </summary>
    public long IsEditable { get; set; }

    /// <summary>
    /// The function to indicate if this item is editable.
    /// </summary>
    public bool Editable() => IsEditable != 0;
  
    /// <summary>
    /// Function to check if two formal word item is equal
    /// </summary>
    public bool IsEqualItem(FormalWordItem item, bool sourceChecked) {
      if (sourceChecked)
        return item.InformalWord.Equals(InformalWord) && item.Source.Equals(Source);
      return item.InformalWord.Equals(InformalWord);
    }
	}
}
