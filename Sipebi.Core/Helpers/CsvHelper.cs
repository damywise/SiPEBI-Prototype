using System.Text;

namespace Sipebi.Core {
  public class CsvHelper {
    public static void AddCsvElement(StringBuilder sb, string element, bool isLast = false) {
      if (!string.IsNullOrWhiteSpace(element))
        sb.Append(element.AsCsvStringValue());
      if(!isLast)
        sb.Append(",");
    }

    public static void AddCsvElement(StringBuilder sb, long element, bool isLast = false) {
      sb.Append(element.ToString());
      if (!isLast)
        sb.Append(",");
    }
  }
}
