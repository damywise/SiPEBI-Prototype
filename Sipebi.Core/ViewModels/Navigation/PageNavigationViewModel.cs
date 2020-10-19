using System.Collections.Generic;
using System.Windows.Input;

namespace Sipebi.Core {
	/// <summary>
	/// A view model for the overview diagnostic list
	/// </summary>
	public class PageNavigationViewModel : BaseViewModel {
    /// <summary>
    /// A single instance of the view model
    /// </summary>
    public static PageNavigationViewModel Instance { get; } = new PageNavigationViewModel();

    /// <summary>
    /// The command to go to the next page
    /// </summary>
    public ICommand NextCommand { get; set; }

    /// <summary>
    /// The command to go to the next 10 pages
    /// </summary>
    public ICommand Next10Command { get; set; }

    /// <summary>
    /// The command to go to the next 100 pages
    /// </summary>
    public ICommand Next100Command { get; set; }

    /// <summary>
    /// The command to go to the last page
    /// </summary>
    public ICommand LastCommand { get; set; }

    /// <summary>
    /// The command to go to the previous page
    /// </summary>
    public ICommand PreviousCommand { get; set; }

    /// <summary>
    /// The command to go to the previous 10 pages
    /// </summary>
    public ICommand Previous10Command { get; set; }

    /// <summary>
    /// The command to go to the previous 100 pages
    /// </summary>
    public ICommand Previous100Command { get; set; }

    /// <summary>
    /// The command to go to the first page
    /// </summary>
    public ICommand FirstCommand { get; set; }

    /// <summary>
    /// The property to indicate the current page number
    /// </summary>
    public int CurrentPageNo { get; set; }

    /// <summary>
    /// The property to indicate the total page number
    /// </summary>
    public int TotalPageNo { get; set; }

    /// <summary>
    /// The property to indicate the number of item per page
    /// </summary>
    public int ItemPerPage { get; set; }

    /// <summary>
    /// The default constructor
    /// </summary>
		public PageNavigationViewModel() {
      //The commands
      NextCommand = new RelayCommand(() => {
      });
      Next10Command = new RelayCommand(() => {
      });
      Next100Command = new RelayCommand(() => {
      });
      LastCommand = new RelayCommand(() => {
      });
      PreviousCommand = new RelayCommand(() => {
      });
      Previous10Command = new RelayCommand(() => {
      });
      Previous100Command = new RelayCommand(() => {
      });
      FirstCommand = new RelayCommand(() => {
      });
    }
  }
}
