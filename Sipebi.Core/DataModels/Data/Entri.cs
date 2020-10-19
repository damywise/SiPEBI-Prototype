using Extension.Extractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipebi.Core {
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
      entriSipebi.punya_makna = maknas.Any(x => !string.IsNullOrWhiteSpace(x.makna)) ? 1 : 0;
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
}
