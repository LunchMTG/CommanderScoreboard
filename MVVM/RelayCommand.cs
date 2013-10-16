using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVM
{
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute) : this((o) => execute()) { }
        public RelayCommand(Action execute, Func<bool> canExecute) : this((o) => execute(), (o) => canExecute()) { }
        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute) : base(execute, canExecute) { }
    }

    public class RelayCommand<T> : ICommand
    {
        #region Fields
        readonly Action<T> _execute;
        readonly Predicate<T> _canExecute;
        #endregion // Fields

        #region Constructors
        public RelayCommand(Action execute) : this((o) => execute()) { }
        public RelayCommand(Action execute, Func<bool> canExecute) : this((o) => execute(), (o) => canExecute()) { }
        public RelayCommand(Action<T> execute) : this(execute, null) { }
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute"); _execute = execute; _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public void Execute(object parameter) 
        { 
            _execute((T)parameter); 
        }
        #endregion // ICommand Members
        
        public event EventHandler CanExecuteChanged;
    }
}
