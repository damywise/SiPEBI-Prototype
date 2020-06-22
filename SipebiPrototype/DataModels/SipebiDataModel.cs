using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Extension.Database.Sqlite;
using Extension.Extractor;
using SipebiPrototype.Core;
using System;
using System.IO;
using Extension.Models;
using KBBI.Core;

namespace SipebiPrototype {
  public class SipebiDataModel : ISipebiDataModel {
    /// <summary>
    /// The list of entri sipebi data loaded/prepared
    /// </summary>
    private static List<EntriSipebi> entriSipebis { get; set; }

    /// <summary>
    /// The list of formal word items loaded/prepared
    /// </summary>
    private static List<FormalWordItem> formalWords { get; set; }

    /// <summary>
    /// The function to get the list of entri sipebi data loaded/prepared
    /// </summary>
    public List<EntriSipebi> GetEntriSipebis() => entriSipebis;

    /// <summary>
    /// The function to prepare all the entri sipebi data
    /// </summary>
    public Task PrepareEntriSipebis() {
      return Task.Run(() => {
        string connString = ConfigurationManager.ConnectionStrings[DH.SipebiDataModelConnectionName].ConnectionString;
        DataTable table = SQLiteHandler.GetFullDataTable(connString, DH.SipebiEntriTableName);
        entriSipebis = BaseExtractor.ExtractList<EntriSipebi>(table);
      });
    }

    /// <summary>
    /// The function to clear all the entri sipebi data
    /// </summary>
    public void ClearEntriSipebis() {
      if (entriSipebis != null)
        entriSipebis.Clear();
    }

    /// <summary>
    /// The first function to be called prepare all necessary items for the subsequent preparations (such as the default diagnostic errors)
    /// </summary>
    public Task InitialPreparation() {
      return Task.Run(() => {
        //Load default diagnostic information errors
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        if (IoC.DiagnosticErrorInformationList == null)
          IoC.DiagnosticErrorInformationList = new List<SipebiDiagnosticErrorInformation>();
        IoC.DiagnosticErrorInformationList.Clear();
        string settingDir = Path.Combine(baseDir, DH.DefaultSettingsFolderName);
        Directory.CreateDirectory(settingDir);
        string filePath = Path.Combine(settingDir, DH.DefaultErrorTypeFileNameAndExtension);
        if (!File.Exists(filePath))
          return;
        var lines = File.ReadAllLines(filePath)
          .Where(x => !string.IsNullOrWhiteSpace(x))
          .Where(x => !x.Trim().StartsWith("//"))
          .Select(x => x.Trim())
          .ToList();

        //Add default error information item(s)
        foreach (var ec in PH.DefaultErrorCodes) {
          var line = lines.FirstOrDefault(x => x.StartsWith(ec));
          if (line == null)
            continue;
          SimpleExpression exp = new SimpleExpression(line, "=>", false);
          if (exp.IsSingular || !exp.IsValid)
            continue;
          SimpleExpression rightExp = new SimpleExpression(exp.RightSide, "=>", false);
          if (rightExp.IsSingular || !rightExp.IsValid)
            continue;
          IoC.DiagnosticErrorInformationList.Add(new SipebiDiagnosticErrorInformation {
            ErrorCode = ec,
            Error = rightExp.LeftSide,
            ErrorExplanation = rightExp.RightSide,
          });
        }
      });
    }

    /// <summary>
    /// The function to get the list of formal word items
    /// </summary>
    public List<FormalWordItem> GetFormalWords() => formalWords;

