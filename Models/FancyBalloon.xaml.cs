using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;
using System.Timers;
using VKDesktop.Core.Messages;
using VKDesktop.Core;
namespace VKDesktop.Models
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>

    public partial class MessagePopup : UserControl
    {/*
        private bool isClosing = false;

        #region BalloonText dependency property

        public string BalloonTitle
        {
            get;
            set;
        }
        public string BalloonText
        {
            get;
            set;
        }
        public string BalloonImage
        {
            get;
            set;
        }

        #endregion

        Timer closeTimer;
        
        public FancyBalloon(string image,string title, string text)
        {
            this.BalloonImage = image;
            this.BalloonText = text;
            this.BalloonTitle = title;
            InitializeComponent();
            TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);

            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            //taskbarIcon.ResetBalloonCloseTimer();
            closeTimer = new Timer();
            closeTimer.Interval = 2000;
            closeTimer.Elapsed += closeTimerElapsed;
            closeTimer.Start();
            
        }

        public void closeTimerElapsed(object sender, ElapsedEventArgs e)
        {
            closeTimer.Stop();
                Close();
        }


        /// <summary>
        /// By subscribing to the <see cref="TaskbarIcon.BalloonClosingEvent"/>
        /// and setting the "Handled" property to true, we suppress the popup
        /// from being closed in order to display the fade-out animation.
        /// </summary>
        private void OnBalloonClosing(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            isClosing = true;
        }


        /// <summary>
        /// Resolves the <see cref="TaskbarIcon"/> that displayed
        /// the balloon and requests a close action.
        /// </summary>
        private void imgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //the tray icon assigned this attached property to simplify access

            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();
        }

        /// <summary>
        /// If the users hovers over the balloon, we don't close it.
        /// </summary>
        private void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //if we're already running the fade-out animation, do not interrupt anymore
            //(makes things too complicated for the sample)
            if (isClosing) return;
            closeTimer.Dispose();
            //the tray icon assigned this attached property to simplify access

        }
        private void grid_MouseLeave(object sender, MouseEventArgs e)
        {
            //if we're already running the fade-out animation, do not interrupt anymore
            //(makes things too complicated for the sample)
            if (isClosing) return;
            closeTimer = new Timer();
            closeTimer.Interval = 2000;
            closeTimer.Elapsed += closeTimerElapsed;
            closeTimer.BeginInit();
            //the tray icon assigned this attached property to simplify access

        }


        /// <summary>
        /// Closes the popup once the fade-out animation completed.
        /// The animation was triggered in XAML through the attached
        /// BalloonClosing event.
        /// </summary>
        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            Popup pp = (Popup)Parent;
            pp.IsOpen = false;
           
        }
        public void Close()
        {
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();
        }*/
        private bool isClosing = false;

        Timer closeTimer;
        #region BalloonText dependency property

        /// <summary>
        /// Description
        /// </summary>
       /* public static readonly DependencyProperty BalloonTextProperty =
            DependencyProperty.Register("BalloonText",
                                        typeof(string),
                                        typeof(MessagePopup),
                                        new FrameworkPropertyMetadata(""));
        */
        /// <summary>
        /// A property wrapper for the <see cref="BalloonTextProperty"/>
        /// dependency property:<br/>
        /// Description
        /// </summary>
        public Message message;
        public string BalloonText
        {
            get { return message.Body; }
        }
        public string BalloonTitle
        {
            get
            {
                return message.User.Name;
            }
        }
        public string BalloonImage
        {
            get
            {
                return message.User.photo_50;
            }
        }
        #endregion

        TaskbarIcon taskbarIcon;
        private MessagePopup()
        {
            InitializeComponent();
            TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);
        }
        public MessagePopup(Message message)
        {
            this.message = message;
            InitializeComponent();
            TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);

            closeTimer = new Timer();
            closeTimer.Interval = 4000;
            closeTimer.Elapsed += closeTimerElapsed;
            Loaded += FancyBalloon_Loaded;
        }

        private void FancyBalloon_Loaded(object sender, RoutedEventArgs e)
        {
            taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            closeTimer.Start();
        }

        private void closeTimerElapsed(object sender, ElapsedEventArgs e)
        {
            closeTimer.Stop();
            Close();
        }

        /// <summary>
        /// By subscribing to the <see cref="TaskbarIcon.BalloonClosingEvent"/>
        /// and setting the "Handled" property to true, we suppress the popup
        /// from being closed in order to display the fade-out animation.
        /// </summary>
        private void OnBalloonClosing(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            isClosing = true;
        }


        /// <summary>
        /// Resolves the <see cref="TaskbarIcon"/> that displayed
        /// the balloon and requests a close action.
        /// </summary>
        private void imgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //the tray icon assigned this attached property to simplify access
            Close();
        }

        public void Close()
        {
            
            taskbarIcon.CloseBalloon();
        }

        /// <summary>
        /// If the users hovers over the balloon, we don't close it.
        /// </summary>
        private void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //if we're already running the fade-out animation, do not interrupt anymore
            //(makes things too complicated for the sample)
            if (isClosing) return;

            //the tray icon assigned this attached property to simplify access
            closeTimer.Dispose();
        }
        private void grid_MouseLeave(object sender, MouseEventArgs e)
        {
            /*
            //if we're already running the fade-out animation, do not interrupt anymore
            //(makes things too complicated for the sample)
            if (isClosing) return;
            closeTimer = new Timer();
            closeTimer.Interval = 2000;
            closeTimer.Elapsed += closeTimerElapsed;
            closeTimer.BeginInit();
            //the tray icon assigned this attached property to simplify access
            */
            closeTimer = new Timer();
            closeTimer.Interval = 2000;
            closeTimer.Elapsed += closeTimerElapsed;
            closeTimer.Start();
            Close();
        }

        /// <summary>
        /// Closes the popup once the fade-out animation completed.
        /// The animation was triggered in XAML through the attached
        /// BalloonClosing event.
        /// </summary>
        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            Popup pp = (Popup)Parent;
            pp.IsOpen = false;
        }

        private void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            message.User.Dialog.Open();
            Close();
        }
    }

}
