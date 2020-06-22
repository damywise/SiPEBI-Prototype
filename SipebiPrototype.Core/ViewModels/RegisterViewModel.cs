using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SipebiPrototype.Core {
	/// <summary>
	/// The View Model for the register screen
	/// </summary>
	public class RegisterViewModel : BaseViewModel {
		#region public properties
		/// <summary>
		/// Email of the user
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// A flag indicating if the register command is running
		/// </summary>
		public bool RegisterIsRunning { get; set; }

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
		public RegisterViewModel() {
			//Normally we don't pass parameter, but this password is exception
			RegisterCommand = new RelayParameterizedCommand(async (parameter) => await RegisterAsync(parameter));
			LoginCommand = new RelayCommand(async () => await LoginAsync());
		}

		/// <summary>
		/// Attempts to register a new user
		/// </summary>
		/// <param name="parameter">The <see cref="SecureString"/> passed in from the View for the user password</param>
		/// <returns></returns>
		private async Task RegisterAsync(object parameter) {
			//this is passing the lamdba expression.. with a property which can be modified internally! This is new!
			await RunCommandAsync(() => RegisterIsRunning, async () => {
				await Task.Delay(5000);
			});
		}

		/// <summary>
		/// Takes the user to the login page
		/// </summary>
		/// <returns></returns>
		private async Task LoginAsync() {
			//Go to login page
			IoC.AppVM.GoToPage (ApplicationPage.Login);

			await Task.Delay(1);
		}
		#endregion

		#region private helpers
		#endregion
	}
}
