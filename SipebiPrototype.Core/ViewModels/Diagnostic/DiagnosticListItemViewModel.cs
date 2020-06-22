using Extension.Extractor;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SipebiPrototype.Core {
  /// <summary>
  /// A view model for each diagnostic list item
  /// </summary>
  public class DiagnosticListItemViewModel : BaseViewModel {
    /// <summary>
    /// Flag to indicate if this diagnostic list item is actually a header instead of an item
    /// </summary>
    public bool IsHeader { get; set; }

    /// <summary>
    /// Flag to indicate if this diagnostic list item is currently selected
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// The diagnostic number for this item, it is a string for this can be a header
    /// </summary>
    public string DiagnosticNo { get; set; }

    /// <summary>
    /// The paragraph number for this item, it is a string for this can be a header
    /// </summary>
    public string ParagraphNo { get; set; }

    /// <summary>
    /// The element number for this item, it is a string for this can be a header
    /// </summary>
    public string ElementNo { get; set; }

    /// <summary>
    /// The original element of this item (before correction)
    /// </summary>
    public string OriginalElement { get; set; }

    /// <summary>
    /// The corrected element of this item (after correction)
    /// </summary>
    public string CorrectedElement { get; set; }

    /// <summary>
    /// The error code for this diagnostic item
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// The error name for this diagnostic item
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    /// The error explanation for this diagnostic item
    /// </summary>
    public string ErrorExplanation { get; set; }

    /// <summary>
    /// The diagnostic item related to this view model
    /// </summary>
    public SipebiDiagnosticError DiagnosticItem { get; set; }

    /// <summary>
    /// The command to navigate the original text cursor to the error related to this element
    /// </summary>
    public ICommand NavigateCommand { get; set; }

    public DiagnosticListItemViewModel() {
      NavigateCommand = new RelayCommand(Navigate);
    }

    public DiagnosticListItemViewModel(int diagnosticNo, SipebiDiagnosticError item, SipebiDiagnosticErrorInformation errorInformation) :
      this() {
      DiagnosticItem = item;
      DiagnosticNo = diagnosticNo.ToString();
      ParagraphNo = item.ParagraphNo.ToString();
      ElementNo = item.ElementNo.ToString();
      ErrorCode = errorInformation.ErrorCode;
      Error = errorInformation.Error;
      OriginalElement = item.OriginalElement;
      CorrectedElement = item.CorrectedElement;
      ErrorExplanation = errorInformation.ErrorExplanation;
    }

    #region public methods
    public void Navigate() {
      IoC.UI.ScrollToPosition(
        DiagnosticItem.OriginalCharPosition, DiagnosticItem.CorrectedCharPosition, DiagnosticItem.OriginalElement.Length , -1);
    }
    //To test if the corrected char position is correct
    //public void Navigate() {
    //  IoC.UI.ScrollToPosition(
    //  -1, DiagnosticItem.CorrectedCharPosition);
    //}

    public string ToCsvElement() {
      BaseSystemData diagnosticNo = new BaseSystemData(DiagnosticNo);
      BaseSystemData paragraphNo = new BaseSystemData(ParagraphNo);
      BaseSystemData elementNo = new BaseSystemData(ElementNo);
      BaseSystemData originalElement = new BaseSystemData(OriginalElement);
      BaseSystemData correctedElement = new BaseSystemData(CorrectedElement);
      BaseSystemData errorCode = new BaseSystemData(ErrorCode);
      BaseSystemData error = new BaseSystemData(Error);
      BaseSystemData errorExplanation = new BaseSystemData(ErrorExplanation);
      return string.Concat(
        diagnosticNo.GetCsvValueString(), ",",
        paragraphNo.GetCsvValueString(), ",",
        elementNo.GetCsvValueString(), ",",
        errorCode.GetCsvValueString(), ",",
        error.GetCsvValueString(), ",",
        originalElement.GetCsvValueString(), ",",
        correctedElement.GetCsvValueString(), ",",
        errorExplanation.GetCsvValueString());
    }
    #endregion
  }
}
