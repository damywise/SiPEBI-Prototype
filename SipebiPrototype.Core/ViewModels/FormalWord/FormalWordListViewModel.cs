using System.Collections.Generic;
using System.Linq;

namespace SipebiPrototype.Core {
	/// <summary>
	/// A view model for the overview formal word list
	/// </summary>
	public class FormalWordListViewModel : BaseViewModel {
    /// <summary>
    /// A single instance of the view model
    /// </summary>
    public static FormalWordListViewModel Instance { get; } = new FormalWordListViewModel();

		/// <summary>
		/// The formal word list items for the list
		/// </summary>
		public List<FormalWordListItemViewModel> Items { get; set; }

		public FormalWordListViewModel() {
		}
	}
}
