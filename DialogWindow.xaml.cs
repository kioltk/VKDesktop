using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VKDesktop.Core.Messages;

namespace VKDesktop
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public Dialog CurrentDialog
        {
            get;
            set;
        }
        public DialogWindow()
        {

            InitializeComponent();
            Loaded += OnLoaded;
            GotFocus += DialogGotFocus;
            LostFocus += DialogLostFocus;
        }

        private void DialogLostFocus(object sender, RoutedEventArgs e)
        {
            CurrentDialog.IsFocused = false;
        }

        private void DialogGotFocus(object sender, RoutedEventArgs e)
        {
            CurrentDialog.IsFocused = true;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            DataContext = CurrentDialog;
            
        }

        private void LoadMore(object sender, RoutedEventArgs e)
        {

            CurrentDialog.LoadMore();

        }

        private void NewMessageKeyUp(object sender, KeyEventArgs e)
        {

            var box = (TextBox)sender;
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {

                CurrentDialog.SendMessage(box.Text);
                box.Text = "";
                
            }

        }

        
        
        
    }
}
