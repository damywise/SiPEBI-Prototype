using System.Threading.Tasks;

namespace SipebiPrototype.Core {
  /// <summary>
  /// The UI manager that handles any UI interaction in the application
  /// </summary>
  public interface IUIManager {
    /// <summary>
    /// Displays a single message box to the user
    /// </summary>
    /// <param name="viewModel">The view model</param>
    /// <returns></returns>
    Task ShowMessage(MessageBoxDialogViewModel viewModel);

    /// <summary>
    /// The function to call a pop-up window stating that a function is not created yet
    /// </summary>
    void FunctionNotCreated();

    /// <summary>
    /// The function to call a pop-up window showing the diagnostic report of SiPEBI
    /// </summary>
    void ShowDiagnosticReport(SipebiDiagnosticReport report);

    /// <summary>
    /// The function to call a pop-up window stating that a saving action has been completed
    /// </summary>
    void SaveCompleted();

    /// <summary>
    /// The function to call a pop-up window stating that a saving action has been cancelled
    /// </summary>
    void SaveCancelled();

    /// <summary>
    /// The function to call a pop-up window for opening a file
    /// </summary>
    bool? OpenFileDialogWindow();

    /// <summary>
    /// The function to call a pop-up window for saving a file
    /// </summary>
    bool? SaveFileDialogWindow(string savedText);

    /// <summary>
    /// The function to call a pop-up window for recording the analysis result
    /// </summary>
    bool? RecordAnalysisSaveFileDialogWindow(string savedText);

    /// <summary>
    /// The function to scroll the scroll-viewer related to this model to a given position
    /// </summary>
    /// <param name="originalTextPosition">The position of the original text to be scrolled to</param>
    /// <param name="correctedTextPosition">The position of the corrected text to be scrolled to</param>
    /// <param name="originalTextLength">The length of the original text to be selected</param>
    /// <param name="correctedTextLength">The length of the corrected text to be selected</param>
    void ScrollToPosition(int originalTextPosition, int correctedTextPosition, int originalTextLength, int correctedTextLength);
    
    /// <summary>
    /// The function to get the open report button text
    /// </summary>
    /// <returns></returns>
    string GetOpenReportButtonText(int numberOfCorrection);

    /// <summary>
    /// The function to get the close report button text
    /// </summary>
    /// <returns></returns>
    string GetCloseReportButtonText();

    /// <summary>
    /// The function to get the open report button tool tip
    /// </summary>
    /// <returns></returns>
    string GetOpenReportButtonToolTip();

    /// <summary>
    /// The function to get the close report button tool tip
    /// </summary>
    /// <returns></returns>
    string GetCloseReportButtonToolTip();
  }
}
