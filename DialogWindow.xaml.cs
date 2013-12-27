using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        public DialogWindow(Dialog dialog)
        {
            CurrentDialog = dialog;
            dialog.Window = this;
            InitializeComponent();
            Loaded += OnLoaded;

            DataContext = CurrentDialog;

            Closed += DialogClosed;
            Activated += DialogWindow_Activated;
            GotFocus += DialogWindow_GotFocus;
        }

        void DialogWindow_GotFocus(object sender, RoutedEventArgs e)
        {
            MarkAsRead();
        }

        void DialogWindow_Activated(object sender, EventArgs e)
        {
            MarkAsRead();
        }

        public void DialogClosed(object sender, EventArgs e)
        {
            CurrentDialog.Window = null;
        }
        
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {


            

            if (CurrentDialog.Messages.Count < 10)
            {
                await CurrentDialog.LoadMore();
            }

            CurrentDialog.ScrollDown();
            if (CurrentDialog.User.Online)
                UserStatus.Text = CurrentDialog.User.FirstName + " набирает сообщение . . .";
            else
                ShowLastSeen();
        }

        private async void LoadMore(object sender, RoutedEventArgs e)
        {

            await CurrentDialog.LoadMore();
        }
        
        private async void MarkAsRead()
        {
            if (CurrentDialog.HasUnread && !CurrentDialog.IsMarkingAsRead)
            {
               await CurrentDialog.MarkAsRead();
            }
        }

        private void NewMessageKeyUp(object sender, KeyEventArgs e)
        {
            CurrentDialog.SendTypping();
            MarkAsRead();
            var box = (TextBox)sender;
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {

                CurrentDialog.SendMessage(box.Text);
                box.Text = "";

            }
            else
            {
                if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                {
                    //box.Text += "\n";
                }
            }

        }
        
        public void ShowTypping()
        {
            UserStatus.Visibility = System.Windows.Visibility.Visible;
            UserStatus.Text = CurrentDialog.User.FirstName + " набирает сообщение . . .";
            var hidingAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                FillBehavior = FillBehavior.Stop,
                BeginTime = TimeSpan.FromSeconds(4.5),
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };

            var storyboard = new Storyboard();

            storyboard.Children.Add(hidingAnimation);
            Storyboard.SetTarget(hidingAnimation, UserStatus);
            Storyboard.SetTargetProperty(hidingAnimation, new PropertyPath(OpacityProperty));
            storyboard.Completed += delegate { UserStatus.Visibility = System.Windows.Visibility.Hidden; };
            storyboard.Begin();
        }
        public void HideTypping()
        {
            UserStatus.Visibility = System.Windows.Visibility.Collapsed;
        }
        public void ShowLastSeen()
        {
            UserStatus.Visibility = System.Windows.Visibility.Visible;
            UserStatus.Text =  CurrentDialog.User.Seen;
        }
        public void HideLastSeen()
        {

            UserStatus.Visibility = System.Windows.Visibility.Collapsed;
            UserStatus.Text = CurrentDialog.User.FirstName + " набирает сообщение . . .";
        }

        private async void DialogsList_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            ScrollBar sb = e.OriginalSource as ScrollBar;

            

            if (sb.Value == sb.Minimum)
            { 
                if(!CurrentDialog.IsLoading)
                    await CurrentDialog.LoadMore();
            }
        }
        
        
    }
}
