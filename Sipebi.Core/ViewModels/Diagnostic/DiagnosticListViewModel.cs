using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sipebi.Core {
	/// <summary>
	/// A view model for the overview diagnostic list
	/// </summary>
	public class DiagnosticListViewModel : BaseViewModel {
    /// <summary>
    /// A single instance of the view model
    /// </summary>
    public static DiagnosticListViewModel Instance { get; } = new DiagnosticListViewModel();

    /// <summary>
    /// The diagnostic list header. In actual case, the number of element in the header will only be one
    /// </summary>
    public List<DiagnosticListItemViewModel> ListHeader { get; set; }

    /// <summary>
    /// The diagnostic list items for the list
    /// </summary>
    public List<DiagnosticListItemViewModel> Items { get; set; }

    /// <summary>
    /// The command to record the analysis result
    /// </summary>
    public ICommand RecordAnalysisCommand { get; set; }

    /// <summary>
    /// The flag to indicate if recording analysis action is currently running
    /// </summary>
    public bool RecordAnalysisIsRunning { get; set; }

    /// <summary>
    /// The flag to indicate request to get refocus after the an action has been completed
    /// </summary>
    public bool RecordRefocusRequest { get; set; }

    /// <summary>
    /// The default constructor
    /// </summary>
		public DiagnosticListViewModel() {
      ListHeader = new List<DiagnosticListItemViewModel> {
        new DiagnosticListItemViewModel {
          DiagnosticNo = "No",
          ParagraphNo = "Para",
          ElementNo = "Ke-",
          ErrorCode = "Kode",
          Error = "Jenis Kesalahan",
          ErrorExplanation = "Penjelasan",
          OriginalElement = "Asli",
          CorrectedElement = "Perbaikan",
          IsHeader = true,
        },
      };

      RecordAnalysisCommand = new RelayCommand(async () => await RecordAnalysisAsync());
    }


    /// <summary>
    /// The command to save the analysis result to a file
    /// </summary>
    /// <returns></returns>
    public async Task RecordAnalysisAsync() {
      await RunCommandAsync(() => RecordAnalysisIsRunning, async () => {
        await Task.Run(() => {
          //Reset the refocus request
          RecordRefocusRequest = false;

          //Get the last analysis result
          List<DiagnosticListItemViewModel> items = Items;
          if (items == null)
            items = new List<DiagnosticListItemViewModel>();

          //Prepare necessary items for saving
          StringBuilder sb = new StringBuilder();
          sb.AppendLine(@"""No"",""Paragraf"",""Elemen Ke-"",""Kode"",""Jenis Kesalahan"",""Asli"",""Perbaikan"",""Penjelasan"""); //the header

          //Get the string of the last analysis result to be saved
          foreach (var item in items)
            sb.AppendLine(item.ToCsvElement());
          
          //Open the save file dialog
          bool? result = IoC.UI.RecordAnalysisSaveFileDialogWindow(sb.ToString());

          //To restore the focus back
          RecordRefocusRequest = true;
        });
      });
    }
  }
}
