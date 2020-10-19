using System.Threading.Tasks;
using System.Windows.Input;

namespace Sipebi.Core {
  /// <summary>
  /// A view model for each formal word list item
  /// </summary>
  public class FormalWordListItemViewModel : BaseViewModel {
    /// <summary>
    /// Flag to indicate if this word list item is currently selected
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// The informal word portion of this item
    /// </summary>
    public string InformalWord { get; set; }

    /// <summary>
    /// The formal word portion of this item
    /// </summary>
    public string FormalWord { get; set; }

    /// <summary>
    /// Flag to indicate if this word list item is editable
    /// </summary>
    public bool IsEditable { get; set; }

    /// <summary>
    /// The source of this formal word item
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// The edit command for this formal word item
    /// </summary>
    public ICommand EditCommand { get; set; }

    /// <summary>
    /// The flag to indicate that this item is currently opening edit option
    /// </summary>
    public bool EditIsRunning { get; set; }

    /// <summary>
    /// The delete command for this formal word item
    /// </summary>
    public ICommand DeleteCommand { get; set; }

    /// <summary>
    /// The flag to indicate that this item is currently opening delete option
    /// </summary>
    public bool DeleteIsRunning { get; set; }

    /// <summary>
    /// The non-activation command for this formal word item
    /// </summary>
    public ICommand DisableCommand { get; set; }

    /// <summary>
    /// The flag to indicate that this item is currently opening disable option
    /// </summary>
    public bool DisableIsRunning { get; set; }

    /// <summary>
    /// The details command for this formal word item
    /// </summary>
    public ICommand DetailsCommand { get; set; }

    /// <summary>
    /// The flag to indicate that this item is currently opening details
    /// </summary>
    public bool DetailsIsRunning { get; set; }

    public FormalWordListItemViewModel() {}

    public FormalWordListItemViewModel(FormalWordItem item) {
      InformalWord = item.InformalWord;
      FormalWord = item.FormalWord;
      IsEditable = item.Editable();
      Source = item.Source;
      EditCommand = new RelayCommand(Edit);
      DeleteCommand = new RelayCommand(Delete);
      DisableCommand = new RelayCommand(Disable);
      DetailsCommand = new RelayCommand(Details);
    }

    #region public methods
    /// <summary>
    /// Attempts to open option for editing the item
    /// </summary>
    /// <returns></returns>
    public void Edit() {
      IoC.UI.FunctionNotCreated();
    }

    /// <summary>
    /// Attempts to open option for deleting the item
    /// </summary>
    /// <returns></returns>
    public void Delete() {
      IoC.UI.FunctionNotCreated();
    }

    /// <summary>
    /// Attempts to open option for disabling the item
    /// </summary>
    /// <returns></returns>
    public void Disable() {
      IoC.UI.FunctionNotCreated();
    }

    /// <summary>
    /// Attempts to open details for the item
    /// </summary>
    /// <returns></returns>
    public void Details() {
      IoC.UI.FunctionNotCreated();
    }
    #endregion
  }
}
