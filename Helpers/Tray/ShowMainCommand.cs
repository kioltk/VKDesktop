using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VKDesktop.Helpers.Tray
{
    public class ShowMainCommand : ICommand
    {
        public void Execute(object parameter)
        {
            Show.MainWindow();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
