﻿using System;
using System.Diagnostics;

#if WINRT
using Windows.UI.Xaml.Input;
using System.Windows.Input;
using System.ComponentModel;
#else
using System.Windows.Input;
using System.ComponentModel;
#endif

namespace MyToolkit.MVVM
{
#if WINRT
	public class RelayCommand : INotifyPropertyChanged, ICommand
#else
    public class RelayCommand : INotifyPropertyChanged, ICommand
#endif
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute)
            : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute;
        }

        public void Execute(object parameter)
        {
            execute();
        }

        public bool CanExecute
        {
            get { return canExecute == null || canExecute(); }
        }

        public void RaiseCanExecuteChanged()
        {
            RaisePropertyChanged("CanExecute");
            var copy = CanExecuteChanged;
            if (copy != null)
                copy(this, new EventArgs());
        }

        private void RaisePropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public event EventHandler CanExecuteChanged;

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Predicate<T> canExecute;

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            var copy = CanExecuteChanged;
            if (copy != null)
                copy(this, new EventArgs());
        }

        public event EventHandler CanExecuteChanged;
    }
}
