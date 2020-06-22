namespace SipebiPrototype.Core {
	public class DH {
    /// <summary>
    /// The default connection string name for sipebi database
    /// </summary>
    public const string SipebiDataModelConnectionName = "KBBISipebi";

    /// <summary>
    /// The default table name for sipebi entri data items
    /// </summary>
    public const string SipebiEntriTableName = "EntriSipebi";

    /// <summary>
    /// The default table name for formal word items
    /// </summary>
    public const string FormalWordItemTableName = "FormalWordItem";


    /// <summary>
    /// The basic informal error code for items in KBBI
    /// </summary>
    public const string KbbiInformalWordDiagnosticErrorCode = "[KBBI-BTB]";

    /// <summary>
    /// The SiPEBI error code for tied words
    /// </summary>
    public const string SipebiTiedWordDiagnosticErrorCode = "[SiPEBI-BT]";

    /// <summary>
    /// The SiPEBI error code for definite capital words
    /// </summary>
    public const string SipebiDefiniteCapitalWordDiagnosticErrorCode = "[SiPEBI-HKD]";

    /// <summary>
    /// The SiPEBI error code for mark-not-attached error
    /// </summary>
    public const string SipebiMarkNotAttachedDiagnosticErrorCode = "[SiPEBI-TBTM]";

    /// <summary>
    /// The SiPEBI error code for ambiguous words
    /// </summary>
    public const string SipebiAmbiguousWordDiagnosticErrorCode = "[SiPEBI-KA]";

    /// <summary>
    /// The SiPEBI error code for place words
    /// </summary>
    public const string SipebiPlaceWordDiagnosticErrorCode = "[SiPEBI-KKT]";

    /// <summary>
    /// The SiPEBI error code for conjunction subordinative words
    /// </summary>
    public const string SipebiConjunctionSubordinativeWordDiagnosticErrorCode = "[SiPEBI-KKS]";

    /// <summary>
    /// The SiPEBI error code for detached mark
    /// </summary>
    public const string SipebiDetachedMarkDiagnosticErrorCode = "[SiPEBI-TBL]";

    /// <summary>
    /// The SiPEBI error code for detached word
    /// </summary>
    public const string SipebiDetachedWordDiagnosticErrorCode = "[SiPEBI-KLDTB]";


    /// <summary>
    /// The default folder name for user customization
    /// </summary>
    public const string DefaultCustomFolderName = "Kustomisasi";

    /// <summary>
    /// The default base folder name for texts
    /// </summary>
    public const string DefaultTextFolderName = "Teks";

    /// <summary>
    /// The default folder name for SiPEBI information
    /// </summary>
    public const string DefaultInfoFolderName = "Info";

    /// <summary>
    /// The default folder name for original texts
    /// </summary>
    public const string DefaultOriginalTextFolderName = "Asli";

    /// <summary>
    /// The default folder name for the default settings in SiPEBI
    /// </summary>
    public const string DefaultSettingsFolderName = "Bawaan";

    /// <summary>
    /// The default folder name for corrected texts
    /// </summary>
    public const string DefaultCorrectedTextFolderName = "Perbaikan";

    /// <summary>
    /// The default folder name for analysis results
    /// </summary>
    public const string DefaultAnalysisResultFolderName = "Analisis";

    /// <summary>
    /// The default file name for general errors
    /// </summary>
    public const string DefaultGeneralErrorsFileName = "KesalahanUmum";

    /// <summary>
    /// The default file name for ambiguous words
    /// </summary>
    public const string DefaultAmbiguousWordsFileName = "KataAmbigu";

    /// <summary>
    /// The default file name for definite capital words
    /// </summary>
    public const string DefaultDefiniteCapitalWordsFileName = "KataKapitalDefinit";

    /// <summary>
    /// The default file name for conjunction subordinative words
    /// </summary>
    public const string DefaultConjunctionSubordinativeWordsFileName = "KataKonjungsiSubordinatif";

    /// <summary>
    /// The default file name for place information words
    /// </summary>
    public const string DefaultPlaceInfoWordsFileName = "KataKeteranganTempat";

    /// <summary>
    /// The default file name for tied words
    /// </summary>
    public const string DefaultTiedWordsFileName = "KataTerikat";

    /// <summary>
    /// The default file name for formal word exception in KBBI
    /// </summary>
    public const string DefaultKbbiFormalWordsExceptionFileName = "PerkecualianKataBakuKbbi";

    /// <summary>
    /// The default file extension for customization information
    /// </summary>
    public const string DefaultCustomizationFileExtension = ".txt";

    /// <summary>
    /// The default augmented phrase for file saving
    /// </summary>
    public const string DefaultAugmentedPhraseForFileSaving = "-Perbaikan";

    /// <summary>
    /// The default file extension for loading and saving data
    /// </summary>
    public const string DefaultLoadedAndSavedFileExtension = ".txt";

    /// <summary>
    /// The default file text for loading and saving file filter
    /// </summary>
    public const string DefaultLoadedAndSavedFileFilterText = "Teks SiPEBI";

    /// <summary>
    /// The default augmented phrase for analysis result saving
    /// </summary>
    public const string DefaultAugmentedPhraseForAnalysisResultSaving = "-Analisis";

    /// <summary>
    /// The default file text for analysis result file filter
    /// </summary>
    public const string DefaultAnalysisFileFilterText = "Analysis SiPEBI";

    /// <summary>
    /// The default file extension for analysis result data
    /// </summary>
    public const string DefaultAnalysisFileExtension = ".csv";

    /// <summary>
    /// The default file name + extension for the default error types
    /// </summary>
    public const string DefaultErrorTypeFileNameAndExtension = "JenisKesalahan.txt";

    /// <summary>
    /// The default file name + extension for the build date file
    /// </summary>
    public const string DefaultBuildDateFileNameAndExtension = "BuildDate.txt";
  }
}


