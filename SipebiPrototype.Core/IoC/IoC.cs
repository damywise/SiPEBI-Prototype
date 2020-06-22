using Ninject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SipebiPrototype.Core {
	/// <summary>
	/// The IoC (Inversion of Control) container for our application
	/// </summary>
	public static class IoC {
		#region public properties
		/// <summary>
		/// The kernel for our IoC container
		/// </summary>
		public static IKernel Kernel { get; private set; } = new StandardKernel();

    /// <summary>
    /// A shortcut to access the <see cref="IUIManager"/>
    /// </summary>
    public static IUIManager UI => Get<IUIManager>();

    /// <summary>
    /// The shortcut to get the Option List view model
    /// </summary>
    public static ApplicationViewModel AppVM { get { return Get<ApplicationViewModel>(); } }

		/// <summary>
		/// The shortcut to get the Option List view model
		/// </summary>
		public static OptionListViewModel OptionListVM { get { return Get<OptionListViewModel>(); } }

		/// <summary>
		/// The shortcut to get the Work view model
		/// </summary>
		public static WorkViewModel WorkVM { get { return Get<WorkViewModel>(); } }

    /// <summary>
    /// The shortcut to get the Sipebi data model
    /// </summary>
    public static ISipebiDataModel SipebiDM { get { return Get<ISipebiDataModel>(); } }

    /// <summary>
    /// The shortcut to get the Formal Word view model
    /// </summary>
    public static FormalWordViewModel FormalWordVM { get { return Get<FormalWordViewModel>(); } }

    /// <summary>
    /// The shortcut to get the Formal Word List view model
    /// </summary>
    public static FormalWordListViewModel FormalWordListVM { get { return Get<FormalWordListViewModel>(); } }

    /// <summary>
    /// The shortcut to get the Formal Word KBBI view model
    /// </summary>
    public static FormalWordKbbiViewModel FormalWordKbbiVM { get { return Get<FormalWordKbbiViewModel>(); } }

    /// <summary>
    /// List of all informal word strings in KBBI
    /// NOTE: To be set externally for optimization
    /// </summary>
    public static List<string> InformalWordStringsKbbi { get; set; }

    /// <summary>
    /// List of all user-defined informal word strings not in KBBI
    /// NOTE: To be set externally for optimization
    /// </summary>
    public static List<string> CustomInformalWordStrings { get; set; }

    /// <summary>
    /// List of all informal words (<see cref="FormalWordItem"/>) in KBBI
    /// NOTE: To be set externally for optimization
    /// </summary>
    public static List<FormalWordItem> InformalWordsKbbi { get; set; }

    /// <summary>
    /// List of all user-defined informal words (<see cref="FormalWordItem"/>) not in KBBI
    /// NOTE: To be set externally for optimization
    /// </summary>
    public static List<FormalWordItem> CustomInformalWords { get; set; }

    /// <summary>
    /// List of SiPEBI diagnostic error information
    /// </summary>
    public static List<SipebiDiagnosticErrorInformation> DiagnosticErrorInformationList { get; set; }

    /// <summary>
    /// The shortcut to get the <see cref="DiagnosticListViewModel"/>
    /// </summary>
    public static DiagnosticListViewModel DiagnosticListVM { get { return Get<DiagnosticListViewModel>(); } }
    #endregion

    #region Construction
    /// <summary>
    /// Sets up the IoC container, binds all information required is ready for use
    /// NOTE: Must be called as soon as your application starts up to ensure all services can be found
    /// </summary>
    public static void Setup() {
			// Bind all required view models
			BindViewModels();
		}

    /// <summary>
    /// To prepare all the data needed for further SiPEBI operation
    /// NOTE: to be called on Login and cleared on Logout
    /// </summary>
    public static async Task PrepareData() {
      //Completed all the data preparation
      await SipebiDM.InitialPreparation();
      //await SipebiDM.PrepareEntriSipebis(); //do not do this anymore
      await SipebiDM.PrepareFormalWords();
      await SipebiDM.PrepareLinkedWords();
      await SipebiDM.PreparePlaceWords();
      await SipebiDM.PrepareConjunctionSubordinativeWords();
      await SipebiDM.PrepareDefiniteCapitalWords();
      await SipebiDM.PrepareAmbiguousWords();
    }

    /// <summary>
    /// Binds all singleton view models
    /// </summary>
    private static void BindViewModels() {
			//Kernel.Bind<ApplicationViewModel>().ToSelf().InSingletonScope();
			//Binds to a single instance of application view model
			Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
			//By doing this binding, everywhere else in the application, can do Kernel.Get and gets whatever is bound back

			//Binds to the single instance of the option list view model
			Kernel.Bind<OptionListViewModel>().ToConstant(OptionListViewModel.ViewInstance);

			//Binds to the single instance of the work view model
			Kernel.Bind<WorkViewModel>().ToConstant(WorkViewModel.Instance);

      //Binds to the single instance of the formal word list view model
      Kernel.Bind<FormalWordViewModel>().ToConstant(FormalWordViewModel.Instance);

      //Binds to the single instance of the formal word list view model
      Kernel.Bind<FormalWordListViewModel>().ToConstant(FormalWordListViewModel.Instance);

      //Binds to the single instance of the formal word KBBI view model
      Kernel.Bind<FormalWordKbbiViewModel>().ToConstant(FormalWordKbbiViewModel.Instance);

      //Binds to the single instance of the diagnostic list view model
      Kernel.Bind<DiagnosticListViewModel>().ToConstant(DiagnosticListViewModel.Instance);
    }
    #endregion

    /// <summary>
    /// Gets a service from the IoC, of the specified type
    /// </summary>
    /// <typeparam name="T">The type to get</typeparam>
    /// <returns></returns>
    public static T Get<T>() {
			return Kernel.Get<T>();
		}
	}
}
