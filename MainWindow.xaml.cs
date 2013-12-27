using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VKDesktop.Api;
using VKDesktop.Core;
using VKDesktop.Core.Messages;
using VKDesktop.Core.Users;
using VKDesktop.Start;

namespace VKDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User currentUser
        {
            get
            {
                return Account.CurrentUser;
            }
        }

        ObservableCollection<Dialog> dialogs = new ObservableCollection<Dialog>();
        public ObservableCollection<Dialog> getDialogs
        {
            get
            {
                return Memory.dialogs;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += OnLoaded;
        
            Closed += DialogClosed;
            
        }

        public void DialogClosed(object sender, EventArgs e)
        {
            Memory.MainWindow = null;// = false;
        }
        

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            
            DialogsList.MouseDoubleClick += DialogSelected;

        }

        private async void LoadDialogs()
        {

            LoginState.Text = currentUser.Name;
        }
        
        private void DialogSelected(object sender, MouseButtonEventArgs e)
        {
            Dialog dialog = (Dialog) ((ListView)sender).SelectedItem;

            dialog.Open();

        }

        private void OnlineBoxLoaded(object sender, RoutedEventArgs e)
        {

            // ... A List.
            List<string> data = new List<string>();
            data.Add("В сети");
            data.Add("Не в сети");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

            // ... Make the first item selected.
            comboBox.SelectedIndex = 0;

        }
        private void OnlineChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            int selected = comboBox.SelectedIndex;
            if (selected == 0)
            {
                Core.Memory.State = "В сети";
                Core.Account.SetOnline();
            }
            else
            {
                Core.Memory.State = "Не в сети";
                Core.Account.SetOffline();
            }
        }

        private void pageCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void addTextBoxMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
