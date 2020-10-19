using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Sipebi.Core {
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
    public bool Editable() => IsEditable > 0;

    /// <summary>
    /// The flag to indicate that this item, other than being informal word, has an actual meaning
    /// </summary>
    public long? HasMeaning { get; set; }

    /// <summary>
    /// The flag to indicate that this item has homonym(s)
    /// </summary>
    public long? HasHomonym { get; set; }

    /// <summary>
    /// The flag to indicate that this item has homograph(s)
    /// </summary>
    public long? HasHomograph { get; set; }

    /// <summary>
    /// The flag to indicate if this item has homograph(s) which all are also informal words and having exact formal words as this one
    /// </summary>
    public long? SameFormalWordsInHomographs { get; set; } 

    /// <summary>
    /// The function call to check if this word is not totally informal, but ambiguous
    /// </summary>
    public long? IsAmbiguous { get; set; }
  
    /// <summary>
    /// Function to check if two formal word item is equal
    /// </summary>
    public bool IsEqualItem(FormalWordItem item, bool sourceChecked) {
      if (sourceChecked)
        return item.InformalWord.Equals(InformalWord) && item.Source.Equals(Source);
      return item.InformalWord.Equals(InformalWord);
    }

    /// <summary>
    /// Function to check if the given item is completely equal to this item
    /// </summary>
    /// <param name="item">The item to compare this item with</param>
    /// <returns>The checking result</returns>
    public bool IsFullyEqualItem(FormalWordItem item) =>
      InformalWord == item.InformalWord &&
      FormalWord == item.FormalWord &&
      Source == item.Source &&
      IsEditable == item.IsEditable &&
      IsAmbiguous == item.IsAmbiguous &&
      HasMeaning == item.HasMeaning &&
      HasHomonym == item.HasHomonym &&
      HasHomograph == item.HasHomograph &&
      SameFormalWordsInHomographs == item.SameFormalWordsInHomographs;
    
    /// <summary>
    /// Function to convert this object into a CSV string
    /// </summary>
    public string ToCsvString() {
      StringBuilder sb = new StringBuilder();
      CsvHelper.AddCsvElement(sb, InformalWord);
      CsvHelper.AddCsvElement(sb, FormalWord);
      CsvHelper.AddCsvElement(sb, Source);
      CsvHelper.AddCsvElement(sb, IsEditable == 1 ? "Ya" : "Tidak");
      CsvHelper.AddCsvElement(sb, IsAmbiguous == 1 ? "Ya" : "Tidak");
      CsvHelper.AddCsvElement(sb, HasMeaning == 1 ? "Ya" : "Tidak");
      CsvHelper.AddCsvElement(sb, HasHomonym == 1 ? "Ya" : "Tidak");
      CsvHelper.AddCsvElement(sb, HasHomograph == 1 ? "Ya" : "Tidak");
      CsvHelper.AddCsvElement(sb, SameFormalWordsInHomographs == 1 ? "Ya" : "Tidak", isLast: true);
      return sb.ToString();
    }

    private long getBoolTokenValue(string token) 
      => token != null && token.Trim().ToLower().Equals("ya") ? 1 : 0;    

    private const int EXPECTED_NO_OF_ELEMENTS = 9;
    /// <summary>
    /// Function to use a CSV string to fill properties of this object
    /// </summary>
    public void FromCsvTokens(List<string> tokens) {
      if (tokens == null || tokens.Count < EXPECTED_NO_OF_ELEMENTS)
        return;
      InformalWord = tokens[0];
      FormalWord = tokens[1];
      Source = tokens[2];
      IsEditable = getBoolTokenValue(tokens[3]);
      IsAmbiguous = getBoolTokenValue(tokens[4]);
      HasMeaning = getBoolTokenValue(tokens[5]);
      HasHomonym = getBoolTokenValue(tokens[6]);
      HasHomograph = getBoolTokenValue(tokens[7]);
      SameFormalWordsInHomographs = getBoolTokenValue(tokens[8]);
    }

    /// <summary>
    /// To get the CSV headers suited for this object
    /// </summary>
    /// <returns></returns>
    public static string GetCsvHeaders() {
      StringBuilder sb = new StringBuilder();
      sb.Append("Bentuk Takbaku,");
      sb.Append("Bentuk Baku,");
      sb.Append("Sumber,");
      sb.Append("Dapat Diubah,");
      sb.Append("Ambigu,");
      sb.Append("Memiliki Makna,");
      sb.Append("Memiliki Homonim,");
      sb.Append("Memiliki Homograf,");
      sb.Append("Bentuk Baku Homograf Sama");
      return sb.ToString();
    }

    /// <summary>
    /// The method to convert this <see cref="FormalWordItem"/> to <see cref="WordItem"/> with injected id
    /// </summary>
    /// <param name="id">The <see cref="WordItem"/>'s ID to be injected</param>
    /// <returns>The <see cref="WordItem"/> returned</returns>
    public WordItem ToWordItem(long id) {
      WordItem item = ToWordItem();
      item.WordId = id;
      return item;
		}

    /// <summary>
    /// The method to convert this <see cref="FormalWordItem"/> to <see cref="WordItem"/>
    /// </summary>
    /// <returns>The <see cref="WordItem"/> returned</returns>
    public WordItem ToWordItem() {
      WordItem item = new WordItem {
        Word = InformalWord,
        Source = Source,
        CorrectionWord = FormalWord,
        IsEditable = IsEditable,
      };
      List<string> props = new List<string>() { DH.takbaku };
      if (HasMeaning == 1)
        props.Add(DH.bermakna);
      if (HasHomonym == 1)
        props.Add(DH.berhomonim);
      if (HasHomograph == 1)
        props.Add(DH.berhomograf);
      if (SameFormalWordsInHomographs == 1)
        props.Add(DH.baku_homograf_sama);
      if (IsAmbiguous == 1)
        props.Add(DH.takbaku_ambigu);
      item.WordProperties = string.Join(";", props.ToArray());
      return item;
		}
	}
}
