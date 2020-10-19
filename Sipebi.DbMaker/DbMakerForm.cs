using Extension.Database.Sqlite;
using Extension.Extractor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sipebi.Core;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Extension.Reader;
using System.Xml.Serialization;
using System.IO.Compression;
using Extension.Cryptography;

namespace Sipebi.DbMaker {
  public partial class DbMakerForm : Form {
    string kbbiReducedConnectionString;
    string kbbiSipebiConnectionString;
    public DbMakerForm() {
      InitializeComponent();
      kbbiReducedConnectionString = ConfigurationManager.ConnectionStrings["KBBIReduced"].ConnectionString;
      kbbiSipebiConnectionString = ConfigurationManager.ConnectionStrings["KBBISipebi"].ConnectionString;
      SipebiSettings.InitCrypt();
    }

    private string getCleanPhrase(string input) {
      int angka_makna = 0;
      return getCleanPhrase(input, out angka_makna);
    }

    private string getCleanPhrase(string input, out int angka_makna) {
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

    delegate void PrintDurationInvokeDelegate(string processName, int count, TimeSpan timeSpan);
    public void printDuration(string processName, int count, TimeSpan timeSpan) {
      if (richTextBoxDebug.InvokeRequired) {
        Invoke(new PrintDurationInvokeDelegate(printDuration), new object[] { processName, count, timeSpan });
      } else 
        richTextBoxDebug.AppendText(processName + ": " + count + ", Duration: " + timeSpan.TotalMilliseconds.ToString("F4") + " ms" + Environment.NewLine);
    }

    delegate void EndisFlpInvokeDelegate(bool endis);
    public void endisFlp(bool endis) {
      if (flowLayoutPanelOptions.InvokeRequired) {
        Invoke(new EndisFlpInvokeDelegate(endisFlp), new object[] { endis });
      } else
        flowLayoutPanelOptions.Enabled = endis;
    }

    private async void createEntriSipebiTable() {
      await Task.Run(() => {
        endisFlp(false);
        DateTime startTime = DateTime.Now;
        List<EntriSipebi> entriSipebis = new List<EntriSipebi>();
        DataTable table = SQLiteHandler.GetFullDataTable(kbbiReducedConnectionString, "Entri");
        List<Entri> entris = BaseExtractor.ExtractList<Entri>(table);
        printDuration("Entri", entris.Count, DateTime.Now - startTime);

        startTime = DateTime.Now;
        table = SQLiteHandler.GetFullDataTable(kbbiReducedConnectionString, "Makna");
        List<Makna> maknas = BaseExtractor.ExtractList<Makna>(table);
        printDuration("Makna", maknas.Count, DateTime.Now - startTime);

        startTime = DateTime.Now;
        SQLiteHandler.ClearTable(kbbiSipebiConnectionString, DH.SipebiEntriTableName);
        foreach (Entri entri in entris) {
          List<Makna> maknasOfEntri = maknas.Where(x => x.eid == entri.eid).ToList();
          EntriSipebi entriSipebi = entri.ToEntriSipebi(maknasOfEntri);
          entriSipebis.Add(entriSipebi);
        }
        SQLiteHandler.InsertObjects(kbbiSipebiConnectionString, DH.SipebiEntriTableName, entriSipebis);
        printDuration("Entri Sipebi", entriSipebis.Count, DateTime.Now - startTime);
        endisFlp(true);
      });
    }

    //Used to create Entri Sipebi Table from KBBI database
    //The reduced database here refers to KBBI database with only four tables: entri, makna, contoh, kategori
    private void buttonCreateEntriSipebiTable_Click(object sender, EventArgs e) {
      createEntriSipebiTable();
    }

    private async void createFormalWordItemTableFromTable() {
      await Task.Run(() => {
        endisFlp(false);
        DateTime startTime = DateTime.Now;
        DataTable table = SQLiteHandler.GetFullDataTable(kbbiSipebiConnectionString, DH.SipebiEntriTableName);
        List<EntriSipebi> entriSipebis = BaseExtractor.ExtractList<EntriSipebi>(table);
        printDuration("Entri Sipebi", entriSipebis.Count, DateTime.Now - startTime);

        //Get all the entris from KBBI which are considered informal
        startTime = DateTime.Now;
        var informalEntris = entriSipebis.Where(x => !string.IsNullOrWhiteSpace(x.entri_rujuk) && x.jenis_rujuk == "→").ToList();

        //Clear all the current items
        SQLiteHandler.ClearTable(kbbiSipebiConnectionString, DH.FormalWordItemTableName);

        List<FormalWordItem> formalWords = new List<FormalWordItem>();
        foreach (EntriSipebi entriSipebi in informalEntris) { //for each Entri Sipebi, do something
          int angkaMaknaInformal, angkaMaknaFormal;
          string informalWord = getCleanPhrase(entriSipebi.entri, out angkaMaknaInformal).Trim();
          string formalWord = getCleanPhrase(entriSipebi.entri_rujuk, out angkaMaknaFormal).Trim();
          if (formalWords.Any(x => x.InformalWord.Equals(informalWord))) //item already existed, cannot be duplicated
            continue;
          var homographs = entriSipebis.Except(new List<EntriSipebi> { entriSipebi })
            .Where(x => x.entri.ToLower().Equals(entriSipebi.entri) && x.aktif > 0)
            .ToList();
          FormalWordItem wordItem = new FormalWordItem {
            InformalWord = informalWord,
            FormalWord = formalWord,
            IsEditable = 0,
            Source = "KBBI V",
            //the informal word is a homonim if angkaMaknaInformal > 0
            //If all the homonims are informals, likely they have different formal words
            //If any of the homonim is formal, then they cannot be changed
            //conclusion: as long as it is homonim, we cannot know what is the formal word for it
            HasHomonym = angkaMaknaInformal > 0 ? 1 : 0,
            HasMeaning = entriSipebi.punya_makna != null && entriSipebi.punya_makna != 0 ? 1 : 0,
            HasHomograph = homographs.Any() ? 1 : 0,
            SameFormalWordsInHomographs = homographs.Any() && homographs.All(x => x.jenis_rujuk == "→" && //without homograph.Any(), All() will return true when nothing is in the homgraph
              !string.IsNullOrWhiteSpace(x.entri_rujuk) && x.entri_rujuk.ToLower().Equals(formalWord)) ? 1 : 0,
          };
          wordItem.IsAmbiguous = wordItem.HasMeaning == 1 || wordItem.HasHomonym == 1 ||
            (wordItem.HasHomograph == 1 && wordItem.SameFormalWordsInHomographs != 1) ? 1 : 0;
          formalWords.Add(wordItem);
        }
        formalWords = formalWords.OrderBy(x => x.InformalWord.ToLower()).ToList();
        SQLiteHandler.InsertObjects(kbbiSipebiConnectionString, DH.FormalWordItemTableName, formalWords, new List<string> { "DiagnosticErrorCode" });
        printDuration("Formal Words", formalWords.Count, DateTime.Now - startTime);
        endisFlp(true);
      });
    }

    //Used to create word item table from Entri Sipebi Table
    private void buttonCreateFormalWordItemTableFromTable_Click(object sender, EventArgs e) {
      createFormalWordItemTableFromTable();
    }

    private const string DATETIME_SAVE_FORMAT = "yyyyMMdd_HHmmss_fff";
    public static string LastSavedDataTableDirectory = "";
    private async void createFileFromFormalWordItemTable() {
      string text = string.Empty;
      await Task.Run(() => {
        endisFlp(false);

        DateTime startTime = DateTime.Now;
        DataTable table = SQLiteHandler.GetFullDataTable(kbbiSipebiConnectionString, DH.FormalWordItemTableName);
        List<FormalWordItem> formalWords = BaseExtractor.ExtractList<FormalWordItem>(table);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(FormalWordItem.GetCsvHeaders());
        foreach (var formalWord in formalWords)
          sb.AppendLine(formalWord.ToCsvString());
        text = sb.ToString();
        printDuration("Formal Words", formalWords.Count, DateTime.Now - startTime);
      });

      //Open the save file dialog
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "CSV Files" + "|*" + ".csv";
      saveFileDialog.FileName = "Daftar Takbaku SiPEBI_" + DateTime.Now.ToString(DATETIME_SAVE_FORMAT) + ".csv";
      if (!string.IsNullOrWhiteSpace(LastSavedDataTableDirectory))
        saveFileDialog.InitialDirectory = LastSavedDataTableDirectory;
      DialogResult result = saveFileDialog.ShowDialog();
      if (result == DialogResult.OK || result == DialogResult.Yes) {
        string latestFolder = Path.GetDirectoryName(saveFileDialog.FileName);
        LastSavedDataTableDirectory = latestFolder;
        File.WriteAllText(saveFileDialog.FileName, text);
      }

      endisFlp(true);
    }

    //Used to create file from formal word item table
    private void buttonCreateFileFromFormalWordItemTable_Click(object sender, EventArgs e) {
      createFileFromFormalWordItemTable();
    }

    //private const bool COMPARE_OLD_VS_NEW = false;
    private async void createFormalWordItemTableFromFile() {
      string text = string.Empty;
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "CSV Files" + "|*" + ".csv";
      openFileDialog.Multiselect = false;
      if (!string.IsNullOrWhiteSpace(LastSavedDataTableDirectory))
        openFileDialog.InitialDirectory = LastSavedDataTableDirectory;
      DialogResult result = openFileDialog.ShowDialog();
      if (result != DialogResult.OK && result != DialogResult.Yes)
        return;
      LastSavedDataTableDirectory = Path.GetDirectoryName(openFileDialog.FileName);
      await Task.Run(() => {
        endisFlp(false);

        DateTime startTime = DateTime.Now;
        List<FormalWordItem> formalWords = new List<FormalWordItem>();

        CsvReader reader = new CsvReader();
        reader.Open(openFileDialog.FileName);
        var csvElements = reader.GetCsvElements(reader.CsvText, hasHeader: true);
        for(int i = 0; i < csvElements.Count; ++i) {
          FormalWordItem item = new FormalWordItem();
          item.FromCsvTokens(csvElements[i]);
          formalWords.Add(item);
        }

        //if (COMPARE_OLD_VS_NEW) {
        //  DateTime comparisonStartTime = DateTime.Now;
        //  int differences = 0;
        //  DataTable table = SQLiteHandler.GetFullDataTable(kbbiSipebiConnectionString, DH.FormalWordItemTableName);
        //  List<FormalWordItem> oldFormalWords = BaseExtractor.ExtractList<FormalWordItem>(table);
        //  for(int i = 0; i < oldFormalWords.Count; ++i) 
        //    differences += oldFormalWords[i].IsFullyEqualItem(formalWords[i]) ? 0 : 1;
        //  printDuration("Comparison", differences, DateTime.Now - comparisonStartTime);
        //}

        SQLiteHandler.ClearTable(kbbiSipebiConnectionString, DH.FormalWordItemTableName);
        formalWords = formalWords.OrderBy(x => x.Source == "KBBI V").ThenBy(x => x.InformalWord.Trim().ToLower()).ToList();
        SQLiteHandler.InsertObjects(kbbiSipebiConnectionString, DH.FormalWordItemTableName, formalWords, new List<string> { "DiagnosticErrorCode" });
        printDuration("Formal Words", formalWords.Count, DateTime.Now - startTime);

        endisFlp(true);
      });
    }

    //Used to create database word item table from file
    private void buttonCreateFormalWordItemTableFromFile_Click(object sender, EventArgs e) {
      createFormalWordItemTableFromFile();
    }

    private async void transferFormalWordItemsToWords() {
      await Task.Run(() => {
        endisFlp(false);
        DateTime startTime = DateTime.Now;
        DataTable table = SQLiteHandler.GetFullDataTable(kbbiSipebiConnectionString, DH.FormalWordItemTableName);
        List<FormalWordItem> formalWordItems = BaseExtractor.ExtractList<FormalWordItem>(table);
        printDuration("Formal Word Items", formalWordItems.Count, DateTime.Now - startTime);

        //TODO By right, this transfer should not clear the existing item.
        //Hence, there must be a better way to "merge" the current items with the added ones.
        //But right now, the focus is just to transfer the everything, hence we will 
        // erase the existing items

        //Clear all the current items
        startTime = DateTime.Now;
        SQLiteHandler.ClearTable(kbbiSipebiConnectionString, DH.WordItemTableName);

        //Writes all the new items to the database
        List<WordItem> wordItems = formalWordItems.Select((x, i) => x.ToWordItem((i + 1))).ToList();
        SQLiteHandler.InsertObjects(kbbiSipebiConnectionString, DH.WordItemTableName, wordItems);
        printDuration("Words", wordItems.Count, DateTime.Now - startTime);
        endisFlp(true);
      });
    }
      
    private void buttonTransferFormalWordItemsToWords_Click(object sender, EventArgs e) {
      transferFormalWordItemsToWords();
    }

    private async void serializeWordsTable() {
      await Task.Run(() => {
        endisFlp(false);
        DateTime startTime = DateTime.Now;
        DataTable table = SQLiteHandler.GetFullDataTable(kbbiSipebiConnectionString, DH.WordItemTableName);
        List<WordItem> words = BaseExtractor.ExtractList<WordItem>(table);
        printDuration("Words", words.Count, DateTime.Now - startTime);

        startTime = DateTime.Now;
        KataSipebi kataSipebi = new KataSipebi() { Words = words };
        string filepath = Path.Combine(Application.StartupPath, "kata-raw.sipebi");
        XmlSerializer serializer = new XmlSerializer(typeof(KataSipebi));
        using (FileStream fileStream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write)) {
          serializer.Serialize(fileStream, kataSipebi);
          fileStream.Close();
        }
        printDuration("Serialization", words.Count, DateTime.Now - startTime);
        endisFlp(true);
      });
    }

