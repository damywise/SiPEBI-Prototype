using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Sipebi.Core;

namespace Sipebi {
	/// <summary>
	/// The base page for all pages to gain base functionalities
	/// </summary>
	public class BasePage : UserControl {
		#region Public Properties
		/// <summary>
		/// The animation to play when the page is first loaded
		/// </summary>
		public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;

		/// <summary>
		/// The animation to play when the page is unloaded
		/// </summary>
		public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToLeft;

		/// <summary>
		/// To time any slide animation takes to complete
		/// Useful for when we are moving the page to another frame
		/// </summary>
		public float SlideSeconds { get; set; } = 0.4f;

		/// <summary>
		/// A flag to indicate if this page should animate out on load
		/// </summary>
		public bool ShouldAnimateOut { get; set; }
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public BasePage() {
			//Don't bother animating in design time
			if (DesignerProperties.GetIsInDesignMode(this))
				return;

			//If we are animating in, hide to begin with
			if (PageLoadAnimation != PageAnimation.None)
				Visibility = Visibility.Collapsed;

			//Listen out for the page loading
			Loaded += BasePage_LoadedAsync;
		}
		#endregion

		#region Animation Load/Unload
		/// <summary>
		/// Once the page is loaded, perform any required animation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		// Note, we don't normally put "async void" because nothing can really await such async (of void)
		// but, there is special effect with "async void" here. Because the await does not actually await anything (void), we don't change the thread (still in UI thread)
		private async void BasePage_LoadedAsync(object sender, System.Windows.RoutedEventArgs e) {
			// If we are setup to animate out on load
			if (ShouldAnimateOut)
				// Animate out of the page
				await AnimateOutAsync();
			else
				//Animate the page in
				//But the method below mess up with the threading, makes it not working
				//Task.Run(async() => await AnimateInAsync()); //this is the trick to run the task asynchronously, independent of the function (be it sync or async)
				await AnimateInAsync();
		}

		/// <summary>
		/// Animates in this page
		/// </summary>
		/// <returns></returns>
		public async Task AnimateInAsync() {
			//Make sure we have something to do
			if (PageLoadAnimation == PageAnimation.None)
				return;

			//Start the animation
			switch (PageLoadAnimation) {
				case PageAnimation.SlideAndFadeInFromRight:
					await this.SlideAndFadeInFromRightAsync(SlideSeconds, width: (int)Application.Current.MainWindow.Width);
					//Storyboard.SetTargetName //we don't need to have target name here
					break;
			}
		}

		/// <summary>
		/// Animates out this page
		/// </summary>
		/// <returns></returns>
		public async Task AnimateOutAsync() {
			//Make sure we have something to do
			if (PageUnloadAnimation == PageAnimation.None)
				return;

			//Start the animation
			switch (PageUnloadAnimation) {
				case PageAnimation.SlideAndFadeOutToLeft:
					await this.SlideAndFadeOutToLeftAsync(SlideSeconds);
					break;
			}
		}
		#endregion
	}

	/// <summary>
	/// A base page for all pages to gain base functionalities with added ViewModel support
	/// </summary>
	public class BasePage<VM> : BasePage where VM : BaseViewModel, new() {
		#region private members
		/// <summary>
		/// The View Model associated with this page
		/// </summary>
		private VM mViewModel;
		#endregion

		#region Public Properties
		/// <summary>
		/// The View Model associated with this page
		/// </summary>
		public VM ViewModel { get => mViewModel; set { if (mViewModel == value) return; mViewModel = value; DataContext = mViewModel; } }

		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public BasePage() : base() {
			//Create a default view model
			ViewModel = new VM();
		}
		#endregion
	}
}
