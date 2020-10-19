using Sipebi.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sipebi {
  /// <summary>
  /// Interaction logic for AboutUsPage.xaml
  /// </summary>
  public partial class AboutUsPage : BasePage<AboutUsViewModel> {
    public AboutUsPage() {
      InitializeComponent();
      TextBlockListOfErrors.Text = string.Join(Environment.NewLine, 
        IoC.DiagnosticErrorInformationList.Select((x, i) => "[" + ++i + "] " + x.ErrorCode + " => " + x.Error 
        + " (Sejak Versi: " + x.AppearOnVersion + ")").ToList());
    }
  }
}
