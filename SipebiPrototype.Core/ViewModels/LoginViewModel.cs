using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SipebiPrototype.Core {
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

		///// <summary>
		///// The users password, use SecureString so that: 
		///// (1) It will be encrypted during process and 
		///// (2) It will be wiped out from memory as soon as it is not needed
		///// </summary>
		//public SecureString Password { get; set; }
		#endregion

		#region Commands

		/// <summary>
		/// The command to login
		/// </summary>
		public ICommand LoginCommand { get; set; }

		/// <summary>
		/// The command to register for a new account
		/// </summary>
		public ICommand RegisterCommand { get; set; }
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public LoginViewModel() {
			//Normally we don't pass parameter, but this password is exception
			LoginCommand = new RelayParameterizedCommand(async (parameter) => await LoginAsync(parameter));
			RegisterCommand = new RelayCommand(async () => await RegisterAsync());
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
