using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sipebi.Core {
	public interface ISipebiDataModel {
    /// <summary>
    /// The first function to be called prepare all necessary items for the subsequent preparations (such as the default diagnostic errors)
    /// </summary>
    Task InitialPreparation();

    /// <summary>
    /// The function to get the list of entri sipebi data loaded/prepared
    /// </summary>
    List<EntriSipebi> GetEntriSipebis();

    /// <summary>
    /// The function to prepare all the entri sipebi data
    /// </summary>
    Task PrepareEntriSipebis();

    /// <summary>
    /// The function to clear all the entri sipebi data
    /// </summary>
    void ClearEntriSipebis();

    /// <summary>
    /// The function to get the list of formal word items
    /// </summary>
    List<FormalWordItem> GetFormalWords();

    /// <summary>
    /// The function to get the list of word items
    /// </summary>
    List<WordItem> GetWords();

    /// <summary>
    /// The function to prepare all the word items
    /// </summary>
    Task PrepareWords();

    /// <summary>
    /// The function to get the User-Mode SiPEBI settings
    /// </summary>
    SipebiSettings GetSettings();

    /// <summary>
    /// The function to prepare all the formal word items
    /// </summary>
    Task PrepareFormalWords();

    /// <summary>
    /// The function to clear all the formal word items
    /// </summary>
    void ClearFormalWords();

    /// <summary>
    /// The function to add a formal word item
    /// <param name="item">The new formal word item to be added</paramref>
    /// </summary>
    bool AddFormalWord(FormalWordItem item);

    /// <summary>
    /// The function to edit a formal word item
    /// <param name="item">The new formal word item to edit the old one</paramref>
    /// <param name="index">The index of the the old item to be edited</paramref>
    /// </summary>
    bool EditFormalWord(FormalWordItem item, int index);

    /// <summary>
    /// The function to delete a formal word item
    /// <param name="index">The index of the the item to be deleted</paramref>
    /// </summary>
    bool DeleteFormalWord(int index);

    /// <summary>
    /// The function to get details of a formal word item
    /// <param name="index">The index of the the item to get the details from</paramref>
    /// <param name="doNotGetWhenNotEditable">The flag to indicate if the item is to be obtained if it is not editable</paramref>
    /// </summary>
    FormalWordItem DetailsFormalWord(int index, bool doNotGetWhenNotEditable);

    /// <summary>
    /// The function to prepare all the linked word items
    /// </summary>
    Task PrepareLinkedWords();

    /// <summary>
    /// The function to clear all the linked word items
    /// </summary>
    void ClearLinkedWords();

    /// <summary>
    /// The function to get the list of linked word items
    /// </summary>
    List<SipebiLinkedWord> GetLinkedWords();

    /// <summary>
    /// The function to prepare all the place word items
    /// </summary>
    Task PreparePlaceWords();

    /// <summary>
    /// The function to clear all the place word items
    /// </summary>
    void ClearPlaceWords();

    /// <summary>
    /// The function to get the list of place word items
    /// </summary>
    List<SipebiPlaceWord> GetPlaceWords();

    /// <summary>
    /// The function to prepare all the conjunction subordinative word items
    /// </summary>
    Task PrepareConjunctionSubordinativeWords();

    /// <summary>
    /// The function to clear all the conjunction subordinative word items
    /// </summary>
    void ClearConjunctionSubordinativeWords();

    /// <summary>
    /// The function to get the list of conjunction subordinative word items
    /// </summary>
    List<SipebiConjunctionSubordinativeWord> GetConjunctionSubordinativeWords();

    /// <summary>
    /// The function to prepare all the definite capital word items
    /// </summary>
    Task PrepareDefiniteCapitalWords();

    /// <summary>
    /// The function to clear all the definite capital word items
    /// </summary>
    void ClearDefiniteCapitalWords();

    /// <summary>
    /// The function to get the list of definite capital word items
    /// </summary>
    List<SipebiDefiniteCapitalWord> GetDefiniteCapitalWords();

    /// <summary>
    /// The function to prepare all the ambiguous word items
    /// </summary>
    Task PrepareAmbiguousWords();

    /// <summary>
    /// The function to clear all the ambiguous word items
    /// </summary>
    void ClearAmbiguousWords();

    /// <summary>
    /// The function to get the list of ambiguous word items
    /// </summary>
    List<SipebiAmbiguousWord> GetAmbiguousWords();

    /// <summary>
    /// The most important function of the <see cref="ISipebiDataModel"/> is this function, 
    /// used to correct a text from an original taxt containing mistakes.
    /// In contrast to <see cref="CorrectText(string)"/> which is run when SiPEBI is run in editor mode, 
    /// this method is used when SiPEBI is run in user mode
    /// </summary>
    /// <returns>The diagnostic report (correction) result</returns>
    SipebiDiagnosticReport CorrectTextUser(string originalText);

    /// <summary>
    /// The most important function of the <see cref="ISipebiDataModel"/> is this function, used to correct a text from an original taxt containing mistakes
    /// </summary>
    /// <returns>The diagnostic report (correction) result</returns>
    SipebiDiagnosticReport CorrectText(string originalText);

    ///// <summary>
    ///// The function to get the last analysis result
    ///// </summary>
    ///// <returns></returns>
    //List<SipebiDiagnosticError> GetLastAnalysisResult();
  }
}
