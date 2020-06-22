namespace SipebiPrototype.Core {
	/// <summary>
	/// Details for a message box dialog
	/// </summary>
	public class MessageBoxDialogDesignModel : MessageBoxDialogViewModel {
		#region Singleton
		/// <summary>
		/// A single instance of the design model
		/// </summary>
		public static MessageBoxDialogDesignModel Instance { get; } = new MessageBoxDialogDesignModel();
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public MessageBoxDialogDesignModel() {
			OkText = "OK";
			Message = "Design time messages are fun :)";
		}
		#endregion

	}
}
