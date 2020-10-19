using Sipebi.Core;
using System.Collections.Generic;
using System.Linq;

namespace KBBI.Core {
  public class BasePrintHelper {
    //public const string PrintResultsFolderName = "PrintResults";
    //public const string ForLiviFolderName = "ForLivi";
    //public static CetakViewModel GetCetakViewModel(PrintSetting setting) {
    //  CetakViewModel cetak = new CetakViewModel {
    //    BarisHurufAwal = setting.DropCapLinesToDrop,
    //    JarakHurufAwal = setting.DropCapDistance,
    //    IndeksAwalContohCetak = setting.PreviewStart,
    //    JenisTulisan = setting.FontFamily,
    //    JumlahContohCetak = setting.PreviewIndex,
    //    MarginAtas = setting.TopMargin,
    //    MarginBawah = setting.BottomMargin,
    //    MarginKanan = setting.RightMargin,
    //    MarginKiri = setting.LeftMargin,
    //    SpasiAntarKolom = setting.ColumnsSpacing,
    //    NamaFile = setting.FileName,
    //    UkuranTulisan = setting.FontSize,
    //    KasusTes = setting.IsTestCase > 0 ? "Ya" : "Tidak",
    //    MulaiGanjil = setting.IsOddStart == null ||
    //      setting.IsOddStart.Value == 1 ? "Ya" : "Tidak",
    //    NomorHalamanAwal = setting.StartPageNo,
    //    CetakAbjadAwalPertama = setting.FirstDropCapPrinted == null ||
    //      setting.FirstDropCapPrinted.Value == 1 ? "Ya" : "Tidak",
    //    TinggiHalaman = setting.PageHeight,
    //    LebarHalaman = setting.PageWidth,
    //    OffsetTinggiHalaman = setting.PageHeightOffset,
    //    OffsetLebarHalaman = setting.PageWidthOffset,
    //    OffsetMarginKiri = setting.LeftMarginOffset,
    //    OffsetMarginKanan = setting.RightMarginOffset,
    //    OffsetMarginAtas = setting.TopMarginOffset,
    //    OffsetMarginBawah = setting.BottomMarginOffset,
    //    TanpaKepalaHalaman = setting.IsHeaderless == null ||
    //      setting.IsHeaderless.Value == 0 ? "Tidak" : "Ya",
    //  };
    //  return cetak;
    //}

    private static Dictionary<char, char> replacedCharsDict = new Dictionary<char, char> {
      { 'à', 'a' }, { 'ā', 'a' }, { 'ä', 'a' },
      { 'ç', 'c' },
      { 'ḍ', 'd' },
      { 'é', 'e' }, { 'ē', 'e' }, { 'è', 'e' }, { 'ê', 'e' },
      { 'ḥ', 'h' },
      { 'î', 'i' }, { 'ī', 'i' },
      { 'ñ', 'n' },
      { 'ö', 'o' }, { 'ô', 'o' },
      { 'ṡ', 's' }, { 'ṣ', 's' },
      { 'ṭ', 't' },
      { 'ü', 'u' }, { 'ū', 'u' },
      { 'ý', 'y' },
      { 'ẓ', 'z' }, { 'ż', 'z' },
    };

    private static List<string> skippedStringsList = new List<string> {
      "-", "’", "'"
    };

    private static string getCleanedWord(string word) {
      foreach (string skippedString in skippedStringsList)
        if (word.StartsWith(skippedString) && word.Length > skippedString.Length)
          word = word.Substring(skippedString.Length);
      return word;
    }

    public static char GetFirstCharWithChecking(string word) {
      string lowerWord = getCleanedWord(word.ToLower());
      return replacedCharsDict.ContainsKey(lowerWord[0]) ? replacedCharsDict[lowerWord[0]] : lowerWord[0];
    }

    public static string GetCompleteUsedWord(string word) {
      string usedWord = word.Trim();
      return string.IsNullOrWhiteSpace(usedWord) ? usedWord : GetUsedWord(getCleanedWord(usedWord.ToLower()));
    }

    public static string GetUsedWord(string word) {
      //"répondez s’il vous plaît"
      //	"’iṭfatur-raḥmān"
      //	"a’ūżu billāh min żālik"
      //	"accent circonflêxe""
      //	"Allāh hāfiý"
      //	"aṣḥābul-furūḍ aṣḥābul-furūḍ"
      //	"Aufklärung"
      //	"da’wah bil-ḥāl"
      //	"fitnatul-kubrā al-ūla"
      return new string(word.ToLower().Select(x => replacedCharsDict.ContainsKey(x) ? replacedCharsDict[x] : x).ToArray());
    }

    //edited for usage in the Sipebi Prototype
    public static IOrderedQueryable<EntriSipebi> GetAlphabeticallyOrderedSequence(IQueryable<EntriSipebi> entries) {
      return entries
        .ToList()
        .AsQueryable()
        .OrderBy(x => GetCompleteUsedWord(x.entri))
        .ThenBy(x => x.id_hom) //added to ensure "Markus (1)" is printed before "markus (2)"
        ;
    }
  }
}