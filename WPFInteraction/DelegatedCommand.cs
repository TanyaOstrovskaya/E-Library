using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFInteraction
{
    public class DelegatedCommand : ICommand
    {
        Action<object> _action;

        bool _canExecuted = true;

        public bool CanExecuted
        {
            get { return _canExecuted; }
            set
            {
                if (_canExecuted != value)
                    this.CanExecuteChanged(this, EventArgs.Empty);

                _canExecuted = value;
            }
        }

        public DelegatedCommand(Action<object> action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged = delegate
        {
        };

        public bool CanExecute(object parameter)
        {
            return _canExecuted;
        }

        public void Execute(object parameter)
        {
            if (_canExecuted)
                _action(parameter);
        }
    }
}
