using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using SipebiPrototype.Core;

namespace SipebiPrototype {
	//public class MyPasswordBox { //cannot inherit from PasswordBox because PasswordBox is a sealed class

	//}

	/// <summary>
	/// The View Model for the custom flat window
	/// </summary>
	public class WindowViewModel : BaseViewModel {
		#region private member
		/// <summary>
		/// The window this view model controls
		/// </summary>
		private Window mWindow;

		/// <summary>
		/// The margin around the window to allow for a drop shadow
		/// </summary>
		private int mOuterMarginSize = 10;

		/// <summary>
		/// The radius of the edges of the window
		/// </summary>
		private int mWindowRadius = 10; //curve edge in the corner of the windows

		/// <summary>
		/// The last known dock position
		/// </summary>
		private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;
		#endregion

		#region public properties
		/// <summary>
		/// The padding of the inner content of the main window
		/// </summary>
		public Thickness InnerContentPadding { get { return new Thickness(0); } }

		/// <summary>
		/// The smallest width the window can go to
		/// </summary>
		public double WindowMinimumWidth { get; set; } = 1080;

		/// <summary>
		/// The smallest height the window can go to
		/// </summary>
		public double WindowMinimumHeight { get; set; } = 640;

    /// <summary>
    /// True if the window should be borderless because it it docked or maximized
    /// </summary>
    public bool Borderless => mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked;

		//Each of the public properties below needs its event handler!
		/// <summary>
		/// The size of the resize border around the window
		/// </summary>
		public int ResizeBorder => Borderless ? 0 : 6;

		/// <summary>
		/// The size of the resize border around the window, taking into account the outer margin
		/// </summary>
		public Thickness ResizeBorderThickness => new Thickness(ResizeBorder + OuterMarginSize); //needs to include the OuterMarginSize here because we already take trouble to create an auto margin

    /// <summary>
    /// The maximum height the content window can go to
    /// </summary>
    public double MaxContentHeight { get; set; } = 450;

    /// <summary>
    /// The margin around the window to allow for a drop shadow
    /// </summary>
    //so actually the outer margin is zeroed when the window is maximized. The window is larger than what it actually looks like
    public int OuterMarginSize {
			get => Borderless ? 0 : mOuterMarginSize;
			set => mOuterMarginSize = value;
		}

		/// <summary>
		/// The thickness of the outer margin (should be 1-2, I think?)
		/// </summary>
		public Thickness OuterMarginSizeThickness => new Thickness(OuterMarginSize);

		/// <summary>
		/// The radius of the edges of the window
		/// </summary>
		public int WindowRadius {
			get => Borderless ? 0 : mWindowRadius;
			set => mWindowRadius = value;
		}

		/// <summary>
		/// The radius of the edges of the window
		/// </summary>
		public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

		/// <summary>
		/// The height of the title bar / caption of the window
		/// </summary>
		public int TitleHeight { get; set; } = 49;

		/// <summary>
		/// The grid length of the title height, includes ResizeBorder
		/// </summary>
		public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);

    /// <summary>
    /// True if we should have a dimmed overlay on the window
    /// such as when a popup is visible or the window is not focused
    /// </summary>
    public bool DimmableOverlayVisible { get; set; }
    #endregion

    #region Commands
    /// <summary>
    /// The command to minimize the window
    /// </summary>
    public ICommand MinimizeCommand { get; set; }

		/// <summary>
		/// The command to maximize the window
		/// </summary>
		public ICommand MaximizeCommand { get; set; }

		/// <summary>
		/// The command to close the window
		/// </summary>
		public ICommand CloseCommand { get; set; }

		/// <summary>
		/// The command to show the system menu of the window
		/// </summary>
		public ICommand MenuCommand { get; set; }
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public WindowViewModel(Window window) {
			mWindow = window;

			//Listen out for the window resizing
			//An event, not in our view model... is in the window
			mWindow.StateChanged += (sender, e) => {
				//Fire off events for all properties which are affected by resize
				OnPropertyChanged(nameof(ResizeBorderThickness));
				OnPropertyChanged(nameof(OuterMarginSize));
				OnPropertyChanged(nameof(OuterMarginSizeThickness));
				OnPropertyChanged(nameof(WindowRadius));
				OnPropertyChanged(nameof(WindowCornerRadius));
        //MaxContentHeight = mWindow.ActualHeight - 190;
        //IoC.FormalWordVM.MaxContentHeight = MaxContentHeight;
        //IoC.FormalWordVM.OnPropertyChanged(nameof(MaxContentHeight));
			};

			//Create commands
			MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized); 
			MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized); //To toggle from normal to maximize. If maximize, the normalize and vice versa
			CloseCommand = new RelayCommand(() => mWindow.Close());
			MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

			//Fixe WPF window resize inherent bug issue
			var resizer = new WindowResizer(mWindow);
		}
		#endregion

		#region private helpers
		////Taken from https://stackoverflow.com/questions/4226740/how-do-i-get-the-current-mouse-screen-coordinates-in-wpf
		//[DllImport("user32.dll")]
		//[return: MarshalAs(UnmanagedType.Bool)]
		//internal static extern bool GetCursorPos(ref Win32Point pt);

		//[StructLayout(LayoutKind.Sequential)]
		//internal struct Win32Point {
		//  public Int32 X;
		//  public Int32 Y;
		//};

		/// <summary>
		/// Gets the current mouse position on the screen
		/// </summary>
		/// <returns></returns>
		private Point GetMousePosition() { //the video makes this private from public
			//Win32Point w32Mouse = new Win32Point();
			//GetCursorPos(ref w32Mouse);

			var position = Mouse.GetPosition(mWindow);
			//Add the window position
			return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);
		}
		#endregion
	}
}
