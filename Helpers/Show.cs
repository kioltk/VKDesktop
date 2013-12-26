using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VKDesktop.Core;
using VKDesktop.Core.Messages;
namespace VKDesktop.Helpers
{
    public class Show
    {
        
        
        public static void MainWindow()
        {
            if (!MainWindowFocus())
            {
                MainWindow main = new MainWindow();
                Memory.MainWindow = main;
                main.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
                main.Top = 0;
                main.Left = SystemParameters.PrimaryScreenWidth - main.Width;
                main.Height = SystemParameters.PrimaryScreenHeight;
                main.Show();
                main.Activate();
            }
        }
        public static bool MainWindowFocus()
        {

            if (Memory.IsOpened)
            {
                var window = Memory.MainWindow;
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;
                
                window.Activate();
                return true;
            }
            return false;
        }

        public static void DialogWindow(Dialog dialog)
        {
            if (!DialogWindowFocus(dialog))
            {
                DialogWindow dialogWindow = new DialogWindow(dialog);
                dialogWindow.Show();
                dialogWindow.Activate();
            }
        }

        public static bool DialogWindowFocus(Dialog dialog)
        {
            if (dialog.IsOpened)
            {
                if (dialog.Window.WindowState == WindowState.Minimized)
                    dialog.Window.WindowState = WindowState.Normal;


                dialog.Window.Activate();
                return true;
            }
            return false;
        }
    }
}
