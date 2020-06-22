using System.Collections.Generic;
using System.Linq;

namespace SipebiPrototype.Core {
	/// <summary>
	/// A view model for the overview option list
	/// </summary>
	public class OptionListViewModel : BaseViewModel {
    /// <summary>
    /// A single instance of the view model
    /// </summary>
    public static OptionListViewModel ViewInstance { get; } = new OptionListViewModel();

		/// <summary>
		/// The option list items for the list
		/// </summary>
		public List<OptionListItemViewModel> Items { get; set; }

		/// <summary>
		/// The property to track which of the items is currently chosen
		/// </summary>
		public int CurrentlyChosen {
			get {
				if (Items == null || Items.Count <= 0)
					return -1;
				//https://stackoverflow.com/questions/4075340/finding-first-index-of-element-that-matches-a-condition-using-linq
				//get index of the first match, by Jon Skeet
				var index = Items.Select((value, i) => new { value, i })
											.Where(pair => pair.value.IsChosen)
											.Select(pair => pair.i)
											.FirstOrDefault();
				return index; 
			}
		}

		public OptionListViewModel() {
			Items = new List<OptionListItemViewModel> {
				new OptionListItemViewModel {
					OptionText = "Benahi Teks",
					OptionIcon = "\uf016", //TODO not a good practice, but leave it for now
          OptionToolTip = "Halaman penyuntingan ejaan",
          OptionPage = ApplicationPage.Work,
					OptionIndex = 0,
					IsChosen = true,
				},
        new OptionListItemViewModel {
          OptionText = "Tambahan\nDaftar Baku",
          OptionIcon = "\uf03a",
          OptionToolTip = "Halaman daftar perbaikan kata takbaku yang tidak terdapat pada KBBI",
          OptionPage = ApplicationPage.FormalWord,
          OptionIndex = 1,
        },
        new OptionListItemViewModel {
          OptionText = "Daftar Baku\nKBBI",
          OptionIcon = "\uf02d",
          OptionToolTip = "Halaman daftar perbaikan kata takbaku berdasarkan data KBBI",
          OptionPage = ApplicationPage.FormalWordKbbi,
          OptionIndex = 2,
        },
        new OptionListItemViewModel {
					OptionText = "Akun Pengguna",
					OptionIcon = "\uf007",
          OptionToolTip = "Halaman seputar akun pengguna",
          OptionPage = ApplicationPage.NotAvailable,
					OptionIndex = 3,
				},
				new OptionListItemViewModel {
					OptionText = "Pengaturan",
					OptionIcon = "\uf013",
          OptionToolTip = "Halaman pengaturan aplikasi",
          OptionPage = ApplicationPage.NotAvailable2,
					OptionIndex = 4,
				},
        new OptionListItemViewModel {
          OptionText = "Tentang SiPEBI",
          OptionIcon = "\uf05a",
          OptionToolTip = "Halaman penjelasan mengenai aplikasi SiPEBI",
          OptionPage = ApplicationPage.AboutUs,
          OptionIndex = 5,
        },
				new OptionListItemViewModel {
					OptionText = "Keluar",
					OptionIcon = "\uf08b",
          OptionToolTip = "Keluar dari aplikasi ini",
          OptionPage = ApplicationPage.Login,
					OptionIndex = 6,
				},        
      };
		}

		public void Choose(int chosenIndex) {
			var currentlyChosen = CurrentlyChosen;
			if (Items == null || chosenIndex >= Items.Count) //cannot choose if there is nothing to choose from
				return;
			if (currentlyChosen != -1)
				Items[currentlyChosen].IsChosen = false;
			if(Items[chosenIndex].OptionPage == ApplicationPage.Login) { //if it returns to the login page
				IoC.WorkVM.ResetState(); //Clear all the work texts, TODO could possible need to clear up some other things too
				Items[0].IsChosen = true; //then the next chosen page should be the first (working) page again
			} else //otherwise, choose the current page
				Items[chosenIndex].IsChosen = true;

		}
	}
}
