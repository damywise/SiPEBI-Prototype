using Extension.Database.Sqlite;
using Extension.Extractor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SipebiPrototype.Core;
using System.Text.RegularExpressions;

namespace SipebiPrototype.DbMaker {
  public partial class DbMakerForm : Form {
    string kbbiReducedConnectionString;
    string kbbiSipebiConnectionString;
    public DbMakerForm() {
      InitializeComponent();
      kbbiReducedConnectionString = ConfigurationManager.ConnectionStrings["KBBIReduced"].ConnectionString;
      kbbiSipebiConnectionString = ConfigurationManager.ConnectionStrings["KBBISipebi"].ConnectionString;
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

    private void buttonCreateEntriSipebiTable_Click(object sender, EventArgs e) {
      DateTime startTime = DateTime.Now;
      List<EntriSipebi> entriSipebis = new List<EntriSipebi>();
      DataTable table = SQLiteHandler.GetFullDataTable(kbbiReducedConnectionString, "Entri");
      List<Entri> entris = BaseExtractor.ExtractList<Entri>(table);
      TimeSpan timeSpan = DateTime.Now - startTime;
      richTextBoxDebug.AppendText("Entri: " + entris.Count + ", Duration: " + timeSpan.TotalMilliseconds.ToString("F4") + " ms" + Environment.NewLine);
      startTime = DateTime.Now;
      table = SQLiteHandler.GetFullDataTable(kbbiReducedConnectionString, "Makna");
      List<Makna> maknas = BaseExtractor.ExtractList<Makna>(table);
      timeSpan = DateTime.Now - startTime;
      richTextBoxDebug.AppendText("Makna: " + maknas.Count + ", Duration: " + timeSpan.TotalMilliseconds.ToString("F4") + " ms" + Environment.NewLine);
      startTime = DateTime.Now;
      SQLiteHandler.ClearTable(kbbiSipebiConnectionString, "EntriSipebi");
      foreach (Entri entri in entris) {
        List<Makna> maknasOfEntri = maknas.Where(x => x.eid == entri.eid).ToList();
        EntriSipebi entriSipebi = entri.ToEntriSipebi(maknasOfEntri);
        entriSipebis.Add(entriSipebi);
      }
      SQLiteHandler.InsertObjects(kbbiSipebiConnectionString, "EntriSipebi", entriSipebis);
      timeSpan = DateTime.Now - startTime;
      richTextBoxDebug.AppendText("Entri Sipebi: " + entriSipebis.Count + ", Duration: " + timeSpan.TotalMilliseconds.ToString("F4") + " ms" + Environment.NewLine);
    }

    private void buttonCreateFormalWordItemTable_Click(object sender, EventArgs e) {
      DateTime startTime = DateTime.Now;
      DataTable table = SQLiteHandler.GetFullDataTable(kbbiSipebiConnectionString, "EntriSipebi");
      List<EntriSipebi> entriSipebis = BaseExtractor.ExtractList<EntriSipebi>(table);
      TimeSpan timeSpan = DateTime.Now - startTime;
      richTextBoxDebug.AppendText("Entri Sipebi: " + entriSipebis.Count + ", Duration: " + timeSpan.TotalMilliseconds.ToString("F4") + " ms" + Environment.NewLine);
      //Get all the entris from KBBI which are considered informal
      startTime = DateTime.Now;
      var informalEntris = entriSipebis.Where(x => !string.IsNullOrWhiteSpace(x.entri_rujuk) && x.jenis_rujuk == "→").ToList();
      SQLiteHandler.ClearTable(kbbiSipebiConnectionString, "FormalWordItem");
      List<FormalWordItem> formalWords = new List<FormalWordItem>();
      foreach (EntriSipebi entriSipebi in informalEntris) { //for each Entri Sipebi, do something
        int angkaMaknaInformal, angkaMaknaFormal;
        string informalWord = getCleanPhrase(entriSipebi.entri, out angkaMaknaInformal).Trim();
        string formalWord = getCleanPhrase(entriSipebi.entri_rujuk, out angkaMaknaFormal).Trim();
        if (angkaMaknaInformal > 0) {//the informal word is a homonim
          //If all the homonims are informals, likely they have different formal words
          //If any of the homonim is formal, then they cannot be changed
          continue; //conclusion: as long as it is homonim, we cannot know what is the formal word for it
          //if (entriSipebis.Any(x => x.id_entri == informalWord &&
          // (string.IsNullOrWhiteSpace(x.entri_rujuk) || x.jenis_rujuk != "→")))
          //  continue; //it has homonim which is formal
        }
        if (formalWords.Any(x => x.InformalWord.Equals(informalWord))) //item already existed, cannot be duplicated
          continue;
        FormalWordItem wordItem = new FormalWordItem {
          InformalWord = informalWord,
          FormalWord = formalWord,
          IsEditable = 0,
          Source = "KBBI V"
        };
        formalWords.Add(wordItem);
      }
      SQLiteHandler.InsertObjects(kbbiSipebiConnectionString, "FormalWordItem", formalWords);
      timeSpan = DateTime.Now - startTime;
      richTextBoxDebug.AppendText("Formal Words: " + formalWords.Count + ", Duration: " + timeSpan.TotalMilliseconds.ToString("F4") + " ms" + Environment.NewLine);
    }
  }