    /// <summary>
    /// The function to prepare all the formal word items
    /// </summary>
    public Task PrepareFormalWords() {
      return Task.Run(() => {
        string connString = ConfigurationManager.ConnectionStrings[DH.SipebiDataModelConnectionName].ConnectionString;

        //Create KBBI formal words
        DataTable table = SQLiteHandler.GetFullDataTable(connString, DH.FormalWordItemTableName);
        IoC.InformalWordsKbbi = BaseExtractor.ExtractList<FormalWordItem>(table);
        IoC.InformalWordsKbbi.ForEach(x => x.DiagnosticErrorCode = DH.KbbiInformalWordDiagnosticErrorCode); //set the error code for KBBI
        List<string> joinStrs = IoC.InformalWordsKbbi.OrderBy(x => x.InformalWord.ToLower())
          .Select(x => x.InformalWord + " -> " + x.FormalWord).ToList();
        int noOfColumns = 4;
        int leftOver = joinStrs.Count % noOfColumns;
        int noOfLines = joinStrs.Count / noOfColumns + (leftOver > 0 ? 1 : 0);
        int totalElementsTaken = 0;
        List<List<string>> joinStrParts = new List<List<string>>();
        for(int i = 0; i < noOfColumns; ++i) {
          List<string> currentJoinStrs = joinStrs.Skip(totalElementsTaken)
          .Take(leftOver == 0 || leftOver > i ? noOfLines : noOfLines - 1).ToList();
          totalElementsTaken += currentJoinStrs.Count;
          joinStrParts.Add(currentJoinStrs);
        }

        int columNo = 0;
        IoC.FormalWordKbbiVM.FormalWordKbbiDataLeftMost = string.Join(Environment.NewLine, joinStrParts[columNo++]);
        IoC.FormalWordKbbiVM.FormalWordKbbiDataLeft = string.Join(Environment.NewLine, joinStrParts[columNo++]);
        IoC.FormalWordKbbiVM.FormalWordKbbiDataMiddleLeft = string.Join(Environment.NewLine, joinStrParts[columNo++]);
        IoC.FormalWordKbbiVM.FormalWordKbbiDataMiddleRight = string.Join(Environment.NewLine, joinStrParts[columNo++]);
        //IoC.FormalWordKbbiVM.FormalWordKbbiDataRight = string.Join(Environment.NewLine, joinStrParts[columNo++]);
        //IoC.FormalWordKbbiVM.FormalWordKbbiDataRightMost = string.Join(Environment.NewLine, joinStrParts[columNo++]);

        //Load formal words from other sources
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string customDir = Path.Combine(baseDir, DH.DefaultCustomFolderName);
        Directory.CreateDirectory(customDir);

        if (IoC.FormalWordListVM.Items == null)
          IoC.FormalWordListVM.Items = new List<FormalWordListItemViewModel>();
        if (IoC.CustomInformalWords == null)
          IoC.CustomInformalWords = new List<FormalWordItem>();
        if (IoC.DiagnosticErrorInformationList == null)
          IoC.DiagnosticErrorInformationList = new List<SipebiDiagnosticErrorInformation>();
        IoC.FormalWordListVM.Items.Clear();
        IoC.CustomInformalWords.Clear();

        //Get exceptions for KBBI formal words which do not want to be corrected
        string exceptionFilePath = Path.Combine(customDir, DH.DefaultKbbiFormalWordsExceptionFileName + DH.DefaultCustomizationFileExtension);
        if (File.Exists(exceptionFilePath)) {
          List<string> kbbiExceptions = File.ReadAllLines(exceptionFilePath).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
          if (kbbiExceptions != null && kbbiExceptions.Count > 0)
            IoC.InformalWordsKbbi = IoC.InformalWordsKbbi.Where(x => !kbbiExceptions.Any(y => y.Equals(x.InformalWord))).ToList();
        }

        string filePath = Path.Combine(customDir, DH.DefaultGeneralErrorsFileName + DH.DefaultCustomizationFileExtension);
        string currentErrorCode = "[UNASSIGNED]"; 
        if (!File.Exists(filePath))
          return;

        //Add error explanation(s) + formal words from other source(s)
        List<string> lines = File.ReadAllLines(filePath).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
        foreach (var line in lines) {
          //check for diagnostic error line
          if (line.Contains("=>")) { 
            SimpleExpression diagExp = new SimpleExpression(line, "=>", false);
            if (!diagExp.IsValid || diagExp.IsSingular)
              continue;
            SimpleExpression diagExplanationExp = new SimpleExpression(diagExp.RightSide, "=>", false);
            if (!diagExplanationExp.IsValid || diagExplanationExp.IsSingular)
              continue;
            currentErrorCode = diagExp.LeftSide; //assign the current error code according to the newly given "header" of error codes
            SipebiDiagnosticErrorInformation sdei = new SipebiDiagnosticErrorInformation {
              ErrorCode = diagExp.LeftSide,
              Error = diagExplanationExp.LeftSide,
              ErrorExplanation = diagExplanationExp.RightSide,
            };
            IoC.DiagnosticErrorInformationList.Add(sdei);
            continue;
          }

          //other than diagnostic error line is the list of errors
          SimpleExpression exp = new SimpleExpression(line, "->", false);
          if (IoC.InformalWordsKbbi.Any(x => x.InformalWord.Equals(exp.LeftSide))) //already in the KBBI
            continue;
          FormalWordItem item = new FormalWordItem {
            InformalWord = exp.LeftSide,
            FormalWord = exp.RightSide,
            IsEditable = 1,
            Source = "Custom",
            DiagnosticErrorCode = currentErrorCode,
          };
          IoC.CustomInformalWords.Add(item);
        }
        IoC.CustomInformalWords = IoC.CustomInformalWords.OrderBy(x => x.InformalWord.ToLower()).ToList();

        //Prepare the view model from the current formal word items
        IoC.FormalWordListVM.Items = IoC.CustomInformalWords.Select(x => new FormalWordListItemViewModel(x)).ToList();

        //Optimization to speed up process of informal/formal word checking
        IoC.InformalWordStringsKbbi = IoC.InformalWordsKbbi.Select(x => x.InformalWord).ToList();
        IoC.CustomInformalWordStrings = IoC.CustomInformalWords.Select(x => x.InformalWord).ToList();

        //Prepare formal words for BOTH KBBI and custom
        formalWords = new List<FormalWordItem>();
        formalWords.AddRange(IoC.InformalWordsKbbi);
        formalWords.AddRange(IoC.CustomInformalWords);
        formalWords = formalWords.OrderBy(x => x.InformalWord.ToLower()).ToList();
      });
    }

    /// <summary>
    /// The function to clear all the formal word items
    /// </summary>
    public void ClearFormalWords() {
      if (formalWords != null)
        formalWords.Clear();
    }

    /// <summary>
    /// The function to add a formal word item
    /// <param name="item">The new formal word item to be added</paramref>
    /// </summary>
    public bool AddFormalWord(FormalWordItem item) {
      if (formalWords == null)
        formalWords = new List<FormalWordItem>();
      if (formalWords.Any(x => x.IsEqualItem(item, sourceChecked: false)))
        return false; //item already exists
      formalWords.Add(item);
      return true;
    }

