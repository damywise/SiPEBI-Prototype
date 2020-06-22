using SipebiPrototype.Core;

namespace SipebiPrototype {
  /// <summary>
  /// Interaction logic for WorkPage.xaml
  /// </summary>
  public partial class WorkPage : BasePage<WorkViewModel> {
    public WorkPage() {
      InitializeComponent();

      //Once initialized, set the text boxes of the UI manager to ease its later use
      ((UIManager)IoC.UI).SetTextBoxes(OriginalTextBox, CorrectedTextBox);
      //((UIManager)IoC.UI).SetTextBoxes(OriginalTextScrollViewer, CorrectedTextScrollViewer,
      //  OriginalTextBox, CorrectedTextBox);
    }
  }
}
