using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SipebiPrototype.Core;

namespace SipebiPrototype {
  /// <summary>
  /// The application's implementation of the <see cref="IUIManager"/>
  /// </summary>
  public class UIManager : IUIManager {
    /// <summary>
    /// Displays a single message box to the user
    /// </summary>
    /// <param name="viewModel">the view model</param>
    /// <returns></returns>
    public Task ShowMessage(MessageBoxDialogViewModel viewModel) {
      return Task.Run(() => {
        Application.Current.Dispatcher.Invoke(() => {
          new DialogMessageBox().ShowDialog(viewModel);
        });
      });
    }

    /// <summary>
    /// The function to call a pop-up window stating that a function is not created yet
    /// </summary>
    public void FunctionNotCreated() {
      IoC.UI.ShowMessage(new MessageBoxDialogViewModel {
        Title = "Belum Tersedia",
        Message = "Fungsi ini belum tersedia",
        OkText = "Oke"
      });
    }

    /// <summary>
    /// The function to call a pop-up window showing the diagnostic report of SiPEBI
    /// </summary>
    public void ShowDiagnosticReport(SipebiDiagnosticReport report) {
      StringBuilder sb = new StringBuilder(report.Errors.Count.ToString() + " bagian teks telah diperbaiki atau ditandai sebagai ambigu!");
      int noOfSpecificErrors = 0;
      int maxSpecificErrors = 20; //so that the pop-up window won't be too "tall"
      int noOfAccummulatedErrors = 0;
      sb.AppendLine();
      if (report.HasReport) { //if the report has a (valid) report, then breaks down the report item
        sb.AppendLine(); //the first time has an extra line
        foreach (var item in report.Items.OrderByDescending(x => x.NoOfErrors)) {
          if (noOfSpecificErrors >= maxSpecificErrors) {
            sb.AppendLine("+" + (report.Errors.Count - noOfAccummulatedErrors) + " Kesalahan Lainnya");
            break;
          } else
            sb.AppendLine(item.NoOfErrors.ToString() + " " + item.ErrorCode + " - " + item.Error);
          noOfAccummulatedErrors += item.NoOfErrors;
          noOfSpecificErrors++;
        }
      }
      sb.AppendLine();
      sb.AppendLine("Durasi Koreksi: " + report.ExecutionTime.TotalSeconds.ToString("N2") + " detik");
      sb.AppendLine("Jumlah Koreksi (Tanpa Ambigu): " + (report.NoOfCorrection - report.NoOfAmbiguous));
      sb.AppendLine("Jumlah Ambivalensi: " + report.NoOfAmbiguous);
      sb.AppendLine("Jumlah Paragraf: " + report.NoOfParagraphs);
      sb.AppendLine("Jumlah Elemen: " + report.NoOfElements);
      sb.AppendLine("Panjang Teks (Awal): " + report.OriginalTextLength);
      sb.AppendLine("Panjang Teks (Hasil Koreksi): " + report.CorrectedTextLength);
      sb.AppendLine("Waktu Mulai Koreksi: " + report.StartTime.ToString("dd-MM-yyyy HH:mm:ss.fff"));
      sb.Append("Waktu Selesai Koreksi: " + report.EndTime.ToString("dd-MM-yyyy HH:mm:ss.fff"));

      IoC.UI.ShowMessage(new MessageBoxDialogViewModel {
        Title = "Laporan Hasil Perbaikan",
        Message = sb.ToString(),
        OkText = "Oke"
      });
    }

    /// <summary>
    /// The function to call a pop-up window stating that a saving action has been completed
    /// </summary>
    public void SaveCompleted() {
      IoC.UI.ShowMessage(new MessageBoxDialogViewModel {
        Title = "Berhasil",
        Message = "Penyimpanan berhasil",
        OkText = "Oke"
      });
    }


