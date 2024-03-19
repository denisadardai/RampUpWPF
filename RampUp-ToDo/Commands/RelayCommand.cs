using System.Windows.Input;

namespace RampUp_ToDo.Commands
{
    public class RelayCommand : ICommand
    {
        private EventHandler? _canExecuteChanged;
        private Action<object> _executeAction;
        private Predicate<object> _canExecutePredicate;
        public RelayCommand(Action<object> ExecuteMethod, Predicate<object>? CanExecuteMethod)
        {
            _executeAction = ExecuteMethod;
            _canExecutePredicate = CanExecuteMethod;

        }
        public event EventHandler? CanExecuteChanged
        {
            add
            {
                _canExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                _canExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecutePredicate(parameter);
        }
        public void Execute(object? parameter)
        {
            _executeAction(parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            _canExecuteChanged.Invoke(this,EventArgs.Empty);
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;
        private EventHandler? _canExecuteChanged;

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
            _execute = execute;
        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute ?? (_ => true);
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute((T)parameter);
        }

        public void Execute(object? parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                _canExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                _canExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