    /// <summary>
    /// The function to edit a formal word item
    /// <param name="item">The new formal word item to edit the old one</paramref>
    /// <param name="index">The index of the the old item to be edited</paramref>
    /// </summary>
    public bool EditFormalWord(FormalWordItem item, int index) {
      if (formalWords == null || index >= formalWords.Count || index < 0)
        return false;
      FormalWordItem oldItem = formalWords[index];
      if (!oldItem.Editable())
        return false; //item not editable
      if (item.IsEqualItem(oldItem, sourceChecked: false)) { //if it is the same as the old item
        if (item.Source.Equals(oldItem.Source) && item.FormalWord.Equals(oldItem.FormalWord)) //nothing is changed
          return false;
        formalWords[index] = item; //source is changed, replace the item with the new source
        return true;
      }
      if (formalWords.Any(x => x.IsEqualItem(item, sourceChecked: false))) //is an equal item with anything but the old edited item
        return false; //item already exists
      formalWords[index] = item; //the item is changed anew, replace the item with the new source
      return true;
    }

    /// <summary>
    /// The function to delete a formal word item
    /// <param name="index">The index of the the item to be deleted</paramref>
    /// </summary>
    public bool DeleteFormalWord(int index) {
      if (formalWords == null || index >= formalWords.Count || index < 0)
        return false;
      FormalWordItem oldItem = formalWords[index];
      if (!oldItem.Editable())
        return false; //item not deletable
      formalWords.RemoveAt(index);
      return true;
    }

    /// <summary>
    /// The function to get details of a formal word item
    /// <param name="index">The index of the the item to get the details from</paramref>
    /// <param name="doNotGetWhenNotEditable">The flag to indicate if the item is to be obtained if it is not editable</paramref>
    /// </summary>
    public FormalWordItem DetailsFormalWord(int index, bool doNotGetWhenNotEditable) {
      if (formalWords == null || index >= formalWords.Count || index < 0)
        return null; //fail to get the formal word item
      FormalWordItem oldItem = formalWords[index];
      if (doNotGetWhenNotEditable && !oldItem.Editable())
        return null; //fail to get because the item is not editable but we want to get it
      return oldItem;
    }

    /// <summary>
    /// The list of SiPEBI linked/tied words
    /// </summary>
    private static List<SipebiLinkedWord> sipebiLinkedWords { get; set; }

    /// <summary>
    /// The function to get he list of SiPEBI linked words
    /// </summary>
    public List<SipebiLinkedWord> GetLinkedWords() => sipebiLinkedWords;

    /// <summary>
    /// The function to prepare all the linked word items
    /// </summary>
    public Task PrepareLinkedWords() {
      return Task.Run(() => {
        //Load formal words from other sources
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string customDir = Path.Combine(baseDir, DH.DefaultCustomFolderName);
        Directory.CreateDirectory(customDir);
        string filePath = Path.Combine(customDir, DH.DefaultTiedWordsFileName + DH.DefaultCustomizationFileExtension);
        if (!File.Exists(filePath))
          return;

        //Create SiPEBI linked words from custom list
        if (sipebiLinkedWords == null)
          sipebiLinkedWords = new List<SipebiLinkedWord>();
        sipebiLinkedWords.Clear();

        List<string> lines = File.ReadAllLines(filePath).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
        foreach (var line in lines) {
          SipebiLinkedWord slw = new SipebiLinkedWord {
            Word = line,
            IsEditable = true,
            Source = "Custom",
          };
          sipebiLinkedWords.Add(slw);
        }
      });
    }

    /// <summary>
    /// The function to clear all the linked word items
    /// </summary>
    public void ClearLinkedWords() {
      if (sipebiLinkedWords != null)
        sipebiLinkedWords.Clear();
    }

    /// <summary>
    /// The list of SiPEBI place words
    /// </summary>
    private static List<SipebiPlaceWord> sipebiPlaceWords { get; set; }

    /// <summary>
    /// The function to get he list of SiPEBI place words
    /// </summary>
    public List<SipebiPlaceWord> GetPlaceWords() => sipebiPlaceWords;

    /// <summary>
    /// The function to prepare all the place word items
    /// </summary>
    public Task PreparePlaceWords() {
      return Task.Run(() => {
        //Load formal words from other sources
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string customDir = Path.Combine(baseDir, DH.DefaultCustomFolderName);
        Directory.CreateDirectory(customDir);
        string filePath = Path.Combine(customDir, DH.DefaultPlaceInfoWordsFileName + DH.DefaultCustomizationFileExtension);
        if (!File.Exists(filePath))
          return;

        //Create SiPEBI place words from custom list
        if (sipebiPlaceWords == null)
          sipebiPlaceWords = new List<SipebiPlaceWord>();
        sipebiPlaceWords.Clear();

        List<string> lines = File.ReadAllLines(filePath).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
        foreach (var line in lines) {
          SipebiPlaceWord spw = new SipebiPlaceWord {
            Word = line,
            IsEditable = true,
            Source = "Custom",
          };
          sipebiPlaceWords.Add(spw);
        }
      });
    }

    /// <summary>
    /// The function to clear all the place word items
    /// </summary>
    public void ClearPlaceWords() {
      if (sipebiPlaceWords != null)
        sipebiPlaceWords.Clear();
    }

    /// <summary>
    /// The list of SiPEBI conjunction subordinative words
    /// </summary>
    private static List<SipebiConjunctionSubordinativeWord> sipebiConjunctionSubordinativeWords { get; set; }

