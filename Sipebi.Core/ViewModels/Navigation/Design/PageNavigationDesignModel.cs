using System.Collections.Generic;

namespace Sipebi.Core {
	/// <summary>
	/// The design-time data for a <see cref="PageNavigationViewModel"/>
	/// </summary>
	public class PageNavigationDesignModel : PageNavigationViewModel {
    #region Singleton
    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static PageNavigationDesignModel DesignInstance { get; } = new PageNavigationDesignModel();
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public PageNavigationDesignModel() {
    }
    #endregion
  }
}
