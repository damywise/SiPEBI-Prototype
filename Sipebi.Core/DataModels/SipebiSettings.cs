using Extension.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sipebi.Core {
	public class SipebiSettings {
		//1. Now, what must be in the settings?
		//2. Create some function to create basic SiPEBI settings
		//For now, just work first until we have a need to load a SiPEBI settings

		public List<SipebiDiagnosticErrorInformation> DiagnosticErrorInfos { get; set; } = new List<SipebiDiagnosticErrorInformation>();

		public static List<SipebiDiagnosticErrorInformation> DefaultDiagnosticErrorInfos { get; set; } = new List<SipebiDiagnosticErrorInformation> {
			new SipebiDiagnosticErrorInformation {
				ErrorCode = DH.KbbiInformalWordDiagnosticErrorCode,
				Error = "[KBBI] Bentuk Takbaku",
				ErrorExplanation = "[KBBI] Kata yang digunakan merupakan bentuk takbaku",
				AppearOnVersion = "1.0.0.0",
			},
			new SipebiDiagnosticErrorInformation {
				ErrorCode = DH.SipebiInformalWordDiagnosticErrorCode,
				Error = "[SiPEBI] Bentuk Takbaku",
				ErrorExplanation = "[SiPEBI] Kata yang digunakan merupakan bentuk takbaku",
				AppearOnVersion = "1.0.0.0",
			},
			new SipebiDiagnosticErrorInformation {
				ErrorCode = DH.SipebiAmbiguousWordDiagnosticErrorCode,
				Error = "[SiPEBI] Kata Ambigu",
				ErrorExplanation = "[SiPEBI] Kata yang digunakan bersifat ambigu, dapat merupakan bentuk baku atau takbaku tergantung dari makna yang dimaksudkan",
				AppearOnVersion = "1.0.0.0",
			},
		};

		public static void InitCrypt() {
			Cryptography.SetExtension(DH.SipebiE);
			Cryptography.SetPassword(PH.SipebiP);
			Cryptography.SetAesKey(PH.SipebiK);
			Cryptography.SetValidity();
		}

		public static T DeserializeDataOrSettingsFile<T>(string filepath) { 
			byte[] resultBytes = Cryptography.DecryptAndDecompress(File.ReadAllBytes(filepath));
			MemoryStream memoryStream = new MemoryStream(resultBytes);
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			return (T)serializer.Deserialize(memoryStream);
		}
	}
}
