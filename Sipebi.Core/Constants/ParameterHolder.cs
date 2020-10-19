using PropertyChanged;
using System.Collections.Generic;

namespace Sipebi.Core {
	public class PH {
    /// <summary>
    /// The string for the default open file dialog directory
    /// </summary>
    public static string DefaultOpenFileDialogDirectory { get; set; }

    /// <summary>
    /// The string for the default save file dialog directory for corrected text
    /// </summary>
    public static string DefaultCorrectedTextSaveFileDialogDirectory { get; set; }

    /// <summary>
    /// The string for the default save file dialog directory for analysis result
    /// </summary>
    public static string DefaultAnalysisResultSaveFileDialogDirectory { get; set; }

    /// <summary>
    /// The string content for the latest opened file for working
    /// </summary>
    public static string LatestOpenedFileString { get; set; }

    /// <summary>
    /// The file name for the latest opened file for working
    /// </summary>
    public static string LatestOpenedFileName { get; set; }

    /// <summary>
    /// The list of default error codes in the <see cref="SipebiDiagnosticError"/>
    /// </summary>
    public static List<string> DefaultErrorCodes = new List<string> {
      DH.KbbiInformalWordDiagnosticErrorCode,
			DH.SipebiTiedWordDiagnosticErrorCode,
			DH.SipebiDefiniteCapitalWordDiagnosticErrorCode,
			DH.SipebiMarkNotAttachedDiagnosticErrorCode,
			DH.SipebiAmbiguousWordDiagnosticErrorCode,
			DH.SipebiPlaceWordDiagnosticErrorCode,
			DH.SipebiConjunctionSubordinativeWordDiagnosticErrorCode,
			DH.SipebiDetachedMarkDiagnosticErrorCode,
			DH.SipebiDetachedWordDiagnosticErrorCode,
		};

    /// <summary>
    /// The list of the valid word properties. This list is to be expanded over time.
    /// </summary>
    public static List<string> ValidWordProperties = new List<string>() {
      DH.takbaku, //this word might be an informal word
      DH.bermakna, //this word has meaning
      DH.berhomonim, //this word has homonym(s)
      DH.berhomograf, //this word has homograph(s)
      DH.baku_homograf_sama, //this word has homographs whose formal words are identical with this one
      DH.takbaku_ambigu, //this word is either informal or formal
    };

    /// <summary>
    /// A simple method to determine if a word property is a valid one
    /// </summary>
    /// <param name="wordProperty">The word property whose validity is to be checked</param>
    /// <returns>The checking return</returns>
    public static bool IsValidWordProperty(string wordProperty) =>
      !string.IsNullOrWhiteSpace(wordProperty) &&
      ValidWordProperties.Contains(wordProperty?.ToLower()?.Trim());

    /// <summary>
    /// A simple method to determine if a word property is the word property intended.
    /// The intended word property must be taken from <see cref="ValidWordProperties"/>
    /// </summary>
    /// <param name="wordProperty">The word property to be checked</param>
    /// <param name="intendedWordProperty">The intended word property to be obtained, taken from <see cref="ValidWordProperties"/></param>
    /// <returns>The checking return</returns>
    public static bool CheckWordProperty(string wordProperty, string intendedWordProperty) =>
      !string.IsNullOrWhiteSpace(wordProperty) &&
      wordProperty?.ToLower().Trim() == intendedWordProperty;

    #region Prototype 2
    //Only needs to change the Properties -> Icon to icon_editor.ico when set to editor mode
    public static readonly bool SipebiUserMode = true;

    public static string SipebiVersionString = SipebiUserMode ? "Pengguna" : "Editor";
    public static string SipebiVersionFullString = "Versi: " + SipebiVersionString;

    public static readonly string SipebiDataFilename = DH.DataFilename + "." + DH.SipebiE;
    public static readonly string SipebiSettingsFilename = DH.SettingsFilename + "." + DH.SipebiE;

    public static readonly string SipebiP = PH.SipebiUserMode ? DH.SipebiPU : DH.SipebiPE;
    public static readonly byte[] SipebiK = PH.SipebiUserMode ? DH.SipebiKU : DH.SipebiKE;

    public static string SipebiIconPath = @"/Images/Logo/" + 
      (SipebiUserMode ? "icon_refined2.png" : "icon_editor2.png");

    public static string SipebiBuildDate = string.Empty;

    /// <summary>
    /// The string for the software version of SiPEBI
    /// </summary>
    public static string SoftwareVersion { get; set; }

    /// <summary>
    /// The KBBI version used for SiPEBI
    /// </summary>
    public static string KBBIVersion { get; set; } = "April 2020";

    public static string CopyrightSymbol { get; set; } = "© 2020";
    #endregion
  }
}
