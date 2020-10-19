using System.Collections.Generic;

namespace Sipebi.Core {
	/// <summary>
	/// The design-time data for a <see cref="FormalWordListViewModel"/>
	/// </summary>
	public class FormalWordListDesignModel : FormalWordListViewModel {
    #region Singleton
    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static FormalWordListDesignModel DesignInstance { get; } = new FormalWordListDesignModel(); //{ get { return new OptionListItemDesignModel(); } }
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public FormalWordListDesignModel() {
      Items = new List<FormalWordListItemViewModel> {
        new FormalWordListItemViewModel {
          InformalWord = "abadiat",
          FormalWord = "abadiah",
          IsSelected = true,
        },
        new FormalWordListItemViewModel {
          InformalWord = "abdul",
          FormalWord = "abdu",
        },
        new FormalWordListItemViewModel {
          InformalWord = "abimana",
          FormalWord = "abaimana",
        },
        new FormalWordListItemViewModel {
          InformalWord = "ablur",
          FormalWord = "hablur",
        },
        new FormalWordListItemViewModel {
          InformalWord = "acan",
          FormalWord = "acah",
        },
        new FormalWordListItemViewModel {
          InformalWord = "aci",
          FormalWord = "acik",
        },
        new FormalWordListItemViewModel {
          InformalWord = "adan",
          FormalWord = "azan",
        },
        new FormalWordListItemViewModel {
          InformalWord = "adenda",
          FormalWord = "adendum",
        },
        new FormalWordListItemViewModel {
          InformalWord = "adikanda",
          FormalWord = "adinda",
        },
        new FormalWordListItemViewModel {
          InformalWord = "adjektif",
          FormalWord = "adjektiva",
        },
        new FormalWordListItemViewModel {
          InformalWord = "adpis",
          FormalWord = "advis",
        },
        new FormalWordListItemViewModel {
          InformalWord = "afuah",
          FormalWord = "afwah",
        },
        new FormalWordListItemViewModel {
          InformalWord = "agamis",
          FormalWord = "agamais",
        },
        new FormalWordListItemViewModel {
          InformalWord = "agung",
          FormalWord = "gung",
        },
        new FormalWordListItemViewModel {
          InformalWord = "aih",
          FormalWord = "ai",
        },
        new FormalWordListItemViewModel {
          InformalWord = "tambahsaya",
          FormalWord = "tambahansaya",
          IsEditable = true,
        },
        new FormalWordListItemViewModel {
          InformalWord = "tambahdia",
          FormalWord = "tambahandia",
          IsEditable = true,
        },
      };
    }
    #endregion
  }
}
