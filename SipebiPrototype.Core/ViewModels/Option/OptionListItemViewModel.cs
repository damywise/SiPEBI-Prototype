using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipebiPrototype.Core {
  /// <summary>
  /// A view model for each option list item
  /// </summary>
  public class OptionListItemViewModel : BaseViewModel {
		/// <summary>
		/// The display icon of this option
		/// </summary>
		public string OptionIcon { get; set; }

		/// <summary>
		/// The display text of this option
		/// </summary>
		public string OptionText { get; set; }

    /// <summary>
    /// The display tool tip of this option
    /// </summary>
    public string OptionToolTip { get; set; }

    /// <summary>
    /// The command affiliated with this option
    /// </summary>
    public RelayCommand OptionCommand { get; set; }

		/// <summary>
		/// The application page affiliated with this option
		/// </summary>
		public ApplicationPage OptionPage { get; set; }

		/// <summary>
		/// The index of this option page, for re-selection 
		/// </summary>
		public int OptionIndex { get; set; }

    /// <summary>
    /// True if this option is chosen
    /// </summary>
    public bool IsChosen { get; set; }

    /// <summary>
    /// true if this item get selected
    /// </summary>
    public bool IsSelected { get; set; }

    //public SolidColorBrush //we cannot put this because this is specific for  WPF
    //principle: never puts in ViewModel anything that is related to the actual UI

		public OptionListItemViewModel() {
			OptionCommand = new RelayCommand(async () => await OptionAsync());
		}

		/// <summary>
		/// Takes the user to the page affiliated with this option
		/// </summary>
		/// <returns></returns>
		private async Task OptionAsync() {
			//Change the switch
			var optionList = IoC.OptionListVM;
			optionList.Choose(OptionIndex);

			//Go to affiliated page
			IoC.AppVM.GoToPage(OptionPage);

			await Task.Delay(1);
		}
	}
}