    /// <summary>
    /// The function to get he list of SiPEBI conjunction subordinative words
    /// </summary>
    public List<SipebiConjunctionSubordinativeWord> GetConjunctionSubordinativeWords() => sipebiConjunctionSubordinativeWords;

    /// <summary>
    /// The function to prepare all the conjunction subordinative word items
    /// </summary>
    public Task PrepareConjunctionSubordinativeWords() {
      return Task.Run(() => {
        //Load formal words from other sources
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string customDir = Path.Combine(baseDir, DH.DefaultCustomFolderName);
        Directory.CreateDirectory(customDir);
        string filePath = Path.Combine(customDir, DH.DefaultConjunctionSubordinativeWordsFileName + DH.DefaultCustomizationFileExtension);
        if (!File.Exists(filePath))
          return;

        //Create SiPEBI conjunction subordinative words from custom list
        if (sipebiConjunctionSubordinativeWords == null)
          sipebiConjunctionSubordinativeWords = new List<SipebiConjunctionSubordinativeWord>();
        sipebiConjunctionSubordinativeWords.Clear();

        List<string> lines = File.ReadAllLines(filePath).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
        foreach (var line in lines) {
          SipebiConjunctionSubordinativeWord scsw = new SipebiConjunctionSubordinativeWord {
            Word = line,
            IsEditable = true,
            Source = "Custom",
          };
          sipebiConjunctionSubordinativeWords.Add(scsw);
        }
      });
    }

    /// <summary>
    /// The function to clear all the conjunction subordinative word items
    /// </summary>
    public void ClearConjunctionSubordinativeWords() {
      if (sipebiConjunctionSubordinativeWords != null)
        sipebiConjunctionSubordinativeWords.Clear();
    }

    /// <summary>
    /// The list of SiPEBI definite capital words
    /// </summary>
    private static List<SipebiDefiniteCapitalWord> sipebiDefiniteCapitalWords { get; set; }

    /// <summary>
    /// The function to get he list of SiPEBI definite capital words
    /// </summary>
    public List<SipebiDefiniteCapitalWord> GetDefiniteCapitalWords() => sipebiDefiniteCapitalWords;

    /// <summary>
    /// The function to prepare all the definite capital word items
    /// </summary>
    public Task PrepareDefiniteCapitalWords() {
      return Task.Run(() => {
        //Load formal words from other sources
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string customDir = Path.Combine(baseDir, DH.DefaultCustomFolderName);
        Directory.CreateDirectory(customDir);
        string filePath = Path.Combine(customDir, DH.DefaultDefiniteCapitalWordsFileName + DH.DefaultCustomizationFileExtension);
        if (!File.Exists(filePath))
          return;

        //Create SiPEBI definite capital words from custom list
        if (sipebiDefiniteCapitalWords == null)
          sipebiDefiniteCapitalWords = new List<SipebiDefiniteCapitalWord>();
        sipebiDefiniteCapitalWords.Clear();

        List<string> lines = File.ReadAllLines(filePath).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
        foreach (var line in lines) {
          SipebiDefiniteCapitalWord sdcw = new SipebiDefiniteCapitalWord {
            Word = line,
            IsEditable = true,
            Source = "Custom",
          };
          sipebiDefiniteCapitalWords.Add(sdcw);
        }
      });
    }

    /// <summary>
    /// The function to clear all the definite capital word items
    /// </summary>
    public void ClearDefiniteCapitalWords() {
      if (sipebiDefiniteCapitalWords != null)
        sipebiDefiniteCapitalWords.Clear();
    }

    /// <summary>
    /// The list of SiPEBI ambiguous words
    /// </summary>
    private static List<SipebiAmbiguousWord> sipebiAmbiguousWords { get; set; }

    /// <summary>
    /// The function to get he list of SiPEBI ambiguous words
    /// </summary>
    public List<SipebiAmbiguousWord> GetAmbiguousWords() => sipebiAmbiguousWords;

    /// <summary>
    /// The function to prepare all the ambiguous word items
    /// </summary>
    public Task PrepareAmbiguousWords() {
      return Task.Run(() => {
        //Load words from other sources
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string customDir = Path.Combine(baseDir, DH.DefaultCustomFolderName);
        Directory.CreateDirectory(customDir);
        string filePath = Path.Combine(customDir, DH.DefaultAmbiguousWordsFileName + DH.DefaultCustomizationFileExtension);
        if (!File.Exists(filePath))
          return;

        //Create SiPEBI ambivalent words from custom list
        if (sipebiAmbiguousWords == null)
          sipebiAmbiguousWords = new List<SipebiAmbiguousWord>();
        sipebiAmbiguousWords.Clear();

        List<string> lines = File.ReadAllLines(filePath).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
        foreach (var line in lines) {
          SimpleExpression exp = new SimpleExpression(line, "->");
          if (!exp.IsValid || exp.IsSingular)
            continue;
          List<string> alternativeWords = exp.RightSide.Split('|')
            .Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
          SipebiAmbiguousWord saw = new SipebiAmbiguousWord {
            Word = exp.LeftSide,
            AlternativeWords = alternativeWords,
            IsEditable = true,
            Source = "Custom",
          };
          sipebiAmbiguousWords.Add(saw);
        }
      });
    }

    /// <summary>
    /// The function to clear all the ambivalent word items
    /// </summary>
    public void ClearAmbiguousWords() {
      if (sipebiAmbiguousWords != null)
        sipebiAmbiguousWords.Clear();
    }

