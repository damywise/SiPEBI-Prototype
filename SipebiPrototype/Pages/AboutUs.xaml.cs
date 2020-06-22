using SipebiPrototype.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SipebiPrototype {
  /// <summary>
  /// Interaction logic for AboutUsPage.xaml
  /// </summary>
  public partial class AboutUsPage : BasePage<AboutUsViewModel> {
    public AboutUsPage() {
      InitializeComponent();

      int i = 1;
      TextBlockListOfErrors.Text = string.Join(Environment.NewLine, 
        IoC.DiagnosticErrorInformationList.Select(x => "[" + i++ + "] " + x.ErrorCode + " => " + x.Error).ToList());
    }
  }
}