  public class Entri {
    public long eid { get; set; }
    public string entri { get; set; }
    public string entri_var { get; set; }
    public string jenis { get; set; }
    public string lafal { get; set; }
    public string silabel { get; set; }
    public long induk { get; set; }
    public string jenis_rujuk { get; set; }
    public string entri_rujuk { get; set; }
    public string id_entri { get; set; }
    public long id_hom { get; set; }
    public long aktif { get; set; }
    public long lampiran { get; set; }
    public string id_pencarian { get; set; }
    public EntriSipebi ToEntriSipebi(List<Makna> maknas) {
      EntriSipebi entriSipebi = BaseExtractor.Transfer<Entri, EntriSipebi>(this, new Dictionary<string, string>());
      List<string> ragams = maknas.Select(x => x.ragam).Union(maknas.Select(x => x.ragam_var)).Distinct()
        .Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
      List<string> kelases = maknas.Select(x => x.kelas).Distinct().Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
      List<string> bahasas = maknas.Select(x => x.bahasa).Distinct().Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
      List<string> bidangs = maknas.Select(x => x.bidang).Distinct().Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
      List<string> kis = maknas.Select(x => x.ki).Distinct().ToList();
      List<string> penyingkats = maknas.Select(x => x.kp).Union(maknas.Select(x => x.akr)).Distinct().ToList();
      entriSipebi.daftar_ragam = string.Join(";", ragams);
      entriSipebi.daftar_kelas = string.Join(";", kelases);
      entriSipebi.daftar_bahasa = string.Join(";", bahasas);
      entriSipebi.daftar_bidang = string.Join(";", bidangs);
      entriSipebi.jenis_ki = string.Empty; //if an Entri has no Makna, then its jenis_ki will naturally be string.Empty
      if (kis.Any(x => string.IsNullOrWhiteSpace(x))) 
        entriSipebi.jenis_ki += "normal";
      if (kis.Any(x => x == "ki"))
        entriSipebi.jenis_ki += (string.IsNullOrWhiteSpace(entriSipebi.jenis_ki) ? "" : ";") + "ki";
      entriSipebi.jenis_penyingkat = string.Empty; //if an Entri has no Makna, then its jenis_penyingkat will naturally be string.Empty
      if (penyingkats.Any(x => string.IsNullOrWhiteSpace(x)))
        entriSipebi.jenis_penyingkat += "normal";
      if (penyingkats.Any(x => x == "kp"))
        entriSipebi.jenis_penyingkat += (string.IsNullOrWhiteSpace(entriSipebi.jenis_penyingkat) ? "" : ";") + "kp";
      if (penyingkats.Any(x => x == "akr"))
        entriSipebi.jenis_penyingkat += (string.IsNullOrWhiteSpace(entriSipebi.jenis_penyingkat) ? "" : ";") + "akr";
      if (penyingkats.Any(x => x == "sing"))
        entriSipebi.jenis_penyingkat += (string.IsNullOrWhiteSpace(entriSipebi.jenis_penyingkat) ? "" : ";") + "sing";
      return entriSipebi;
    }
  }

  public class Makna {
    public long mid { get; set; }
    public long eid { get; set; }
    public long polisem { get; set; }
    public string ragam { get; set; }
    public string ragam_var { get; set; }
    public string kelas { get; set; }
    public string bahasa { get; set; }
    public string bidang { get; set; }
    public string ki { get; set; }
    public string kp { get; set; }
    public string akr { get; set; }
    public string makna { get; set; }
    public string ilmiah { get; set; }
    public string kimia { get; set; }
    public long aktif { get; set; }
  }
}