///// <summary>
///// The basic informal (long) error name for items in KBBI
///// </summary>
//public const string KbbiInformalWordDiagnosticError = "[KBBI] Bentuk Takbaku";

///// <summary>
///// The basic informal error explanation for items in KBBI
///// </summary>
//public const string KbbiInformalWordDiagnosticErrorExplanation = "[KBBI] Kata yang digunakan merupakan bentuk takbaku";

///// <summary>
///// The SiPEBI (long) error name for tied words
///// </summary>
//public const string SipebiTiedWordDiagnosticError = "[SiPEBI] Bentuk Terikat";

///// <summary>
///// The SiPEBI error explanation for tied words
///// </summary>
//public const string SipebiTiedWordDiagnosticErrorExplanation = "[SiPEBI] Kata yang digunakan merupakan bentuk terikat. Penulisannya harus disambung dengan kata sesudahnya";

///// <summary>
///// The SiPEBI (long) error name for definite capital words
///// </summary>
//public const string SipebiDefiniteCapitalWordDiagnosticError = "[SiPEBI] Huruf Kapital Definit";

///// <summary>
///// The SiPEBI error explanation for definite capital words
///// </summary>
//public const string SipebiDefiniteCapitalWordDiagnosticErrorExplanation = "[SiPEBI] Kata yang digunakan harus selalu ditulis menggunakan huruf kapital";

///// <summary>
///// The SiPEBI (long) error name for mark-not-attached error
///// </summary>
//public const string SipebiMarkNotAttachedDiagnosticError = "[SiPEBI] Tanda Baca Tidak Menempel";

///// <summary>
///// The SiPEBI error explanation for mark-not-attached error
///// </summary>
//public const string SipebiMarkNotAttachedDiagnosticErrorExplanation = "[SiPEBI] Tanda baca yang digunakan harus menempel pada kata sebelumnya";

///// <summary>
///// The SiPEBI (long) error name for ambiguous words
///// </summary>
//public const string SipebiAmbiguousWordDiagnosticError = "[SiPEBI] Kata Ambiguous";

///// <summary>
///// The SiPEBI error explanation for ambiguous words
///// </summary>
//public const string SipebiAmbiguosWordDiagnosticErrorExplanation = "[SiPEBI] Kata yang digunakan merupakan kata dengan kemungkinan ejaan yang ambigu";

///// <summary>
///// The SiPEBI (long) error name for place words
///// </summary>
//public const string SipebiPlaceWordDiagnosticError = "[SiPEBI] Kata Keterangan Tempat";

///// <summary>
///// The SiPEBI error explanation for place words
///// </summary>
//public const string SipebiPlaceWordDiagnosticErrorExplanation = "[SiPEBI] Kata yang digunakan merupakan kata keterangan tempat, seharusnya ditulis terpisah dari kata depan di-, ke-, dan dari-";

///// <summary>
///// The SiPEBI (long) error name for conjunction subordinative words
///// </summary>
//public const string SipebiConjunctionSubordinativeWordDiagnosticError = "[SiPEBI] Kata Konjungsi Subordinatif";

///// <summary>
///// The SiPEBI error explanation for conjunction subordinative words
///// </summary>
//public const string SipebiConjunctionSubordinativeWordDiagnosticErrorExplanation = "[SiPEBI] Kata yang digunakan merupakan kata konjungsi subordinatif, tidak perlu dituliskan dengan didahului koma pada kata sebelumnya";

///// <summary>
///// The SiPEBI (long) error name for detached mark
///// </summary>
//public const string SipebiDetachedMarkDiagnosticError = "[SiPEBI] Tanda Baca Lepas";

///// <summary>
///// The SiPEBI error explanation for detached mark
///// </summary>
//public const string SipebiDetachedMarkDiagnosticErrorExplanation = "[SiPEBI] Tanda baca ini merupakan tanda baca yang lepas, perlu disambungkan dengan elemen/kata terdekatnya";

///// <summary>
///// The SiPEBI (long) error name for detached word
///// </summary>
//public const string SipebiDetachedWordDiagnosticError = "[SiPEBI] Kata Lepas dari Tanda Baca";

///// <summary>
///// The SiPEBI error explanation for detached word
///// </summary>
//public const string SipebiDetachedWordDiagnosticErrorExplanation = "[SiPEBI] Kata ini tidak ditulis menempel pada tanda bacanya";