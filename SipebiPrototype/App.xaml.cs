using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Extension.Versioning;
using SipebiPrototype.Core;

namespace SipebiPrototype {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		/// <summary>
		/// Customer startup so we load our IoC immediately before anything else
		/// </summary>
		/// <param name="e"></param>
		protected override void OnStartup(StartupEventArgs e) {
			// Let the base application do what it needs
			base.OnStartup(e);

			// Setup IoC, must be before the main window is shown
			IoC.Setup();

      // Bind a UI Manager
      IoC.Kernel.Bind<IUIManager>().ToConstant(new UIManager());

      // Setup the Sipebi data model used in the application
      IoC.Kernel.Bind<ISipebiDataModel>().ToConstant(new SipebiDataModel());

      //Set the default directory for loading and saving files
      string baseDir = AppDomain.CurrentDomain.BaseDirectory;
      string textsDir = Path.Combine(baseDir, DH.DefaultTextFolderName);
      string originalTextsDir = Path.Combine(textsDir, DH.DefaultOriginalTextFolderName);
      string correctedTextsDir = Path.Combine(textsDir, DH.DefaultCorrectedTextFolderName);
      string analysisResultsDir = Path.Combine(textsDir, DH.DefaultAnalysisResultFolderName);
      string settingDir = Path.Combine(baseDir, DH.DefaultSettingsFolderName);
      Directory.CreateDirectory(originalTextsDir);
      Directory.CreateDirectory(correctedTextsDir);
      Directory.CreateDirectory(analysisResultsDir);
      Directory.CreateDirectory(settingDir);
      PH.DefaultOpenFileDialogDirectory = originalTextsDir;
      PH.DefaultCorrectedTextSaveFileDialogDirectory = correctedTextsDir;
      PH.DefaultAnalysisResultSaveFileDialogDirectory = analysisResultsDir;

      // Get the software version
      string version = Info.GetFileVersionFor(Assembly.GetExecutingAssembly());
      string infoDir = Path.Combine(baseDir, DH.DefaultInfoFolderName);
      string buildDateFilePath = Path.Combine(infoDir, DH.DefaultBuildDateFileNameAndExtension);
      Directory.CreateDirectory(infoDir);
      DateTime timeStamp = new DateTime();//TimeStamp.RetrieveLinkerTimestamp();
      if (File.Exists(buildDateFilePath)) {
        string buildDateInfo = File.ReadAllText(buildDateFilePath);
        bool result;
        DateTime testDt;
        result = DateTime.TryParseExact(buildDateInfo.Trim(), "ddd MM/dd/yyyy HH:mm:ss.ff", null, DateTimeStyles.AssumeLocal, out testDt);
        if (result)
          timeStamp = testDt;
      }
      PH.SoftwareVersion = version + "-" + timeStamp.ToString("yyyyMMddHHmmss");

      // Show the main window
      Current.MainWindow = new MainWindow();
			Current.MainWindow.Show();
		}
	}
}