    /// <summary>
    /// The function to call a pop-up window stating that a saving action has been cancelled
    /// </summary>
    public void SaveCancelled() {
      IoC.UI.ShowMessage(new MessageBoxDialogViewModel {
        Title = "Batal",
        Message = "Penyimpanan dibatalkan",
        OkText = "Oke"
      });
    }

    /// <summary>
    /// The function to call a pop-up window for opening a file
    /// </summary>
    public bool? OpenFileDialogWindow() {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = DH.DefaultLoadedAndSavedFileFilterText + "|*" + DH.DefaultLoadedAndSavedFileExtension;
      openFileDialog.Multiselect = false; //only a single file can be selected
      openFileDialog.InitialDirectory = PH.DefaultOpenFileDialogDirectory;
      bool? result = openFileDialog.ShowDialog();
      if (result != null && result.Value) {
        string fileName = openFileDialog.FileName;
        string latestFolder = Path.GetDirectoryName(openFileDialog.FileName);
        PH.LatestOpenedFileName = Path.GetFileNameWithoutExtension(fileName);
        PH.DefaultOpenFileDialogDirectory = latestFolder;
        PH.LatestOpenedFileString = File.ReadAllText(fileName);
      }
      return result;
    }

    /// <summary>
    /// The function to call a pop-up window for saving a file
    /// </summary>
    public bool? SaveFileDialogWindow(string savedText) {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = DH.DefaultLoadedAndSavedFileFilterText + "|*" + DH.DefaultLoadedAndSavedFileExtension;
      saveFileDialog.FileName = PH.LatestOpenedFileName + DH.DefaultAugmentedPhraseForFileSaving + DH.DefaultLoadedAndSavedFileExtension;
      saveFileDialog.InitialDirectory = PH.DefaultCorrectedTextSaveFileDialogDirectory;
      bool? result = saveFileDialog.ShowDialog();
      if (result != null && result.Value) {
        string latestFolder = Path.GetDirectoryName(saveFileDialog.FileName);
        PH.DefaultCorrectedTextSaveFileDialogDirectory = latestFolder;
        File.WriteAllText(saveFileDialog.FileName, savedText);
      }
      return result;
    }

    /// <summary>
    /// The function to call a pop-up window for recording the analysis result
    /// </summary>
    public bool? RecordAnalysisSaveFileDialogWindow(string savedText) {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = DH.DefaultAnalysisFileFilterText + "|*" + DH.DefaultAnalysisFileExtension;
      saveFileDialog.FileName = PH.LatestOpenedFileName + DH.DefaultAugmentedPhraseForAnalysisResultSaving + DH.DefaultAnalysisFileExtension;
      saveFileDialog.InitialDirectory = PH.DefaultAnalysisResultSaveFileDialogDirectory;
      bool? result = saveFileDialog.ShowDialog();
      if (result != null && result.Value) {
        string latestFolder = Path.GetDirectoryName(saveFileDialog.FileName);
        PH.DefaultAnalysisResultSaveFileDialogDirectory = latestFolder;
        File.WriteAllText(saveFileDialog.FileName, savedText);
      }
      return result;
    }

    ///// <summary>
    ///// The <see cref="ScrollViewer"/> of the text box containing the original text
    ///// </summary>
    //public static ScrollViewer OriginalTextScrollViewer { get; set; }

    ///// <summary>
    ///// The <see cref="ScrollViewer"/> of the text box containing the corrected text
    ///// </summary>
    //public static ScrollViewer CorrectedTextScrollViewer { get; set; }

    /// <summary>
    /// The <see cref="TextBox"/> of the text box containing the original text
    /// </summary>
    public static TextBox OriginalTextBox { get; set; }

    /// <summary>
    /// The <see cref="TextBox"/> of the text box containing the corrected text
    /// </summary>
    public static TextBox CorrectedTextBox { get; set; }