    private SipebiDiagnosticError correctWordDivisionFormalityWithMethod(WordDivision wordDiv, bool useCapitalWordChecking) {
      //Correct to use the formal word
      SipebiDiagnosticError diagnosticError = new SipebiDiagnosticError();
      Tuple<bool, FormalWordItem> result = wordDiv.CorrectInformality(IoC.InformalWordStringsKbbi, IoC.InformalWordsKbbi, useCapitalWordChecking);
      if (result.Item1) {
        diagnosticError.OriginalWordDivision = wordDiv;
        diagnosticError.ErrorCode = DH.KbbiInformalWordDiagnosticErrorCode;
        diagnosticError.OriginalElement = wordDiv.OriginalString;
        diagnosticError.CorrectedElement = wordDiv.ReconstructedString;
        return diagnosticError;
      }
      result = wordDiv.CorrectInformality(IoC.CustomInformalWordStrings, IoC.CustomInformalWords, useCapitalWordChecking);
      if (result.Item1) {
        diagnosticError.OriginalWordDivision = wordDiv;
        diagnosticError.ErrorCode = result.Item2.DiagnosticErrorCode;
        diagnosticError.OriginalElement = wordDiv.OriginalString;
        diagnosticError.CorrectedElement = wordDiv.ReconstructedString;
      }
      return diagnosticError;
    }

    private SipebiDiagnosticError correctWordDivisionFormality(WordDivision wordDiv) {
      SipebiDiagnosticError diagnosticError = new SipebiDiagnosticError();
      if ((wordDiv.WordPositionInSentence & WordPositionInSentence.FirstWord) != 0) {
        bool startedWithCapital = char.IsUpper(wordDiv.CleanWordString[0]);
        //If it is the first word, then the possibilities are:
        //1. It is supposed to be written in capital because this is something like name
        //2. It is not supposed to be written in capital but is written in capital because it is the first word
        //3. Not written in capital though it is the first word (false first word)
        if (!startedWithCapital) { //false first word case
                                   //TODO, probabaly want to capitalize, leave it for now
        } else //POSSIBLY true first word case
          diagnosticError = correctWordDivisionFormalityWithMethod(wordDiv, useCapitalWordChecking: true);
      }

      //Not the first word, the treatment is strict, without capital letter checking
      if (!diagnosticError.HasError) //if does not have error the first time, try the second time
        diagnosticError = correctWordDivisionFormalityWithMethod(wordDiv, useCapitalWordChecking: false);
      return diagnosticError; //return whatever is the result
    }

    ///// <summary>
    ///// The list of last diagnostic errors and ambivalency found 
    ///// </summary>
    //public List<SipebiDiagnosticError> LastDiagnosticErrors { get; set; }

