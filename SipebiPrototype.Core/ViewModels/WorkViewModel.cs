using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SipebiPrototype.Core {
  /// <summary>
  /// The View Model for the login screen
  /// </summary>
  public class WorkViewModel : BaseViewModel {
		//#region private member
		//#endregion

		#region public properties
		/// <summary>
		/// The single instance of this WorkViewModel
		/// </summary>
		public static WorkViewModel Instance { get; } = new WorkViewModel();

		/// <summary>
		/// The original text before correction
		/// </summary>
		public string OriginalText { get; set; }

		/// <summary>
		/// The text after correction
		/// </summary>
		public string CorrectedText { get; set; }

    /// <summary>
    /// The text for analysis result button
    /// </summary>
    public string AnalysisResultButtonText { get; set; }

    /// <summary>
    /// The tooltip for analysis result button
    /// </summary>
    public string AnalysisResultButtonToolTip { get; set; }

    /// <summary>
    /// A flag indicating if the correction command is running
    /// </summary>
    public bool CorrectionIsRunning { get; set; }

    /// <summary>
    /// A flag indicating if the save command is running
    /// </summary>
    public bool SaveIsRunning { get; set; }

    /// <summary>
    /// A flag indicating if the analysis result is currently being updated
    /// </summary>
    public bool AnalysisUpdateIsRunning { get; set; }

    /// <summary>
    /// A flag indicating if the analysis result page is opened
    /// </summary>
    public bool AnalysisResultIsOpened { get; set; }

    /// <summary>
    /// Flag to indicate if the workpage requests to get refocus after releasing its focus.
    /// Intended to be used after save file dialog completion, where the focus from the workpage is taken and never returned
    /// </summary>
    public bool RefocusRequest { get; set; }

    ///// <summary>
    ///// The value of the orginal text caret index
    ///// </summary>
    //public int OriginalTextCaretIndex { get; set; }

    ///// <summary>
    ///// The value of the corrected text caret index
    ///// </summary>
    //public int CorrectedTextCaretIndex { get; set; }

    ///// <summary>
    ///// A text indicating the corner radius of the correction button
    ///// </summary>
    //public string CorrectionButtonCornerRadius { get; set; }
    #endregion

    #region Commands
    /// <summary>
    /// The command to load a file
    /// </summary>
    public ICommand LoadFileCommand { get; set; }

    /// <summary>
    /// The command to do the correction
    /// </summary>
    public ICommand CorrectionCommand { get; set; }

    /// <summary>
    /// The command to save the work content
    /// </summary>
    public ICommand SaveCommand { get; set; }
    
    /// <summary>
    /// The command to open analysis result
    /// </summary>
    public ICommand AnalysisResultCommand { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Default constructor
    /// </summary>
    public WorkViewModel() {
      AnalysisResultButtonText = "\uf108 Analisis (0)"; //TODO the first time is a hack, no choice for now
      AnalysisResultButtonToolTip = "Buka hasil analisis teks";
      LoadFileCommand = new RelayCommand(() => LoadFile());
      CorrectionCommand = new RelayCommand(async () => await CorrectionAsync());
      SaveCommand = new RelayCommand(async () => await SaveAsync());
      AnalysisResultCommand = new RelayCommand(async () => await AnalysisResultAsync());
    }
    #endregion

    #region public methods
    /// <summary>
    /// The command to load a file from windows
    /// </summary>
    public void LoadFile() {
      bool? result = IoC.UI.OpenFileDialogWindow();
      if (!result.HasValue || !result.Value)
        return;
      OriginalText = PH.LatestOpenedFileString;
      PH.LatestOpenedFileString = string.Empty; //clear
    }

    /// <summary>
    /// Global running number to track the number of corrections made in the latest execution
    /// </summary>
    public int NumberOfCorrection => LastReport == null ? 0 : LastReport.NoOfCorrection;

    /// <summary>
    /// Global running number to track the number of ambiguity found in the latest execution
    /// </summary>
    public int NumberOfAmbiguity => LastReport == null ? 0 : LastReport.NoOfAmbiguous;

    /// <summary>
    /// Flag to indicate if last analysis result needs update
    /// </summary>
    public bool LastAnalysisNotUpdated = false;

    /// <summary>
    /// Function to update the last analysis results
    /// </summary>
    private void updateLastAnalysis(List<SipebiDiagnosticError> lastErrors) {
      //Get the last errors and update the view model
      IoC.DiagnosticListVM.Items = new List<DiagnosticListItemViewModel>();
      if (lastErrors != null) {
        int diagnosticNo = 1;
        for (int i = 0; i < lastErrors.Count; ++i) {
          SipebiDiagnosticErrorInformation ei = IoC.DiagnosticErrorInformationList.FirstOrDefault(x => x.ErrorCode == lastErrors[i].ErrorCode);
          if (ei == null)
            continue;
          IoC.DiagnosticListVM.Items.Add(new DiagnosticListItemViewModel(diagnosticNo, lastErrors[i], ei));
          ++diagnosticNo;
        }
      }
      LastAnalysisNotUpdated = false;
    }

    /// <summary>
    /// The last report obtained by this view model
    /// </summary>
    public SipebiDiagnosticReport LastReport { get; set; } = new SipebiDiagnosticReport();

    /// <summary>
    /// Attempts to correct the original text
    /// </summary>
    /// <returns></returns>
    //[STAThread]
    public async Task CorrectionAsync() {
      //this is passing the lamdba expression... with a property which can be modified internally! This is new!
      await RunCommandAsync(() => CorrectionIsRunning, async () => {
        await Task.Run(() => {
          //Correct the text using whatever methodology available
          LastReport = IoC.SipebiDM.CorrectText(OriginalText);
          CorrectedText = LastReport.CorrectedText;

          //As long as it has report, then shows the report
          if (LastReport.HasReport)
            IoC.UI.ShowDiagnosticReport(LastReport);

          //Update the UI and flags
          if (AnalysisResultIsOpened) {
            updateLastAnalysis(LastReport.Errors);
            LastAnalysisNotUpdated = false;
          } else {
            AnalysisResultButtonText = IoC.UI.GetOpenReportButtonText(NumberOfCorrection);
            LastAnalysisNotUpdated = true;
          }
        });
      });
    }

    /// <summary>
    /// The command to save the corrected text to a file
    /// </summary>
    /// <returns></returns>
    public async Task SaveAsync() {
      await RunCommandAsync(() => SaveIsRunning, async () => {
        await Task.Run(() => {
          //Reset the refocus request
          RefocusRequest = false;

          //Open the save file dialog
          bool? result = IoC.UI.SaveFileDialogWindow(CorrectedText);

          //Cannot be done since it won't make the focus come back to the main UI, leave it for now
          ////Show the save completed/cancelled message
          //if (result != null && result.Value)
          //  IoC.UI.SaveCompleted();
          //else
          //  IoC.UI.SaveCancelled();

          //To restore the focus back
          RefocusRequest = true;
        });
      });
    }

    /// <summary>
    /// The command to show the latest report of the correction execution
    /// </summary>
    public async Task AnalysisResultAsync() {
      await RunCommandAsync(() => AnalysisUpdateIsRunning, async () => {
        await Task.Run(() => {
          if (LastAnalysisNotUpdated)
            updateLastAnalysis(LastReport.Errors);
          AnalysisResultIsOpened = !AnalysisResultIsOpened;
          AnalysisResultButtonText = AnalysisResultIsOpened ? 
            IoC.UI.GetCloseReportButtonText() : (IoC.UI.GetOpenReportButtonText(NumberOfCorrection));
          AnalysisResultButtonToolTip = AnalysisResultIsOpened ?
            IoC.UI.GetCloseReportButtonToolTip() : IoC.UI.GetOpenReportButtonToolTip();
        });
      });
    }

    //To reset the state of the view model
    public void ResetState() {
      LastReport = new SipebiDiagnosticReport();
      CorrectedText = string.Empty;
      OriginalText = string.Empty;
      AnalysisResultIsOpened = false; //close the latest analysis result
      AnalysisResultButtonText = IoC.UI.GetOpenReportButtonText(0);
      AnalysisResultButtonToolTip = IoC.UI.GetOpenReportButtonToolTip();
      if (IoC.DiagnosticListVM.Items != null)
        IoC.DiagnosticListVM.Items.Clear();
    }
    #endregion

    #region private helpers
    #endregion
  }
}