    /// <summary>
    /// The function to provide this manager with scroll viewers and textboxes to be referred to
    /// </summary>
    /// <param name="originalTextBox">The <see cref="TextBox"/> of the original text</param>
    /// <param name="correctedTextBox">The <see cref="TextBox"/> of the corrected text</param>
    public void SetTextBoxes(TextBox originalTextBox, TextBox correctedTextBox) {
      OriginalTextBox = originalTextBox;
      CorrectedTextBox = correctedTextBox;
    }
    ///// <summary>
    ///// The function to provide this manager with scroll viewers and textboxes to be referred to
    ///// </summary>
    ///// <param name="originalTextScrollViewer">The <see cref="ScrollViewer"/> of the original text</param>
    ///// <param name="correctedTextScrollViewer">The <see cref="ScrollViewer"/> of the corrected text</param>
    ///// <param name="originalTextBox">The <see cref="TextBox"/> of the original text</param>
    ///// <param name="correctedTextBox">The <see cref="TextBox"/> of the corrected text</param>
    //  public void SetScrolls(ScrollViewer originalTextScrollViewer, ScrollViewer correctedTextScrollViewer,
    //  TextBox originalTextBox, TextBox correctedTextBox) {
    //  OriginalTextScrollViewer = originalTextScrollViewer;
    //  CorrectedTextScrollViewer = correctedTextScrollViewer;
    //  OriginalTextBox = originalTextBox;
    //  CorrectedTextBox = correctedTextBox;
    //}

    /// <summary>
    /// The function to scroll the scroll-viewer related to this model to a given position
    /// </summary>
    /// <param name="originalTextPosition">The position of the original text to be scrolled to</param>
    /// <param name="correctedTextPosition">The position of the corrected text to be scrolled to</param>
    /// <param name="originalTextLength">The length of the original text to be selected</param>
    /// <param name="correctedTextLength">The length of the corrected text to be selected</param>
    public void ScrollToPosition(int originalTextPosition, int correctedTextPosition, int originalTextLength, int correctedTextLength) {
      //if (OriginalTextScrollViewer == null || CorrectedTextScrollViewer == null || OriginalTextBox == null || CorrectedTextBox == null)
      //  return; //immediately retun if any of the scroll viewer(s) is missing
      if (OriginalTextBox == null || CorrectedTextBox == null)
        return; //immediately retun if any of the scroll viewer(s) is missing
      if (correctedTextPosition >= 0) {         
        CorrectedTextBox.Focus();  //NOTE: focus must come first!! Otherwise the caret will not bring scrolling
        CorrectedTextBox.CaretIndex = correctedTextPosition;
        if (correctedTextLength > 0)
          CorrectedTextBox.Select(correctedTextPosition, correctedTextLength);
      }
      if (originalTextPosition >= 0) { //as long as the original text position is non-negative
        OriginalTextBox.Focus(); //the original text box always gets the focus
        OriginalTextBox.CaretIndex = originalTextPosition;
        if (originalTextLength > 0)
          OriginalTextBox.Select(originalTextPosition, originalTextLength);
      }
    }

    /// <summary>
    /// The function to get the open report button text
    /// </summary>
    /// <returns></returns>
    public string GetOpenReportButtonText(int numberOfCorrection)
      => (string)Application.Current.FindResource("FontAwesomeOpenReportWordWithIcon") + " (" + numberOfCorrection + ")";
    

    /// <summary>
    /// The function to get the close report button text
    /// </summary>
    /// <returns></returns>
    public string GetCloseReportButtonText() 
      => (string)Application.Current.FindResource("FontAwesomeCloseReportWordWithIcon");    

    /// <summary>
    /// The function to get the open report button tool tip
    /// </summary>
    /// <returns></returns>
    public string GetOpenReportButtonToolTip() => "Buka hasil analisis teks";

    /// <summary>
    /// The function to get the close report button tool tip
    /// </summary>
    /// <returns></returns>
    public string GetCloseReportButtonToolTip() => "Tutup hasil analisis teks";
  }
}
