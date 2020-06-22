using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PropertyChanged;

namespace SipebiPrototype.Core {
  /// <summary>
  /// A base view model that fires Property Changed events as needed
  /// </summary>
  //[ImplementPropertyChanged] //deprecated, changed to AddINotifyPropertyChangedInterface
  [AddINotifyPropertyChangedInterface]
  public class BaseViewModel : INotifyPropertyChanged {
    /// <summary>
    /// The event is fired when any child property changes its value
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

    /// <summary>
    /// Call this to fire a <see cref="PropertyChanged"/> event
    /// </summary>
    /// <param name="name"></param>
    public void OnPropertyChanged(string name) {
      PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

    #region command helpers
    //Expression argument... interesting
    /// <summary>
    /// Runs a command if the updating flag is not set.
    /// If the flag is true (indicating the function is already running) then the action is not run.
    /// If the flag is false (indicating no running cuntion) then the action is run.
    /// Once the action is finished, if it was run, then the flag is reset to false.
    /// </summary>
    /// <param name="updatingFlag">The boolean property flag definiting if the command is already running</param>
    /// <param name="action">The action to run if the command is not already running</param>
    /// <returns></returns>
    protected async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action) {
      //Check if the flag property is true (meaning the function is already running)
      //The compile will compile the expression, and because we invoke it, we like running the function/method
      //But because there is no method, there is only boolean value, thus the boolean value will be returned.
      if (updatingFlag.GetPropertyValue())
        return;

      //Set the property flag to true to indicate we are running
      updatingFlag.SetPropertyValue(true);

      try {
        //Run the passed in action
        await action();
      } finally {
        //Set the property flag back to fase, now it's finished!
        updatingFlag.SetPropertyValue(false);
      }
    }
    #endregion
  }
}
