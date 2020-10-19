using System.Windows.Input;

namespace Sipebi.Core {
  /// <summary>
  /// A view model for each formal word page
  /// </summary>
  public class FormalWordViewModel : BaseViewModel {
    /// <summary>
    /// A single instance of the view model
    /// </summary>
    public static FormalWordViewModel Instance { get; } = new FormalWordViewModel();

    /// <summary>
    /// The flag to indicate if currently the adding (new item) process is going on
    /// </summary>
    public bool AddingIsRunning { get; set; }

    /// <summary>
    /// The flag to indicate if currently the filtering (the items) process is going on
    /// </summary>
    public bool FilteringIsRunning { get; set; }

    /// <summary>
    /// The flag to indicate if currently the filtering (the items) process is going on
    /// </summary>
    public double MaxContentHeight { get; set; } = 450;

    /// <summary>
    /// The command to add item to the formal word item list
    /// </summary>
    public ICommand AddCommand { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public FormalWordViewModel() {
      AddCommand = new RelayCommand(Add);
    }

    #region public methods
    /// <summary>
    /// Attempts to open option for adding the item
    /// </summary>
    /// <returns></returns>
    public void Add() {
      IoC.UI.FunctionNotCreated();
    }
    #endregion
  }
}
