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

    public abstract partial class SimplePopup : UserControl
    {
        private bool isClosing = false;

        Timer closeTimer;

        public abstract string BalloonText { get; }
        public abstract string BalloonTitle { get; }
        public abstract string BalloonImage { get; }
        

        TaskbarIcon taskbarIcon;
        int? closeTiming;
        public SimplePopup(int? closeTiming)
        {
            this.closeTiming = closeTiming;
            if (closeTiming.HasValue)
            {

                closeTimer = new Timer();
                closeTimer.Interval = closeTiming.Value;
                closeTimer.Elapsed += closeTimerElapsed;
            }
        }
        public void Initialize()
        {
            InitializeComponent();
            TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);

            Loaded += FancyBalloon_Loaded;

        }
        private void FancyBalloon_Loaded(object sender, RoutedEventArgs e)
        {
            taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            if (closeTimer != null)
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
            if (closeTimer != null)
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
            if (closeTimer != null)
            {
                closeTimer = new Timer();
                closeTimer.Interval = closeTiming.Value;
                closeTimer.Elapsed += closeTimerElapsed;
                closeTimer.Start();
            }
            else
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

        public abstract void grid_MouseDown(object sender, MouseButtonEventArgs e);
    }

}
