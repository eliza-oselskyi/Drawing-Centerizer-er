using System;
using System.Windows.Input;

namespace Drawing.CenterViewWPF.Core.Commands;

/// Encapsulates a command that invokes an action and determines its executable state.
/// Implements the ICommand interface to support data binding in a UI framework.
public class RelayCommand : ICommand
{
    /// Represents a command that can be bound to user interface elements
    /// to execute an action and determine whether it can be executed.
    /// The class allows encapsulation of an action and provides support
    /// for conditional execution logic.
    /// Implements the System.Windows.Input.ICommand interface.
    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
        _Execute = execute;
        _CanExecute = canExecute;
    }

    private Action<object> _Execute { get; }
    private Predicate<object> _CanExecute { get; }

    /// Determines whether the command can execute in its current state.
    /// This method evaluates a predicate function provided during the initialization
    /// of the command and returns its result.
    /// <param name="parameter">
    ///     An optional parameter used by the predicate function to evaluate whether the command can
    ///     execute.
    /// </param>
    /// <return>Returns true if the command can execute; otherwise, returns false.</return>
    public bool CanExecute(object parameter)
    {
        return _CanExecute(parameter);
    }

    /// Executes the command logic associated with this instance.
    /// Invokes the action provided during the initialization of the command.
    /// <param name="parameter">
    ///     An optional parameter passed to the action method that defines the execution logic of the
    ///     command.
    /// </param>
    public void Execute(object parameter)
    {
        _Execute(parameter);
    }

    public event EventHandler? CanExecuteChanged;
}