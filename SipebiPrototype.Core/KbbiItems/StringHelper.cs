using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace KBBI.Core {
  public class StringHelper {
    public static Dictionary<string, string> Abbreviations = new Dictionary<string, string> {
			//{ "dl", "dalam" }, //removed as per Bu Dora's instructions on 2017-01-19 15:55 PM WIB
			{ "dng", "dengan" },
      { "dp", "daripada" },
      { "dr", "dari" },
      { "kpd", "kepada" },
      { "krn", "karena" },
      { "msl", "misalnya" },
      { "pd", "pada" },
      { "sbg", "sebagai" },
			//{ "shg", "sehingga" }, //later addition, proven to be wrong!
			{ "spt", "seperti" },
      { "thd", "terhadap" },
      { "tt", "tentang" },
      { "yg", "yang" },
      { "dsb", "dan sebagainya" },
      { "dll", "dan lain-lain" },
      { "tsb", "tersebut" }, //later addition after Bu Dora's instruction on 2018-01-16
      { "dst", "dan seterusnya" }, //later addition after Bu Dora's instruction on 2018-01-16
    };

    public static List<string> AbbreviationsExtension = new List<string> {
      "dll.",
      "dsb.",
    }; //these extensions are to help website display

    //later addition, to support wrong non-Abbreviations
    public static Dictionary<string, string> NonAbbreviations = new Dictionary<string, string> {
      { "dl", "dalam" },
      { "shg", "sehingga" },
    };

    public static List<string> SpeciallyAllowedNonAbbreviated = new List<string>() {
      "Yang", //must be strictly written this way for Tuhan Yang Maha Esa, etc...
		};

    //public static List<string> GetSpeciallyAllowedNonAbbreviated() {
    //	return new List<string>(speciallyAllowedNonAbbreviated);
    //}

    static List<string> reservedKeyWords = new List<string> { "nul", "bin" };
    static List<char> reservedSymbols = new List<char> { '.', '?' };
    public static string GetInBetweenString(string input) {
      return string.IsNullOrWhiteSpace(input) ||
        reservedSymbols.Any(x => input.Contains(x)) ||
        reservedKeyWords.Any(x => input.ToLower().Trim().Equals(x)) ?
        "Cari/Hasil?frasa=" : "entri/";
    }

    public static string GetCleanPhrase(string input) {
      int angka_makna = 0;
      return GetCleanPhrase(input, out angka_makna);
    }

    public static string GetCleanPhrase(string input, out int angka_makna) {
      angka_makna = 0;
      if (string.IsNullOrWhiteSpace(input))
        return null;
      string regexPattern = @"\(\d{1,2}\)";
      string cleanPhrase = input.Trim();
      Match match = Regex.Match(input, regexPattern);
      if (!string.IsNullOrWhiteSpace(match.Value)) {
        var angka_str = match.ToString();
        cleanPhrase = input.Replace(angka_str, string.Empty).Trim();
        angka_makna = Convert.ToInt32(angka_str.Substring(1, angka_str.Length - 2));
      }
      return cleanPhrase;
    }

    public static string GetExtendedPhrase(string input, Dictionary<string, string> abbreviations) {
      if (string.IsNullOrWhiteSpace(input))
        return input;
      string[] subs = input.Split(' ');
      for (int i = 0; i < subs.Length; ++i) {
        var foundAbbrsInWord = abbreviations
          .Where(x => RoughlyContainsPhrase(subs[i], x.Key));
        foreach (var abbr in foundAbbrsInWord)
          if (isTextReplaceable(subs[i], abbr.Key))
            subs[i] = subs[i].Replace(abbr.Key, abbr.Value);
      }
      return string.Join(" ", subs);
    }

    public static string GetExtendedPhrase(string input) {
      return GetExtendedPhrase(input, Abbreviations);
    }

    static List<char> cleanedChars = new List<char> { //- char should not be cleaned
			',', '!', '?', '/', '{', '}', '(', ')', '.', '%', '\'', '"',
      '|', '\\', '[', ']', ';', ':', '<', '>', '&', '_', '“', '”', //added special characters “ and ” which are different from "
			'*', '^', '#', '’', '‘', //added special characters ‘ and ’ which are different from '
		};

    static List<string> acceptableSuffixes = new List<string> {
      "nya",
      "kah",
      "lah",
      "ku",
      "mu", //contoh: [agunan]mu //all possessive pronouns are allowed //tanda bacas are acceptable for the last characters
			"nyalah",
      "mulah",
      "kulah",
      "nyakah",
      "mukah",
      "kukah",
    };

    static List<string> acceptablePrefixes = new List<string> {
      "non", //lembaga pemerintah yg [departemental] dan non[departemental] (spt Dewan Pertimbangan Agung) berpusat di ibu kota pemerintahan
			"se", //benang yang diurai itu digulung se[perut]-se[perut]
			"ber", //(ber)[selesa]
			"(ber)", //(ber)[selesa]
			"ku",
      "kau",
    };

    public static bool IsAffix(string phrase) {
      bool isPrefix = phrase.EndsWith("-") && !phrase.StartsWith("-");
      bool isSuffix = phrase.StartsWith("-") && !phrase.EndsWith("-");
      bool isInterfix = phrase.StartsWith("-") && phrase.EndsWith("-"); //contoh -er-, -el-, dsb...
      bool isCircumfix = !phrase.StartsWith("-") && !phrase.EndsWith("-") && phrase.Contains("--"); //contoh peng--an pada penginapan
      return isPrefix || isSuffix || isInterfix || isCircumfix;
    }

    public static bool StrictlyContainsPerWord(string word, string phrase) {
      bool isPrefix = phrase.EndsWith("-") && !phrase.StartsWith("-");
      bool isSuffix = phrase.StartsWith("-") && !phrase.EndsWith("-");
      bool isInterfix = phrase.StartsWith("-") && phrase.EndsWith("-"); //contoh -er-, -el-, dsb...
      bool isCircumfix = !phrase.StartsWith("-") && !phrase.EndsWith("-") && phrase.Contains("--"); //contoh peng--an pada penginapan

      if (!isPrefix && !isSuffix && !isInterfix && !isCircumfix)
        return ShouldWordBeChanged(word, phrase, isStrict: true); //don't anyhow change the word!

      string usedWord = getCleanCompleteUsedInput(word);
      string usedPhrase = BasePrintHelper.GetUsedWord(phrase); //note that we should not remove the sign here yet...

      if (!isCircumfix) //only circumfix is treated differently!
        usedPhrase = usedPhrase.Replace("-", string.Empty);

      if (isPrefix)
        return usedWord.StartsWith(usedPhrase); //only true if the used word contains used phrase IN FRONT

      if (isSuffix)
        return usedWord.EndsWith(usedPhrase); //only true if the used word contains used phrase IN THE BACK

      if (isInterfix)
        return !usedWord.StartsWith(usedPhrase) && !usedWord.EndsWith(usedPhrase) && usedWord.Contains(usedPhrase);

      //circumfix case
      List<string> phrases = phrase.Split('-')
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .ToList();
      return usedWord.StartsWith(phrases[0]) && usedWord.EndsWith(phrases[1]) && //must be in the circumfix pattern
        usedWord.Length > phrases[0].Length + phrases[1].Length; //but contains any item in between
    }

    private static string getCleanUsedInput(string input) { //to be used in pair with strict ShouldWordBeChanged
      List<char> chars = input.ToCharArray().ToList();
      List<char> intersects = chars.Intersect(cleanedChars).ToList();
      if (intersects.Any())
        foreach (var ch in intersects)
          input = input.Replace(ch.ToString(), string.Empty); //replace all chars in the intersection chars
      return input;
    }

    private static string getCleanCompleteUsedInput(string input) {
      string usedInput = BasePrintHelper.GetCompleteUsedWord(input);
      return getCleanUsedInput(usedInput);
    }

    public static bool ShouldWordBeChanged(string input, string cleanIndukAwalOrEntriAwal, bool isStrict = false) {
      string usedInput = isStrict ? getCleanUsedInput(input) : getCleanCompleteUsedInput(input);
      string usedAwal = isStrict ? cleanIndukAwalOrEntriAwal : BasePrintHelper.GetCompleteUsedWord(cleanIndukAwalOrEntriAwal);
      if (usedInput == usedAwal)
        return true;
      if (usedInput.StartsWith(usedAwal)) //to handle something like keadilannya -> [keadilan]nya
        if (acceptableSuffixes.Contains(usedInput.Substring(usedAwal.Length).ToLower())) //[anugerah]nya is identic with [anugerah]Nya
          return true;
      if (usedInput.EndsWith(usedAwal))
        if (acceptablePrefixes.Any(x => usedInput.Length >= x.Length && usedInput.Substring(0, x.Length).ToLower() == x))
          return true;
      return false;
      //return usedInput == PrintHelper.GetCompleteUsedWord(cleanIndukAwalOrEntriAwal);
    }

    static List<string> cleanedHtmlTags = new List<string> {
      "<i>", "</i>", "<b>", "</b>", "<u>", "</u>", "<sup>", "</sup>", "<sub>", "</sub>"
    };

    public static string CleanUpText(string text) {
      var specChars = text.Intersect(cleanedChars);
      string usedText = text;
      foreach (var ch in specChars)
        usedText = usedText.Replace(ch.ToString(), string.Empty);
      foreach (var tag in cleanedHtmlTags)
        usedText = usedText.Replace(tag.ToString(), string.Empty);
      return usedText;
    }

    public static bool RoughlyContainsPhrase(string text, string phrase) {
      return CleanUpText(text).ToLower().Split(' ').Contains(phrase.ToLower());
    }

    //Replacable texts are in the following format: dl, (dl, dl), (dl)
    private static bool isTextReplaceable(string text, string abbr) {
      Regex rgx = new Regex("[^a-zA-Z0-9 -]");
      text = rgx.Replace(text, string.Empty).Trim();
      return text == abbr;
    }

    private static List<char> preCharsOmitted = new List<char> {
      '(', '-', '\'', '"', '[', '{'
    };
    private static List<char> postCharsOmitted = new List<char> {
      '?', ',', '!', '.', ')', ':', '-', '\'', '"', '}', ']', ';'
    };
    public static WordDivision GetWordDivision(string subphrase) {
      if (string.IsNullOrWhiteSpace(subphrase))
        throw new Exception("Invalid string input!");
      WordDivision wordDiv = new WordDivision { OriginalString = subphrase };
      int startIndex = 0;
      bool hasPreChar = true; //for firstTime entry

      while (startIndex < subphrase.Length && hasPreChar) {
        hasPreChar = preCharsOmitted.Contains(subphrase[startIndex]);
        if (hasPreChar) {
          wordDiv.PreWord = string.Concat(wordDiv.PreWord, subphrase[startIndex]);
          ++startIndex;
        }
      }

      int postIndexEvaluated = subphrase.Length - 1;
      bool hasPostChar = true; //for firstTime entry
      while (postIndexEvaluated > 0 && hasPostChar) {
        hasPostChar = postCharsOmitted.Contains(subphrase[postIndexEvaluated]);
        if (hasPostChar)
          wordDiv.PostWord = string.Concat(subphrase[postIndexEvaluated], wordDiv.PostWord);
        --postIndexEvaluated;
      }

      int length = subphrase.Length - startIndex - wordDiv.PostWord.Length;
      wordDiv.CleanWordString = length <= 0 ? subphrase : //fail to convert to cleanword when length is less than or equal to zero
          subphrase.Substring(startIndex, length);
      return wordDiv;
    }

    public static string GetSupNumberedHtml(string phrase, int no) => no > 0 ? string.Concat(phrase, "<sup>", no, "</sup>") : phrase;

    public static string GetBracketNumberedText(string phrase, int no) => no > 0 ? string.Concat(phrase, " (", no, ")") : phrase;    

    //Unused
    //static List<string> allowableHtmlTags = new List<string> {
    //  "<b>", "</b>", "<i>", "</i>", "<u>", "</u>", "<sub>", "</sub>", "<sup>", "</sup>" };
    //public static bool ContainsOnlyAllowableHtml(string input, out string errorString) {
    //  errorString = string.Empty;
    //  if (string.IsNullOrWhiteSpace(input))
    //    return true;
    //  bool containsHTML = input != HttpUtility.HtmlEncode(input);
    //  if (!containsHTML)
    //    return true;
    //  foreach (string tag in allowableHtmlTags)
    //    input = input.Replace(tag, string.Empty);
    //  Regex regex = new Regex(@"<([a-zA-Z\/][a-zA-Z0-9\/]*)\b[^>]*>(.*?)"); //Check if it still contains html tag like <..> or </..>
    //  if (regex.IsMatch(input)) {//contains something else
    //    errorString = "Terdapat elemen HTML lain pada kotak ini selain <b>, <i>, <u>, <sub>, dan <sup>";
    //    return false;
    //  }
    //  return true;
    //  //bool containsOnlyAllowableHTML = input == HttpUtility.HtmlEncode(input);
    //  //if (containsOnlyAllowableHTML)
    //  //	return true; //if the input at this point is the same as html encode, then it only contains of allowable HTML
    //  //return false;
    //}

    public static List<string> ExemptedStrings = new List<string> {
      "rupa", "rupe"
    };

    public static string ReplaceMaknaOrContohString(string cleanEntri, string str, string strToReplaceWith) {
      string pattern = @"\[(.*?)\]";
      MatchCollection mc = Regex.Matches(str, pattern);
      if (mc.Count != 0) {
        Regex r = new Regex(pattern);
        Match m = r.Match(str);
        if (m != null && m.Value != null) { //at least has length of three to prevent all from being taken away
          int length = m.Value.Length;
          if (length > 3 && !ExemptedStrings.Contains(m.Value.Substring(1, m.Value.Length - 2))) //exempted string not counted
            return r.Replace(str, strToReplaceWith);
          string inside = m.Value.Substring(1, length - 2);
          if (inside == cleanEntri)
            return r.Replace(str, strToReplaceWith);
        }
      }

      return str;
    }

    //Solution from https://dotnet-snippets.de/snippet/roemische-zahlen/1457
    public static string Roman(int number) {
      StringBuilder result = new StringBuilder();
      int[] digitsValues = { 1, 4, 5, 9, 10, 40, 50, 90, 100, 400, 500, 900, 1000 };
      string[] romanDigits = { "I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD", "D", "CM", "M" };
      while (number > 0) {
        for (int i = digitsValues.Count() - 1; i >= 0; i--)
          if (number / digitsValues[i] >= 1) {
            number -= digitsValues[i];
            result.Append(romanDigits[i]);
            break;
          }
      }
      return result.ToString();
    }
  }
}