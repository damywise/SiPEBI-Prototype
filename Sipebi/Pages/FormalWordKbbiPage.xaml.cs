using Sipebi.Core;

namespace Sipebi {
  /// <summary>
  /// Interaction logic for FormalWordKbbiPage.xaml
  /// </summary>
  public partial class FormalWordKbbiPage : BasePage<FormalWordKbbiViewModel>{
    public FormalWordKbbiPage() {
      InitializeComponent();
      ViewModel = IoC.FormalWordKbbiVM;
    }
  }
}
