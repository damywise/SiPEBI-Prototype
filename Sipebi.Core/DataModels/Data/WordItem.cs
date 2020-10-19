using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Extension.String;

namespace Sipebi.Core {
	public class WordItem {
    /// <summary>
    /// The Word Id is used to identically indicate a <see cref="WordItem"/> from the other
    /// </summary>
		public long WordId { get; set; }

    /// <summary>
    /// The word text associated with the <see cref="WordId"/>
    /// </summary>
		public string Word { get; set; }

    /// <summary>
    /// The source of this word item.
    /// NOTE: source from KBBI is unique
    /// </summary>
		public string Source { get; set; }

    /// <summary>
    /// The correction word for the word. This property is not used by all words, 
    /// but only words this correction word is applied to
    /// </summary>
    public string CorrectionWord { get; set; }

    /// <summary>
    /// The flag to indicate if this item is editable.
    /// NOTE: item taken directly from KBBI is not editable.
    /// NOTE: must be designed this way to support automatic SQL database item convertion to .Net data type
    /// </summary>
    public long IsEditable { get; set; }

    /// <summary>
    /// The other properties attached to this word. Will be used to make correction when necessary
    /// </summary>
    public string WordProperties { get; set; }

    private const char wordPropertiesSeparator = ';';

    /// <summary>
    /// The method to return trimmed, non-empty properties of this <see cref="WordItem"/>.
    /// It will return empty list when the <see cref="WordProperties"/> is null or whitespaces.
    /// We use ; instead of , to split the word properties to avoid overlapping symbols with CSV
    /// </summary>
    /// <returns></returns>
    public List<string> GetWordProperties() => string.IsNullOrWhiteSpace(WordProperties) ?
      new List<string>() : WordProperties.GetTrimmedNonEmptyParts(wordPropertiesSeparator);

    /// <summary>
    /// Function to check if the given item is completely equal to this item, including its word properties. 
    /// This will return true if everything but <see cref="WordId"/> or <see cref="IsEditable"> of both items are equal
    /// </summary>
    /// <param name="item">The item to compare this item with</param>
    /// <returns>The checking result</returns>
    public bool IsFullyEqualItem(WordItem item) {
      if (!IsEqualWord(item)) return false; //at this point, everything else is already equal except its properties
      if (string.IsNullOrWhiteSpace(item.WordProperties) && string.IsNullOrWhiteSpace(WordProperties))
        return true;
      if ((!string.IsNullOrWhiteSpace(item.WordProperties) && string.IsNullOrWhiteSpace(WordProperties)) ||
        (string.IsNullOrWhiteSpace(item.WordProperties) && !string.IsNullOrWhiteSpace(WordProperties)))
        return false;
      List<string> wps1 = GetWordProperties();
      List<string> wps2 = item.GetWordProperties();
      return wps1.Count > wps2.Count ? wps1.Except(wps2).Count() == 0 
        : wps1.Except(wps2).Count() == 0; 
    }

    /// <summary>
    /// Function to check if the given item is completely equal to this item, including its word properties. 
    /// This will return true if everything but <see cref="WordId"/> or <see cref="IsEditable"> 
    /// or <see cref="WordProperties"/> of both items are equal
    /// </summary>
    /// <param name="item">The item to compare this item with</param>
    /// <returns>The checking result</returns>
    public bool IsEqualWord(WordItem item) => 
      StringHelper.EqualsWhenNullOrWhiteSpace(Word, item.Word) &&
      StringHelper.EqualsWhenNullOrWhiteSpace(Source, item.Source) &&
      StringHelper.EqualsWhenNullOrWhiteSpace(CorrectionWord, item.CorrectionWord);

    /// <summary>
    /// Function to convert this object into a CSV string
    /// </summary>
    public string ToCsvString() {
      StringBuilder sb = new StringBuilder();
      CsvHelper.AddCsvElement(sb, WordId);
      CsvHelper.AddCsvElement(sb, Word);
      CsvHelper.AddCsvElement(sb, Source);
      CsvHelper.AddCsvElement(sb, CorrectionWord);
      CsvHelper.AddCsvElement(sb, IsEditable == 1 ? "Ya" : "Tidak");
      CsvHelper.AddCsvElement(sb, WordProperties);
      return sb.ToString();
    }

    private long getBoolTokenValue(string token) 
      => token != null && token.Trim().ToLower().Equals("ya") ? 1 : 0;    

    private long getLongTokenValue(string token) {
      if (string.IsNullOrWhiteSpace(token)) return -1;
      long dummyLong;
      bool result = long.TryParse(token, out dummyLong);
      if (result) return dummyLong;
      return -1;
		}
      
    private const int EXPECTED_NO_OF_ELEMENTS = 6;
    /// <summary>
    /// Function to use a CSV string to fill properties of this object
    /// </summary>
    public void FromCsvTokens(List<string> tokens) {
      if (tokens == null || tokens.Count < EXPECTED_NO_OF_ELEMENTS)
        return;
      WordId = getLongTokenValue(tokens[0]);
      Word = tokens[1];
      Source = tokens[2];
      CorrectionWord = tokens[3];
      IsEditable = getBoolTokenValue(tokens[4]);
    }

    /// <summary>
    /// To get the CSV headers suited for this object
    /// </summary>
    /// <returns></returns>
    public static string GetCsvHeaders() {
      StringBuilder sb = new StringBuilder();
      sb.Append("ID Kata,");
      sb.Append("Kata,");
      sb.Append("Sumber,");
      sb.Append("Pembenahan,");
      sb.Append("Dapat Diubah,");
      sb.Append("Token,");
      return sb.ToString();
    }
	}

  [Serializable]
  [XmlRoot(ElementName = "KataSipebi")]
  public class KataSipebi {
    /// <summary>
    /// The list of each word item in the SiPEBI application
    /// </summary>
    [XmlElement("Kata")]
    public List<WordItem> Words = new List<WordItem>();
  }
}
