using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.IO;
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
using VKDesktop.Start;
using VKDesktop.Helpers;
using System.Windows.Controls.Primitives;
using VKDesktop.Models;
namespace VKDesktop
{
    /// <summary>
    /// Interaction logic for BackgroundActivity.xaml
    /// </summary>
    public partial class BackgroundActivity : Window
    {
        public BackgroundActivity()
        {
            InitializeComponent();


            Loaded += BackgroundActivity_Loaded;
            Closed += BackgroundActivity_Closed;
        }

        public void BackgroundActivity_Closed(object sender, EventArgs e)
        {
            TrayIcon.Visibility = Visibility.Collapsed;
        }

        public void BackgroundActivity_Loaded(object sender, RoutedEventArgs e)
        {
            Core.Memory.State = "Старт..";
            WindowState = System.Windows.WindowState.Minimized;
            Visibility = System.Windows.Visibility.Hidden;
            StartWindow start = new StartWindow();
            start.Owner = this;
            bool? isLogined = start.ShowDialog();

            if (isLogined.HasValue)
                if (isLogined.Value)
                {
                    Core.Memory.State = "В сети";
                    VKDesktop.Helpers.Show.MainWindow();
                }
                else
                {
                    Close();
                }

            LoadNotification();
        }

        public void LoadNotification()
        {
            /*
                var uri = new Uri("/resources/icons/favicon.ico", UriKind.Relative);
                Stream iconStream = Application.GetResourceStream(uri).Stream;
                var ic = new System.Drawing.Icon(iconStream);
                TrayIcon.Icon = ic;
            */
            Notificator.Tray = TrayIcon;
            TrayIcon.Visibility = System.Windows.Visibility.Visible;
            TrayTooltip.DataContext = Models.TrayTooltip.Get;
            ContextMenu trayMenu = TrayMenu;
            
            MenuItem item1 = new MenuItem();
            MenuItem item2 = new MenuItem();

            item1.Header = "Шарик?";
            item1.Click += ShowBalloon;

            item2.Header = "Выход";
            item2.Click += ExitClicked;

            trayMenu.Items.Add(item1);
            trayMenu.Items.Add(item2);

          
        }

        public void ShowBalloon(object sender, RoutedEventArgs e)
        {
            //ShowCustomBalloon();
        }

        
        public void ExitClicked(object sender, RoutedEventArgs e)
        {
            Core.Account.SetOffline();
            Close();
        }
        

        
        
    }
}
