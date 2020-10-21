using Extension.Versioning;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sipebi.Core {
	/// <summary>
	/// The View Model for the login screen
	/// </summary>
	public class LoginViewModel : BaseViewModel {
		//#region private member
		//#endregion

		#region public properties
		/// <summary>
		/// Email of the user
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// A flag indicating if the login command is running
		/// </summary>
		public bool LoginIsRunning { get; set; }

		public bool UserVersion { get; set; }

		public string CurrentVersion { get; set; }

		public string LatestVersion { get; set; }

		public bool NewVersionIsAvailable { get; set; }

		public string UpdateCheckText { get; set; }

		public string VersionText { get; set; }
		public bool GetUpdateIsRunning { get; set; }


		///// <summary>
		///// The users password, use SecureString so that: 
		///// (1) It will be encrypted during process and 
		///// (2) It will be wiped out from memory as soon as it is not needed
		///// </summary>
		//public SecureString Password { get; set; }
		#endregion

		public static LoginViewModel DesignInstance = new LoginViewModel() { NewVersionIsAvailable = true, VersionText = "Versi: 1.0.0.0 Pengguna" };

		#region Commands

		/// <summary>
		/// The command to login
		/// </summary>
		public ICommand LoginCommand { get; set; }

		/// <summary>
		/// The command to register for a new account
		/// </summary>
		public ICommand RegisterCommand { get; set; }

		/// <summary>
		/// The command to go to the webpage containing the latest application version
		/// </summary>
		public ICommand GetUpdateCommand { get; set; }
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public LoginViewModel() {
			//Normally we don't pass parameter, but this password is exception
			LoginCommand = new RelayParameterizedCommand(async (parameter) => await LoginAsync(parameter));
			RegisterCommand = new RelayCommand(async () => await RegisterAsync());
			GetUpdateCommand = new RelayCommand(async () => await GetUpdateAsync());

			//Should now check from the internet if we may get any latest SiPEBI version
			checkUpdate();
		}

		private async Task GetUpdateAsync() {
			await RunCommandAsync(() => GetUpdateIsRunning, async () => {
				await Task.Run(() => {
					System.Diagnostics.Process.Start(DH.GetUpdateUrl);
				});
			});
		}


		private async void checkUpdate() {
			UserVersion = PH.SipebiUserMode;
			VersionText = "Versi: " + PH.SoftwareVersion;
			string version = Info.GetFileVersionFor(Assembly.GetExecutingAssembly());
			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DH.UpdateCheckUrl);
				request.Method = "GET";
				request.Timeout = 10000;
				WebResponse response = await request.GetResponseAsync(); //This async seems to work really well
				StreamReader loResponseStream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252));
				string responseText = loResponseStream.ReadToEnd();
				response.Close();
				loResponseStream.Close();
				LatestVersion = responseText;
				NewVersionIsAvailable = !string.IsNullOrWhiteSpace(LatestVersion) && LatestVersion != version;
				UpdateCheckText = "Dapatkan Versi Terbaru: " + LatestVersion;
			} catch { //do nothing for now if the update fails
			}
		}

		/// <summary>
		/// Attempts to log the user in
		/// </summary>
		/// <param name="parameter">The <see cref="SecureString"/> passed in from the View for the user password</param>
		/// <returns></returns>
		private async Task LoginAsync(object parameter) {
			//this is passing the lamdba expression.. with a property which can be modified internally! This is new!
			await RunCommandAsync(() => LoginIsRunning, async () => {
        //TODO should be changed with the real authentication procedure

        //To prepare all necessary data for Sipebi to start working
        await IoC.PrepareData();

				//Go to work page
				IoC.AppVM.GoToPage(ApplicationPage.Work);
				////IMPORTANT: Never store unsecure password in variable like this
				//var password = (parameter as IHavePassword).SecurePassword.Unsecure();
			});
		}

		/// <summary>
		/// Takes the user to the register page
		/// </summary>
		/// <returns></returns>
		private async Task RegisterAsync() {
			//Go to register page
			//Application.Current.MainWindow always return MainWindow of the application (regardless whether it is named MainWindow.xaml or not)
			//((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Register;
			IoC.AppVM.GoToPage(ApplicationPage.Register);

			await Task.Delay(1);
		}
		#endregion

		#region private helpers
		#endregion
	}
}
