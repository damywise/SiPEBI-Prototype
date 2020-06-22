using SipebiPrototype.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KBBI.Core {
  /// <summary>
  /// The position of the word with respect to its linked word (if there is any)
  /// </summary>
  public enum LinkedWordPosition {
    /// <summary>
    /// Not a linked word, default type
    /// </summary>
    None = 0,

    /// <summary>
    /// Front part of supposedly linked words like "antar" in "antar muka"
    /// </summary>
    Front,

    /// <summary>
    /// Back part of supposedly linked words like "muka" in "antar muka"
    /// </summary>
    Back,

    /// <summary>
    /// The linked word itself, though it is not supposed to, like "swa-karya" which is supposed to be written as "swakarya"
    /// </summary>
    Combined,
  }

  /// <summary>
  /// The bit-enumeration (flags) to indicate property of a word in a sentence. SiPEBI item.
  /// </summary>
  [Flags]
  public enum WordPositionInSentence {
    /// <summary>
    /// Default flag, nothing special about this word's position in a sentence
    /// </summary>
    None = 0, 

    /// <summary>
    /// Flag to indicate the first word in a sentence, so that we know if we can use capital/not for the word
    /// </summary>
    FirstWord = 1,

    /// <summary>
    /// Flag to indicate the last word in a sentence, so that we know that the next word (if there is any) can use capital letter for the first letter
    /// </summary>
    LastWord = 2,
  }

  /// <summary>
  /// The enum used to detect wrong place word writing, such as "dimana, kemana, darimana"
  /// </summary>
  public enum WrongPlaceWordType {
    /// <summary>
    /// This word is not a wrong place word
    /// </summary>
    None = 0,

    /// <summary>
    /// This word is a wrong "ke" place word
    /// </summary>
    Ke = 1,

    /// <summary>
    /// This word is a wrong "di" place word
    /// </summary>
    Di = 2,

    /// <summary>
    /// This word is a wrong "dari" place word
    /// </summary>
    Dari = 3,
  }

  /// <summary>
  /// The class used to represent a word and its surrounding characters (i.e. a word surrounded by other characters like "(layar,")
  /// </summary>
  public class WordDivision {
    /// <summary>
    /// Pre-word "item" in the sub-phrase (that is, <see cref="OriginalString"/> of) word division, like "(" in the "(layar,"
    /// </summary>
    public string PreWord { get; set; } = string.Empty; //like (

    /// <summary>
    /// Post-word "item" in the sub-phrase (that is, <see cref="OriginalString"/> of) word division, like "," in the "(layar,"
    /// </summary>
    public string PostWord { get; set; } = string.Empty; //like ,

    /// <summary>
    /// The original sub-phrase in the word division, like "(layar,"
    /// </summary>
    public string OriginalString { get; set; } //like (layar, 

    /// <summary>
    /// The clean word in the sub-phrase of a word division, like "layar" in the "(layar,"
    /// </summary>
    public string CleanWordString { get; set; } //like layar		

    /// <summary>
    /// The reconstructed sub-phrase by making use of the current <see cref="PreWord"/> + <see cref="CleanWordString"/> + <see cref="PostWord"/>
    /// of this <see cref="WordDivision"/>. SiPEBI property.
    /// </summary>
    public string ReconstructedString => PreWord + CleanWordString + PostWord; //Added for SiPEBI

    /// <summary>
    /// The property to indicate if this <see cref="WordDivision"/> is part of a "linked word". Like "antar" or "muka" for "antar muka".
    /// See <see cref="LinkedWordPosition"/> <see cref="enum"/> for more details.
    /// </summary>
    public LinkedWordPosition LinkedWordPosition { get; set; } = LinkedWordPosition.None; //Added for SiPEBI

    /// <summary>
    /// The property to indicate if this <see cref="WordDivision"/> has special place in a sentence. 
    /// See <see cref="WordPositionInSentence"/> <see cref="enum"/> for more details.
    /// </summary>
    public WordPositionInSentence WordPositionInSentence { get; set; } = WordPositionInSentence.None; //Added for SiPEBI

    /// <summary>
    /// The property to indicate if this <see cref="WordDivision"/> is a place word with wrong arrangement
    /// </summary>
    public WrongPlaceWordType WrongPlaceWordType { get; set; } = WrongPlaceWordType.None; //Added for SiPEBI

    /// <summary>
    /// The property to indicate if this <see cref="WordDivision"/> is only having a post-word without any word or pre-word
    /// </summary>
    public bool OnlyHasPostWord => string.IsNullOrEmpty(CleanWordString) && !string.IsNullOrWhiteSpace(PostWord);

    /// <summary>
    /// The property to indicate if this <see cref="WordDivision"/> is having word but with no pre-word
    /// </summary>
    public bool WordWithNoPreWord => string.IsNullOrEmpty(PreWord) && !string.IsNullOrWhiteSpace(CleanWordString);

    /// <summary>
    /// The property to indicate if this word is a conjunction subordinative word
    /// </summary>
    public bool IsConjunctionSubordinativeWord { get; set; }

    /// <summary>
    /// The property to indicate the paragraph number of this <see cref="WordDivision"/> with respect to the whole text
    /// </summary>
    public int ParagraphOffset { get; set; }

    /// <summary>
    /// The property to indicate the character start position of this <see cref="WordDivision"/> with respect to the whole text
    /// </summary>
    public int CharPositionOffset { get; set; }

    /// <summary>
    /// The property to indicate the element number of this <see cref="WordDivision"/> with respect to the paragraph where this <see cref="WordDivision"/> is
    /// </summary>
    public int ElementNo { get; set; } = -1;

    ///// <summary>
    ///// The flag to indicate if the convertion from sub-phrase to a <see cref="WordDivision"/> is successful
    ///// </summary>
    //public bool IsValid { get; private set; } 

    private static List<char> preCharsOmitted = new List<char> {
      '(', '\'', '"', '[', '{',
      '-'
    };
    private static List<char> postCharsOmitted = new List<char> {
      '?', ',', '!', '.', ')', ':', '-', '\'', '"', '}', ']', ';'
    };

    /// <summary>
    /// Default constructor
    /// </summary>
    public WordDivision() { }

    /// <summary>
    /// Constructor which takes a subphrase (like [(layar,] and converts it to a <see cref="WordDivision"/>)
    /// </summary>
    /// <param name="subphrase">The input phrase to be converted into <see cref="WordDivision"/></param>
    public WordDivision(string subphrase) {
      if (string.IsNullOrWhiteSpace(subphrase))
        return;
      OriginalString = subphrase;

      int postIndexEvaluated = subphrase.Length - 1;
      bool hasPostChar = true; //for firstTime entry
      while (postIndexEvaluated >= 0 && hasPostChar) {
        hasPostChar = postCharsOmitted.Contains(subphrase[postIndexEvaluated]);
        if (hasPostChar)
          PostWord = string.Concat(subphrase[postIndexEvaluated], PostWord);
        --postIndexEvaluated;
      }

      int startIndex = 0;
      bool hasPreChar = true; //for firstTime entry
      while (startIndex < postIndexEvaluated && hasPreChar) { //cannot detect what has been detected by PostWord
        hasPreChar = preCharsOmitted.Contains(subphrase[startIndex]);
        if (hasPreChar) {
          PreWord = string.Concat(PreWord, subphrase[startIndex]);
          ++startIndex;
        }
      }

      int length = subphrase.Length - startIndex - PostWord.Length;
      CleanWordString = length <= 0 ? string.Empty : //fail to convert to cleanword when length is less than or equal to zero
          subphrase.Substring(startIndex, length);

      //if (length > 0) //condition no longer needed
      //IsValid = true;
    }

    public bool IsNullOrEmpty => string.IsNullOrEmpty(OriginalString);

    public override string ToString() => OriginalString;

    /// <summary>
    /// Function to correct informal word using a list of formal word items and its (informal word) string list.
    /// If the clean word is corrected, returns true.
    /// </summary>
    /// <param name="itemStrings">The informal word string list corresponds to the <see cref="items"/></param>
    /// <param name="items">The list of <see cref="FormalWordItem"/> to check the clean word of the <see cref="WordDivision"/> against</param>
    /// <param name="useCapitalWordChecking">true when the clean word might be written in capital version (originally non-capitalized word)</param>
    public Tuple<bool, FormalWordItem> CorrectInformality(List<string> itemStrings, List<FormalWordItem> items, bool useCapitalWordChecking = false) {
      //in any case, check for both capitalized version and non-capitalized version in the list
      bool isInformal = itemStrings.Any(x => x == CleanWordString);
      if (isInformal) { //found, immediately changes this without anything else to be done
        var fwItem = items.FirstOrDefault(x => x.InformalWord == CleanWordString);
        CleanWordString = fwItem.FormalWord;
        return new Tuple<bool, FormalWordItem>(true, fwItem);
      }
      if (!useCapitalWordChecking) 
        return new Tuple<bool, FormalWordItem>(false, null); //without usage of capital word checking, then immediately returns false here
      isInformal = itemStrings.Any(x => x == CleanWordString.ToLower()); //get lower-cased version
      if (isInformal) { //found in the lower-cased version
        var fwItem = items.FirstOrDefault(x => x.InformalWord == CleanWordString.ToLower());
        CleanWordString = fwItem.FormalWord;
        if (CleanWordString.Length > 1)
          CleanWordString = CleanWordString[0].ToString().ToUpper() //re-capitalize the first letter
            + CleanWordString.Substring(1);
        return new Tuple<bool, FormalWordItem>(true, fwItem);
      }
      return new Tuple<bool, FormalWordItem>(false, null);
    }

    /// <summary>
    /// When <see cref="List{T}"/> of <see cref="WordDivision"/>(s) and 
    /// <see cref="List{T}"/> of <see cref="SipebiLinkedWord"/>(s) are streamed to this method, 
    /// the <see cref="LinkedWordPosition"/> and <see cref="WordPositionInSentence"/> of the <see cref="WordDivision"/>(s)
    /// will be updated
    /// </summary>
    /// <param name="wordDivisions">The <see cref="List{T}"/> of <see cref="WordDivision"/>(s) streamed to this method</param>
    /// <param name="paragraphOffset">The paragraph number (offset) for this current stream</param>
    /// <param name="charPositionOffset">The character position offset at the start of this stream</param>
    /// <param name="linkedWords">The <see cref="List{T}"/> of <see cref="SipebiLinkedWord"/>(s) streamed to this method</param>
    /// <param name="placeWords">The <see cref="List{T}"/> of <see cref="SipebiPlaceWord"/>(s) streamed to this method</param>
    /// <param name="conjunctionSubordinativeWords">The <see cref="List{T}"/> of <see cref="SipebiConjunctionSubordinativeWord"/>(s) streamed to this method</param>
    public static int ProcessWordDivisionStream(List<WordDivision> wordDivisions, 
      int paragraphOffset, int charPositionOffset, List<SipebiLinkedWord> linkedWords, 
      List<SipebiPlaceWord> placeWords, List<SipebiConjunctionSubordinativeWord> conjunctionSubordinativeWords) {
      if (wordDivisions == null || wordDivisions.Count <= 0)
        return 0;
      bool lastWordDivWasLastWord = false;
      bool lastWordDivWasFirstLinkedWord = false;
      int accummulatedCharPosition = 0;
      int elementNo = 1; //element number is started from 1, not 0
      for (int i = 0; i < wordDivisions.Count; ++i) {
        WordDivision wordDivision = wordDivisions[i];
        wordDivision.ParagraphOffset = paragraphOffset;
        wordDivision.CharPositionOffset = charPositionOffset + accummulatedCharPosition;
        accummulatedCharPosition += wordDivision.IsNullOrEmpty ? 1 : wordDivision.OriginalString.Length + 1; //the accummulated character position would be this word division length + 1 (that is, space)
        if (i == wordDivisions.Count - 1) //if this is the last element, then 
          accummulatedCharPosition--; //minus the accummulated char position by 1 for last element does not have extra space
        string lowerCleanWord = wordDivision.CleanWordString?.ToLower();

        if (string.IsNullOrEmpty(lowerCleanWord))
          continue; //cannot progress further than this if the lower clean word is null
        wordDivision.ElementNo = elementNo++; //only adds element number if it survives the lower clean word checking

        //update the word position in sentence
        if (i == 0 || lastWordDivWasLastWord) //the first in the index is definitely the first word
          wordDivision.WordPositionInSentence = WordPositionInSentence.FirstWord;
        lastWordDivWasLastWord = false; //put to false until it is turned below to be true again
        //has a HIGH chance to be the last word of a sentence IF the clean word is not supposed to have "."
        if (wordDivision.PostWord == "." || wordDivision.PostWord == ".\"" || wordDivision.PostWord == ".'" ||
          i == wordDivisions.Count - 1) { 
          //TODO later do the distinction, for now, just assume all of them are the last words in the sentence
          wordDivision.WordPositionInSentence |= WordPositionInSentence.LastWord;
          lastWordDivWasLastWord = true;
        }

        //TODO later, make it better 
        //update the linked word, for now keep it simple, check the toLower version of each word to check if it is linked
        if (lastWordDivWasFirstLinkedWord) {
          wordDivision.LinkedWordPosition = LinkedWordPosition.Back;
          lastWordDivWasFirstLinkedWord = false; //linked word cannot be doubled
          continue;
        }
        lastWordDivWasFirstLinkedWord = false;

        //The first type of linked word, having space in between
        if (linkedWords != null) {
          if (linkedWords.Any(x => x.Word.ToLower() == lowerCleanWord) &&
            (string.IsNullOrWhiteSpace(wordDivision.PostWord) || wordDivision.PostWord == "-") &&
            i < wordDivisions.Count - 1) {
            WordDivision nextDiv = wordDivisions[i + 1];
            if (string.IsNullOrWhiteSpace(nextDiv.PreWord)) { //next word must not have pre-word for this to be a valid front linked word
              wordDivision.LinkedWordPosition = LinkedWordPosition.Front;
              lastWordDivWasFirstLinkedWord = true;
            }
          }

          //Second type of linked word, combined, having dash in the middle -> "antar-muka"
          if (linkedWords.Any(x => lowerCleanWord.StartsWith(x.Word.ToLower() + "-"))) {
            //TODO, currently, very simple logic is used
            wordDivision.LinkedWordPosition = LinkedWordPosition.Combined;
          }
        }

        //Add place words detection        
        string cutLowerCleanWord;
        if (placeWords != null) {
          if (lowerCleanWord.StartsWith("ke") && lowerCleanWord.Length > 2) {
            cutLowerCleanWord = lowerCleanWord.Substring(2);
            if (placeWords.Any(x => x.Word.ToLower() == cutLowerCleanWord))
              wordDivision.WrongPlaceWordType = WrongPlaceWordType.Ke;
          } else if (lowerCleanWord.StartsWith("di") && lowerCleanWord.Length > 2) {
            cutLowerCleanWord = lowerCleanWord.Substring(2);
            if (placeWords.Any(x => x.Word.ToLower() == cutLowerCleanWord))
              wordDivision.WrongPlaceWordType = WrongPlaceWordType.Di;
          } else if (lowerCleanWord.StartsWith("dari") && lowerCleanWord.Length > 4) {
            cutLowerCleanWord = lowerCleanWord.Substring(4);
            if (placeWords.Any(x => x.Word.ToLower() == cutLowerCleanWord))
              wordDivision.WrongPlaceWordType = WrongPlaceWordType.Dari;
          }
        }

        //Add conjunction subordinative word detection
        if (conjunctionSubordinativeWords != null) 
          if (conjunctionSubordinativeWords.Any(x => x.Word.ToLower() == lowerCleanWord))
            wordDivision.IsConjunctionSubordinativeWord = true;        
      }

      //return the accummulated character position through the whole streaming process
      return accummulatedCharPosition;
    }
  }
}