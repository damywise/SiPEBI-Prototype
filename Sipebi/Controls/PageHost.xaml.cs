using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sipebi.Core;

namespace Sipebi {
	/// <summary>
	/// Interaction logic for PageHost.xaml
	/// </summary>
	public partial class PageHost : UserControl {
		#region Dependency Properties
		/// <summary>
		/// The current page to show in the page host
		/// </summary>
		//This is a dependency property (like AttachedProperty, they are both messy), we are going to create one like that
		//DependencyProperty is just about getting a public property to be settable inside XAML
		public BasePage CurrentPage {
			get => (BasePage)GetValue(CurrentPageProperty);
			set => SetValue(CurrentPageProperty, value);
		}

		/// <summary>
		/// Registers <see cref="CurrentPage"/> as a dependency property
		/// </summary>
		// Using a DependencyProperty as the backing store for CurrentPage. This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CurrentPageProperty =
			DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(PageHost),
				new UIPropertyMetadata(CurrentPagePropertyChanged));

		#endregion
		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public PageHost() {
			InitializeComponent();

			//If we are in design mode, show the current page
			//as the dependency property does not fire
			if (DesignerProperties.GetIsInDesignMode(this)) {
				NewPage.Content = (BasePage)new ApplicationPageValueConverter().Convert(IoC.AppVM.CurrentPage);
			}
		}
		#endregion

		#region Property Changed Events
		/// <summary>
		/// Called when the <see cref="CurrenPageProperty"/> value has changed
		/// </summary>
		/// <param name="d">The sender, this object class' instance (PageHost)</param>
		/// <param name="e"></param>
		private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			// Get the frames
			var oldPageFrame = (d as PageHost).OldPage;
			var newPageFrame = (d as PageHost).NewPage;

			// Store the current page content as the old page
			var oldPageContent = newPageFrame.Content;

			// Remove current page from new page frame
			newPageFrame.Content = null;

			// Move the previous page into the old page frame
			oldPageFrame.Content = oldPageContent;

			// Animates the old page (previous page) out when the Loaded event fires
			// right after this call due to moving frames
			if (oldPageContent is BasePage oldPage) {
				//Tell old page to animate out
				oldPage.ShouldAnimateOut = true;
				//Task.Run(oldPage.AnimateOutAsync); //oldPage.AnimateOutAsync() will cause VS to complain (cause there isn't await) though it means the same

				//Once it is done, remove it
				Task.Delay((int)(oldPage.SlideSeconds * 1000)).ContinueWith((t) => {
					//Remove the old page
					Application.Current.Dispatcher.Invoke(() => oldPageFrame.Content = null);
				});
			}

			// Set the new page content
			newPageFrame.Content = e.NewValue;
		}
		#endregion
	}
}


