using System.Collections.Generic;

namespace Sipebi.Core {
	/// <summary>
	/// The design-time data for a <see cref="OptionListViewModel"/>
	/// </summary>
	public class OptionListDesignModel : OptionListViewModel {
    #region Singleton
    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static OptionListDesignModel DesignInstance { get; } = new OptionListDesignModel(); //{ get { return new OptionListItemDesignModel(); } }
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public OptionListDesignModel() : base() {}
    #endregion
  }
}
