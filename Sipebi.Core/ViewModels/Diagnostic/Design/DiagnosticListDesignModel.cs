using System.Collections.Generic;

namespace Sipebi.Core {
	/// <summary>
	/// The design-time data for a <see cref="DiagnosticListViewModel"/>
	/// </summary>
	public class DiagnosticListDesignModel : DiagnosticListViewModel {
    #region Singleton
    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static DiagnosticListDesignModel DesignInstance { get; } = new DiagnosticListDesignModel();
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public DiagnosticListDesignModel() {
      Items = new List<DiagnosticListItemViewModel> {
        new DiagnosticListItemViewModel {
          DiagnosticNo = "1100",
          ParagraphNo = "1200",
          ElementNo = "400",
          ErrorCode = "[00001]",
          Error = "Error Long Name 00001",
          ErrorExplanation = "Explanation long long long long long 00001",
          OriginalElement = "Original Long 00001",
          CorrectedElement = "Corrected Long 00001",                    
        },
        new DiagnosticListItemViewModel {
          DiagnosticNo = "2",
          ParagraphNo = "2",
          ElementNo = "4",
          ErrorCode = "[00002]",
          Error = "Error Name 00002",
          ErrorExplanation = "Explanation 00002",
          OriginalElement = "Original 00002",
          CorrectedElement = "Corrected 00002",
        },
        new DiagnosticListItemViewModel {
          DiagnosticNo = "3",
          ParagraphNo = "5",
          ElementNo = "12",
          ErrorCode = "[00003]",
          Error = "Error Name 00003",
          ErrorExplanation = "Explanation 00003",
          OriginalElement = "Original 00003",
          CorrectedElement = "Corrected 00003",
        },
      };
    }
    #endregion
  }
}
