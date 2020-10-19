namespace Sipebi.Core {
  public static class StringExtension {

    public static string AsCsvStringValue(this string input) {
      return "\"" + GetCsvSafeStringValue(input) + "\"";
    }

    public static string GetCsvSafeStringValue(this string input) {
      return string.IsNullOrWhiteSpace(input) ? string.Empty : input.Replace("\"", "\"\"");
    }
  }
}
