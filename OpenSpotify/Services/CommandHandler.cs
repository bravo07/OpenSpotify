using System;
using System.Windows.Input;

namespace OpenSpotify.Services {

    public class CommandHandler<T> : ICommand {

        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public CommandHandler(Action<T> execute) : this(execute, null) { }

        public CommandHandler(Action<T> execute, Predicate<T> canExecute) {
            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) {
            _execute((T)parameter);
        }
    }
}