    private void buttonSerializeWordsTable_Click(object sender, EventArgs e) {
      serializeWordsTable();
    }

    private async void compressEncryptSerializedWordsTable() {
      string filepath = Path.Combine(Application.StartupPath, "kata-raw.sipebi");
      string targetFilepath = Path.Combine(Application.StartupPath, "kata.sipebi");
      if (!File.Exists(filepath))
        return;
      await Task.Run(() => {
        endisFlp(false);
        DateTime startTime = DateTime.Now;
				var bytes = File.ReadAllBytes(filepath);
        var resultBytes = Cryptography.CompressAndEncrypt(bytes);
        using (FileStream fileStream = new FileStream(targetFilepath, FileMode.OpenOrCreate, FileAccess.Write)) {
          fileStream.Write(resultBytes, 0, resultBytes.Length);
          fileStream.Close();
				}
        printDuration("Compress-Encrypt", resultBytes.Length, DateTime.Now - startTime);
        endisFlp(true);
      });
		}

		private void buttonCompressEncryptSerializedWordsTable_Click(object sender, EventArgs e) {
      compressEncryptSerializedWordsTable();
		}

    private async void readSerializeEncryptedCompressedWordsFile() {
      string filepath = Path.Combine(Application.StartupPath, "kata.sipebi");
      string targetFilepath = Path.Combine(Application.StartupPath, "kata-recovered.sipebi");
      if (!File.Exists(filepath))
        return;
      await Task.Run(() => {
        endisFlp(false);
        DateTime startTime = DateTime.Now;
        var bytes = File.ReadAllBytes(filepath);
        var resultBytes = Cryptography.DecryptAndDecompress(bytes);
        File.WriteAllBytes(targetFilepath, resultBytes);
        string serializedString = Encoding.ASCII.GetString(resultBytes);
        MemoryStream memoryStream = new MemoryStream(resultBytes);
        XmlSerializer serializer = new XmlSerializer(typeof(KataSipebi));
        var result = (KataSipebi)serializer.Deserialize(memoryStream);
        printDuration("Recovered", result.Words.Count, DateTime.Now - startTime);
        endisFlp(true);
      });
    }