    /// <summary>
    /// The most important function of the <see cref="ISipebiDataModel"/> is this function, used to correct a text from an original taxt containing mistakes
    /// </summary>
    /// <returns>The diagnostic report (correction) result</returns>
    public SipebiDiagnosticReport CorrectText(string originalText) {
      List<SipebiDefiniteCapitalWord> definiteCapitalWords = GetDefiniteCapitalWords();
      List<SipebiAmbiguousWord> ambiguousWords = GetAmbiguousWords();
      SipebiDiagnosticReport report = new SipebiDiagnosticReport();
      int numberOfCorrection = 0;
      int numberOfAmbivalency = 0;
      report.StartTime = DateTime.Now;

      //Prepare the diagnostic error list
      List<SipebiDiagnosticError> diagnosticErrors = new List<SipebiDiagnosticError>();

      //If there is no valid text, just return the original text
      if (string.IsNullOrWhiteSpace(originalText)) {
        report.EndTime = DateTime.Now;
        return report;
      }

      //Prepare a list for corrected paragraphs
      List<string> correctedParagraphs = new List<string>();

      //Create paragraphs
      int paragraphNo = 0;
      int charPositionOffset = 0;
      int correctionParagraphPositionOffset = 0; //the paragraph position offset for the correction
      int correctionCharPositionOffset = 0;
      int totalNoOfElements = 0;
      var paragraphs = originalText.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
      foreach(var paragraph in paragraphs) {
        //Skip empty paragraph
        if (string.IsNullOrEmpty(paragraph)) {
          correctedParagraphs.Add(string.Empty);
          charPositionOffset += 2; //to add \r\n to the consideration of the offset position
          correctionParagraphPositionOffset += 2;
          continue;
        }

        //paragraph number is started from 1
        ++paragraphNo;

        //Prepare a list for corrected strings
        List<string> correctedStrings = new List<string>();

        //Create the word divisions from the text
        //DO NOT TRIM at all, later, the position could be shifted pretty badly due to the trim
        var wordDivs = paragraph.Split(' ') //.Trim().Split(' ') //.Select(x => x.Trim()) //NOTE: DO NOT TRIM PER TEXT for it will change the reconstruction later
          .Select(x => new WordDivision(x)).ToList();

        //by calling below function, now all the first words and last words are already detected, so do all the linked words
        int currentParLength = WordDivision.ProcessWordDivisionStream(wordDivs, paragraphNo, charPositionOffset, 
          IoC.SipebiDM.GetLinkedWords(), IoC.SipebiDM.GetPlaceWords(), IoC.SipebiDM.GetConjunctionSubordinativeWords());
        charPositionOffset += currentParLength + 2; //adds the current paragraph length +2 (that is, for \r\n) for the next char position offset

        //reset the correction character position offset before correction
        correctionCharPositionOffset = 0; //when it is final, then adds the final length to the offset

        //Check the streams of word divisions submitted
        for (int i = 0; i < wordDivs.Count; ++i) {
          SipebiDiagnosticError diagnosticError = new SipebiDiagnosticError();
          string currentCorrectedString = string.Empty;
          WordDivision wordDiv = wordDivs[i];
          int finalWordLength = wordDiv.IsNullOrEmpty ? 0 : wordDiv.OriginalString.Length; //to trace the "final" word length for this round for length calculation
          int initialCorrectedCharPosition = correctionParagraphPositionOffset + correctionCharPositionOffset;

          //Cannot process null or empty string
          if (wordDiv.IsNullOrEmpty) {
            correctedStrings.Add(string.Empty);
            //Does not need the addition below when empty, very interesting!
            //if (wordDivs.Count != i - 1) //if not the final element
            //  correctionCharPositionOffset += 1; //then add the character position offset before "continue" (actually, finishing, because it is the last element)
            continue;
          }

          //Non-empty word div increases the number of elements processed
          ++totalNoOfElements;

          //Correct the word division formality first
          diagnosticError = correctWordDivisionFormality(wordDiv);

          if (diagnosticError.HasError) {
            diagnosticError.CorrectedCharPosition = initialCorrectedCharPosition;
            diagnosticErrors.Add(diagnosticError);
            ++numberOfCorrection;
          }

          //Now, try to link the words if this word is found to be the first in the linked word and this is not the end of the stream
          if (wordDiv.LinkedWordPosition == LinkedWordPosition.Front && i < wordDivs.Count - 1) {
            WordDivision nextWordDiv = wordDivs[i + 1];
            SipebiDiagnosticError nextDiagnosticError = correctWordDivisionFormality(nextWordDiv); //correct the next word formality now
            ++i; //skip the next loop TODO, skipping the next loop is a big deal now, make sure that all checking process for single element is completely done

            if (nextDiagnosticError.HasError) {
              //The correction position is the same as the previous word div! This is because this word and the previous word will ultimately be linked
              diagnosticError.CorrectedCharPosition = initialCorrectedCharPosition;
              diagnosticErrors.Add(nextDiagnosticError);
              ++numberOfCorrection;
            }

            currentCorrectedString = wordDiv.ReconstructedString + nextWordDiv.ReconstructedString; //combine them without space

            ++numberOfCorrection;
            SipebiDiagnosticError linkedWordDiagnosticError = new SipebiDiagnosticError {
              OriginalWordDivision = wordDiv,
              ErrorCode = DH.SipebiTiedWordDiagnosticErrorCode,
              OriginalElement = wordDiv.ReconstructedString + " " + nextWordDiv.ReconstructedString, //original element is having string
              CorrectedElement = currentCorrectedString,
              CorrectedCharPosition = initialCorrectedCharPosition,
            };
            diagnosticErrors.Add(linkedWordDiagnosticError);

          } else if (wordDiv.LinkedWordPosition == LinkedWordPosition.Combined) {
            ++numberOfCorrection;
            SipebiDiagnosticError linkedWordDiagnosticError = new SipebiDiagnosticError {
              OriginalWordDivision = wordDiv,
              ErrorCode = DH.SipebiTiedWordDiagnosticErrorCode,
              OriginalElement = wordDiv.ReconstructedString,
              CorrectedCharPosition = initialCorrectedCharPosition,
            };
            diagnosticErrors.Add(linkedWordDiagnosticError);

            wordDiv.CleanWordString = wordDiv.CleanWordString.Replace("-", string.Empty);
            currentCorrectedString = wordDiv.ReconstructedString; //reconstruct after replacement
            linkedWordDiagnosticError.CorrectedElement = currentCorrectedString;
          } else
            currentCorrectedString = wordDiv.ReconstructedString; //immediately reconstruct

          //Now, check if there is any wrong (connected) place word made
          if (wordDiv.WrongPlaceWordType != WrongPlaceWordType.None) {
            SipebiDiagnosticError placeWordDiagnosticError = new SipebiDiagnosticError {
              OriginalWordDivision = wordDiv,
              ErrorCode = DH.SipebiPlaceWordDiagnosticErrorCode,
              OriginalElement = currentCorrectedString,
              CorrectedCharPosition = initialCorrectedCharPosition,
            };
            diagnosticErrors.Add(placeWordDiagnosticError);
            ++numberOfCorrection;

            switch (wordDiv.WrongPlaceWordType) {
              case WrongPlaceWordType.Ke:
              case WrongPlaceWordType.Di:
                currentCorrectedString = currentCorrectedString.Substring(0, 2) + " " + currentCorrectedString.Substring(2);
                break;
              case WrongPlaceWordType.Dari:
                currentCorrectedString = currentCorrectedString.Substring(0, 4) + " " + currentCorrectedString.Substring(4);
                break;
            }

            placeWordDiagnosticError.CorrectedElement = currentCorrectedString;
          }

          //Check if next word only has post word
          bool onlyHasPostWord = true; //for the first time
          while (onlyHasPostWord && i < wordDivs.Count - 1) {//not the last word
            WordDivision nextWordDiv = wordDivs[i + 1];
            onlyHasPostWord = nextWordDiv.OnlyHasPostWord;
            if (onlyHasPostWord) {
              ++i; //skip the next loop TODO is a big deal actually
              ++numberOfCorrection;
              SipebiDiagnosticError detachedMarkDiagnosticError = new SipebiDiagnosticError {
                OriginalWordDivision = wordDiv,
                ErrorCode = DH.SipebiDetachedMarkDiagnosticErrorCode,
                OriginalElement = currentCorrectedString + " " + nextWordDiv.ReconstructedString,
                CorrectedCharPosition = initialCorrectedCharPosition,
              };
              diagnosticErrors.Add(detachedMarkDiagnosticError);

              currentCorrectedString += nextWordDiv.ReconstructedString; //get the reconstructed string now
              detachedMarkDiagnosticError.CorrectedElement = currentCorrectedString;
              //not the last word and the post word char is special "-" 
              //then the next work is supposed to be attached to: not [untung- untungan] but [untung-untungan]
              if (i < wordDivs.Count - 1 && nextWordDiv.PostWord == "-") {
                nextWordDiv = wordDivs[i + 1]; //get the next next word
                if (nextWordDiv.WordWithNoPreWord) { //no preword, has to be connected to the previous word
                  ++i; //skip the next loop
                  ++numberOfCorrection;
                  SipebiDiagnosticError detachedWordDiagnosticError = new SipebiDiagnosticError {
                    OriginalWordDivision = wordDiv,
                    ErrorCode = DH.SipebiDetachedWordDiagnosticErrorCode,
                    OriginalElement = currentCorrectedString + " " + nextWordDiv.ReconstructedString,
                    CorrectedCharPosition = initialCorrectedCharPosition,
                  };
                  diagnosticErrors.Add(detachedWordDiagnosticError);

                  currentCorrectedString += nextWordDiv.ReconstructedString; //get the reconstructed string now
                  detachedWordDiagnosticError.CorrectedElement = currentCorrectedString;
                }
              }
            }
          }

          //Add next conjunction subordinative words detection before adding the current string
          if (currentCorrectedString.EndsWith(",") && i < wordDivs.Count - 1) {
            WordDivision nextWordDiv = wordDivs[i + 1];
            if (nextWordDiv.IsConjunctionSubordinativeWord) {//if the next word is of conjunction subordinative type
              SipebiDiagnosticError conjunctionSubordinativeWordDiagnosticError = new SipebiDiagnosticError {
                OriginalWordDivision = wordDiv,
                ErrorCode = DH.SipebiConjunctionSubordinativeWordDiagnosticErrorCode,
                OriginalElement = currentCorrectedString + " " + nextWordDiv.OriginalString,
                CorrectedCharPosition = initialCorrectedCharPosition,
              };
              diagnosticErrors.Add(conjunctionSubordinativeWordDiagnosticError);

              ++numberOfCorrection; //just add the number of correction, but do not skip the next loop
              currentCorrectedString = currentCorrectedString.Substring(0, currentCorrectedString.Length - 1); //take out the last comma
              conjunctionSubordinativeWordDiagnosticError.CorrectedElement = currentCorrectedString + " " + nextWordDiv.OriginalString;
            }
          }

          //Check if the latest formed string is a definite capital word
          WordDivision reconstructedWordDiv = new WordDivision(currentCorrectedString);
          if (definiteCapitalWords != null &&
            !string.IsNullOrWhiteSpace(reconstructedWordDiv.CleanWordString) && 
            reconstructedWordDiv.CleanWordString.Length > 1 && //must be at least having length of 2
            definiteCapitalWords.Any(x => x.Word.ToLower() == reconstructedWordDiv.CleanWordString.ToLower()) && //it is found in the definite capital word list
            !char.IsUpper(reconstructedWordDiv.CleanWordString[0]) //but its first letter is not written in capital
            ){
            SipebiDiagnosticError definiteCapitalWordDiagnosticError = new SipebiDiagnosticError {
              OriginalWordDivision = wordDiv,
              ErrorCode = DH.SipebiDefiniteCapitalWordDiagnosticErrorCode,
              OriginalElement = reconstructedWordDiv.OriginalString,
              CorrectedCharPosition = initialCorrectedCharPosition,
            };
            diagnosticErrors.Add(definiteCapitalWordDiagnosticError);

            ++numberOfCorrection;
            reconstructedWordDiv.CleanWordString = char.ToUpper(reconstructedWordDiv.CleanWordString[0]) +
              reconstructedWordDiv.CleanWordString.Substring(1);
            definiteCapitalWordDiagnosticError.CorrectedElement = reconstructedWordDiv.ReconstructedString;
            currentCorrectedString = reconstructedWordDiv.ReconstructedString;
          }

          finalWordLength = currentCorrectedString.Length;

          //Check if the latest formed string is an ambiguous word
          if (ambiguousWords != null && 
            !diagnosticError.HasError && //word which previously has been formalized cannot be listed as ambiguous
            !string.IsNullOrWhiteSpace(reconstructedWordDiv.CleanWordString) && 
            ambiguousWords.Any(x => x.Word.ToLower() == reconstructedWordDiv.CleanWordString.ToLower())){
            SipebiAmbiguousWord ambiguousWord = ambiguousWords.First(x => x.Word.ToLower() == reconstructedWordDiv.CleanWordString.ToLower());            
            SipebiDiagnosticError ambiguousWordDiagnosticError = new SipebiDiagnosticError {
              OriginalWordDivision = wordDiv,
              ErrorCode = DH.SipebiAmbiguousWordDiagnosticErrorCode,
              OriginalElement = reconstructedWordDiv.ReconstructedString,
              CorrectedCharPosition = initialCorrectedCharPosition,
              CorrectedElement = "Saran: " + string.Join(" | ", ambiguousWord.AlternativeWords),
            };
            diagnosticErrors.Add(ambiguousWordDiagnosticError);

            numberOfCorrection++;
            numberOfAmbivalency++;
          }

          //Add the reconstructed string to the corrected string list
          correctedStrings.Add(currentCorrectedString);

          //Updates the corrected character position
          correctionCharPositionOffset += finalWordLength;
          if (i != wordDivs.Count - 1) //as long as this is not the final element
            correctionCharPositionOffset++; //then adds the character position offset by 1 for the next space character
        }

        //Reconstruct the string after all the correction
        //Add the reconstructed paragraph to the corrected paragraph list
        correctedParagraphs.Add(string.Join(" ", correctedStrings));

        //Updates the paragraph position offset for the next paragraph, +2 whatever happens
        correctionParagraphPositionOffset += correctionCharPositionOffset + 2;
      }

      //Reconstruct the paragraphs
      string correctedText = string.Join("\r\n", correctedParagraphs);

      //Write and return the report
      report.OriginalText = originalText;
      report.CorrectedText = correctedText;
      report.Errors = diagnosticErrors;
      report.NoOfElements = totalNoOfElements;
      report.NoOfParagraphs = paragraphNo;
      report.CreateReportItems(diagnosticErrors, IoC.DiagnosticErrorInformationList);
      report.EndTime = DateTime.Now;
      return report;
    }

