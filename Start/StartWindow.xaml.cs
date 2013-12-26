using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
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
using VKDesktop.Api;
using VKDesktop.Core;
using VKDesktop.Core.Messages;
using VKDesktop.Core.Users;

namespace VKDesktop.Start
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public Models.Start Start;
        public StartWindow()
        {
            InitializeComponent();

            Start = new Models.Start();
            DataContext = Start;
            Loaded += OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Activate();
            AuthorisationDialog auth = new AuthorisationDialog();
            auth.Owner = this;
            do
            {
                Start.State = "Проходит авторизация...";
            }
            while (auth.ShowDialog().Value);
            if (string.IsNullOrEmpty(Access.access_token))
            {
                Start.State = "Авторизация неудачна(";

                //System.Threading.Thread.Sleep(3000);
                Start.State = "Выход..";
                //System.Threading.Thread.Sleep(3000);
                this.DialogResult = false;
                this.Close();
            }
            else
            {
                LoadDialogs();
            }
            


        }

        private async void LoadDialogs()
        {

            Start.State = "Подключение...";

            User user = await Api.Request.GetUser();
            Account.CurrentUser = user;

            HelloUser.Text = "Привет, " + Account.CurrentUser.Name + "!";
            //System.Threading.Thread.Sleep(3000);
            Start.State = "Загрузка...";

            List<Message> messagesList = await Api.Request.GetDialogs();
            messagesList.RemoveAll(x => x.user_id < 0);
            Memory.messages.AddRange(messagesList);

            int[] userIds = messagesList.Select(x => x.user_id).ToArray();
            List<User> usersList = await Api.Request.GetUsers(userIds, "photo_50,online,last_seen,online_mobile,sex");
            Memory.users.AddRange(usersList);

            foreach (Message message in messagesList)
            {
                var d = new Dialog(message.user_id);
                Memory.dialogs.Add(d);
            }


            
            LongPoll.Start();
            //System.Threading.Thread.Sleep(3000);

            this.DialogResult = true;
            this.Close();
        }
        
    }
}