    private void buttonReadSerializeEncryptedCompressedWordsFile_Click(object sender, EventArgs e) {
      readSerializeEncryptedCompressedWordsFile();
    }

    private async void createCompressEncryptSerializedSettingsFile() {
      await Task.Run(() => {
        endisFlp(false);
        DateTime startTime = DateTime.Now;
        SipebiSettings settings = new SipebiSettings() { DiagnosticErrorInfos = SipebiSettings.DefaultDiagnosticErrorInfos };
        XmlSerializer serializer = new XmlSerializer(typeof(SipebiSettings));
        MemoryStream memoryStream = new MemoryStream();
        serializer.Serialize(memoryStream, settings);       
        var resultBytes = Cryptography.CompressAndEncrypt(memoryStream.ToArray());
        string filepath = Path.Combine(Application.StartupPath, "pengaturan.sipebi");
        using (FileStream fileStream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write)) {
          fileStream.Write(resultBytes, 0, resultBytes.Length);
          fileStream.Close();
        }
        printDuration("Serialization", settings.DiagnosticErrorInfos.Count, DateTime.Now - startTime);
        endisFlp(true);
      });
		}

    private void buttonCreateCompressEncryptSerializedSettingsFile_Click(object sender, EventArgs e) {
      createCompressEncryptSerializedSettingsFile();
    }
	}
}
