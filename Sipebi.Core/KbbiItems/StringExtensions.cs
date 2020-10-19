using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBBI.Core {
	public static class StringExtensions {
		public static List<int> AllIndexesOf(this string str, string value) {
			if (string.IsNullOrEmpty(value))
				throw new ArgumentException("the string to find may not be empty", "value");
			List<int> indexes = new List<int>();
			for (int index = 0; ; index += value.Length) {
				index = str.IndexOf(value, index);
				if (index == -1)
					return indexes;
				indexes.Add(index);
			}
		}

		public static bool HasNonExtendedAbbreviationsExcept(this string str, string exception) {
			return StringHelper.NonAbbreviations.Keys
				.Intersect(StringHelper.CleanUpText(str).ToLower().Split(' '))
				.Any(x => x != exception);
		}

		public static bool HasExtendedAbbreviationsExcept(this string str, string exception) {
			if (!str.Contains(' ')) //if only one word, always excluded from checking
				return false;
			return StringHelper.Abbreviations.Values
				.Intersect(StringHelper.CleanUpText(str).Split(' ') //split
					.Where(x => !StringHelper.SpeciallyAllowedNonAbbreviated.Contains(x)) //and the split item is not in the specially allowed non abbreviated like "Yang"
					.Select(x => x.ToLower())) //then select the intersection ToLower()
				.Any(x => x != exception); //and return everything but the exception
		}

		public static bool HasNonExtendedAbbreviations(this string str) {
			return StringHelper.NonAbbreviations.Keys
				.Intersect(StringHelper.CleanUpText(str).ToLower().Split(' ')).Any();
		}

		public static bool HasExtendedAbbreviations(this string str) {
			if (!str.Contains(' ')) //if only one word, always excluded from checking
				return false;
			return StringHelper.Abbreviations.Values
				.Intersect(StringHelper.CleanUpText(str).Split(' ') //split
					.Where(x => !StringHelper.SpeciallyAllowedNonAbbreviated.Contains(x)) //and the split item is not in the specially allowed non abbreviated like "Yang"
					.Select(x => x.ToLower())) //then select the intersection ToLower()
				.Any(); //and return everything
		}

		public static List<string> GetNonExtendedAbbreviations(this string str) {
			return StringHelper.NonAbbreviations.Keys
				.Intersect(StringHelper.CleanUpText(str).ToLower().Split(' '))
				.ToList();
		}

		//WARNING! It is currently used only for getting the extended abbreviations AFTER special checking
		//There might still be something missing in its logic!
		public static List<string> GetExtendedAbbreviations(this string str) {
			return StringHelper.Abbreviations.Values
				.Intersect(StringHelper.CleanUpText(str).Split(' ') //split
					.Where(x => !StringHelper.SpeciallyAllowedNonAbbreviated.Contains(x)) //and the split item is not in the specially allowed non abbreviated like "Yang"
					.Select(x => x.ToLower())) //then select the intersection ToLower()
				.ToList();
		}

		public static bool StrictlyContains(this string str, List<string> phrases) {
			return phrases.Any(x => str.StrictlyContains(x));
		}

		public static bool StrictlyContains(this string str, string phrase) {
			if (str == null || phrase == null)
				return false;
			string text = str.Replace("[", string.Empty).Replace("]", string.Empty);
			if (text.Length < phrase.Length) //cannot be parsed too
				return false;

			if (text == phrase) //very special case, input is exactly the same awal
				return true;

			if (!phrase.Contains(' ')) { //single word, may contain affixes
				var subs = text.Split(' ');
				for (int i = 0; i < subs.Length; ++i) {
					if (StringHelper.StrictlyContainsPerWord(subs[i], phrase)) //changed from simple ShouldWordBeChanged to StrictlyContainsPerWord
						return true;
					else if (subs[i].Contains('-')) { //kata ulang dijadikan contoh untuk kata bukan ulang
						string[] subsubs = subs[i].Split('-');
						for (int j = 0; j < subsubs.Length; ++j)
							if (StringHelper.StrictlyContainsPerWord(subsubs[j], phrase)) //changed from simple ShouldWordBeChanged to StrictlyContainsPerWord
								return true;
					}
				}
				return false;
			} 

			string loopedInput = text;
			List<int> indices = text.AllIndexesOf(phrase);
			if (indices == null || indices.Count <= 0)
				return false;

			for (int i = indices.Count - 1; i >= 0; --i) {
				int index = indices[i];
				if (index + phrase.Length == text.Length) { //at the very end, check the beginning
					if (!char.IsLetter(text[index - 1])) //valid case, replace this portion only
						return true;
				} else if (index == 0) { //at the very beginning, check the end
					if (!char.IsLetter(text[phrase.Length]))
						return true;
				} else { //in between
					if (!char.IsLetter(text[index - 1]) && !char.IsLetter(text[index + phrase.Length]))
						return true;
				}
			}

			return false;
		}

    /// <summary>
    /// Converts <see cref="string"/> from foo_bar to FooBar
    /// </summary>
    public static string ToCamelCase(this string str) {      
      List<int> indices = str.AllIndexesOf("_"); //get all the underscore
      char[] chars = str.ToCharArray(); //to store the conversion from foo_bar to FooBar
      foreach (int index in indices)
        if (str.Length > index + 2) //name blabla_, length = 7, index = 6. If index is 6, length has to be 8 to mark that is it not the end
          chars[index + 1] = char.ToUpper(str[index + 1]);
      chars[0] = char.ToUpper(str[0]); //assuming index 0 always exists
      string camelCaseName = new string(chars); //now it looks like Foo_Bar
      camelCaseName = camelCaseName.Replace("_", string.Empty); //now it looks like FooBar
      return camelCaseName;
    }

    /// <summary>
    /// Converts <see cref="string"/> from FooBar to foo_bar
    /// </summary>
    public static string ToUnderscore(this string str) {
      StringBuilder sb = new StringBuilder();
      bool firstTime = true;
      foreach (var ch in str)
        if (firstTime) {
          sb.Append(char.ToLower(ch));
          firstTime = false;
        } else if (char.IsUpper(ch)) { //upper case, adds "_"
          sb.Append('_');
          sb.Append(char.ToLower(ch));
        } else
          sb.Append(ch);
      return sb.ToString();
    }
  }
}
