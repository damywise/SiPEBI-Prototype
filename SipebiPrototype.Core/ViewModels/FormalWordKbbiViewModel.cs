using System.Collections.Generic;

namespace SipebiPrototype.Core {
  /// <summary>
  /// A view model for each formal word page
  /// </summary>
  public class FormalWordKbbiViewModel : BaseViewModel {
    /// <summary>
    /// A single instance of the view model
    /// </summary>
    public static FormalWordKbbiViewModel Instance { get; } = new FormalWordKbbiViewModel();

    /// <summary>
    /// The string containing the leftmost formal words in the KBBI
    /// </summary>
    public string FormalWordKbbiDataLeftMost { get; set; }

    /// <summary>
    /// The string containing the left formal words in the KBBI
    /// </summary>
    public string FormalWordKbbiDataLeft { get; set; }

    /// <summary>
    /// The string containing the middle left formal words in the KBBI
    /// </summary>
    public string FormalWordKbbiDataMiddleLeft { get; set; }

    /// <summary>
    /// The string containing the middle right formal words in the KBBI
    /// </summary>
    public string FormalWordKbbiDataMiddleRight { get; set; }

    /// <summary>
    /// The string containing the right formal words in the KBBI
    /// </summary>
    public string FormalWordKbbiDataRight { get; set; }

    /// <summary>
    /// The string containing the rightmost formal words in the KBBI
    /// </summary>
    public string FormalWordKbbiDataRightMost { get; set; }

    ///// <summary>
    ///// The formal word items in the KBBI
    ///// </summary>
    //public List<FormalWordItem> Items { get; set; }
  }
}