    ///// <summary>
    ///// The function to get the last analysis result
    ///// </summary>
    ///// <returns></returns>
    //public List<SipebiDiagnosticError> GetLastAnalysisResult() => LastDiagnosticErrors;
  }
}


////Add default error information item(s)
//IoC.DiagnosticErrorInformationList.Add(new SipebiDiagnosticErrorInformation {
//  ErrorCode = DH.KbbiInformalWordDiagnosticErrorCode,
//  Error = DH.KbbiInformalWordDiagnosticError,
//  ErrorExplanation = DH.KbbiInformalWordDiagnosticErrorExplanation,
//});
//IoC.DiagnosticErrorInformationList.Add(new SipebiDiagnosticErrorInformation {
//  ErrorCode = DH.SipebiTiedWordDiagnosticErrorCode,
//  Error = DH.SipebiTiedWordDiagnosticError,
//  ErrorExplanation = DH.SipebiTiedWordDiagnosticErrorExplanation,
//});
//IoC.DiagnosticErrorInformationList.Add(new SipebiDiagnosticErrorInformation {
//  ErrorCode = DH.SipebiDefiniteCapitalWordDiagnosticErrorCode,
//  Error = DH.SipebiDefiniteCapitalWordDiagnosticError,
//  ErrorExplanation = DH.SipebiDefiniteCapitalWordDiagnosticErrorExplanation,
//});
//IoC.DiagnosticErrorInformationList.Add(new SipebiDiagnosticErrorInformation {
//  ErrorCode = DH.SipebiMarkNotAttachedDiagnosticErrorCode,
//  Error = DH.SipebiMarkNotAttachedDiagnosticError,
//  ErrorExplanation = DH.SipebiMarkNotAttachedDiagnosticErrorExplanation,
//});
//IoC.DiagnosticErrorInformationList.Add(new SipebiDiagnosticErrorInformation {
//  ErrorCode = DH.SipebiAmbivalentWordDiagnosticErrorCode,
//  Error = DH.SipebiAmbivalentWordDiagnosticError,
//  ErrorExplanation = DH.SipebiAmbivalentWordDiagnosticErrorExplanation,
//});
//IoC.DiagnosticErrorInformationList.Add(new SipebiDiagnosticErrorInformation {
//  ErrorCode = DH.SipebiPlaceWordDiagnosticErrorCode,
//  Error = DH.SipebiPlaceWordDiagnosticError,
//  ErrorExplanation = DH.SipebiPlaceWordDiagnosticErrorExplanation,
//});
//IoC.DiagnosticErrorInformationList.Add(new SipebiDiagnosticErrorInformation {
//  ErrorCode = DH.SipebiConjunctionSubordinativeWordDiagnosticErrorCode,
//  Error = DH.SipebiConjunctionSubordinativeWordDiagnosticError,
//  ErrorExplanation = DH.SipebiConjunctionSubordinativeWordDiagnosticErrorExplanation,
//});
//IoC.DiagnosticErrorInformationList.Add(new SipebiDiagnosticErrorInformation {
//  ErrorCode = DH.SipebiDetachedMarkDiagnosticErrorCode,
//  Error = DH.SipebiDetachedMarkDiagnosticError,
//  ErrorExplanation = DH.SipebiDetachedMarkDiagnosticErrorExplanation,
//});
//IoC.DiagnosticErrorInformationList.Add(new SipebiDiagnosticErrorInformation {
//  ErrorCode = DH.SipebiDetachedWordDiagnosticErrorCode,
//  Error = DH.SipebiDetachedWordDiagnosticError,
//  ErrorExplanation = DH.SipebiDetachedWordDiagnosticErrorExplanation,
//});
