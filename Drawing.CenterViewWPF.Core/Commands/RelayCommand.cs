using System;
using System.Windows.Input;

namespace Drawing.CenterViewWPF.Core.Commands;

public class RelayCommand : ICommand
{
    
    private Action<object> _Execute { get; set; }
    private Predicate<object> _CanExecute { get; set; }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
        _Execute = execute;
        _CanExecute = canExecute;
    }
    
    public bool CanExecute(object parameter)
    {
        return _CanExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _Execute(parameter);
    }

    public event EventHandler? CanExecuteChanged;
}