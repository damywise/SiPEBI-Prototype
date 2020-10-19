using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipebi.Core {
	/// <summary>
	/// The design-time data for a <see cref="OptionListItemViewModel"/>
	/// </summary>
	public class OptionListItemDesignModel : OptionListItemViewModel {
    #region Singleton
    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static OptionListItemDesignModel Instance { get; } = new OptionListItemDesignModel(); //{ get { return new ChatListItemDesignModel(); } }
    #endregion

    #region Constructor
    /// <summary>
    /// Default constructor
    /// </summary>
    public OptionListItemDesignModel() {
      OptionText = "Benahi Teks";
      OptionIcon = "\uf016"; //TODO not a good practice, but leave it for now
      OptionToolTip = "Halaman penyuntingan ejaan";
			OptionPage = ApplicationPage.Work;
    }
		#endregion
	}
}
