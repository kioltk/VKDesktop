using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKDesktop.Core.Messages;
using VKDesktop.Core.Users;
using VKDesktop.Helpers;

namespace VKDesktop.Core
{
    public class Memory
    {
        private static string state = "";
        public static string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                Models.TrayTooltip.Get.State = value;
            }
        }

        public static List<User> users = new List<User>();
        public static List<Message> messages = new List<Message>();
        public static List<Message> sending
        {
            get
            {
                return messages.Where(m => m.Sending).ToList();
            }
        }
        public static List<Message> errored
        {
            get
            {
                return messages.Where(x => x.id == 0).ToList();
            }
        }
        public static ObservableCollection<Dialog> dialogs = new ObservableCollection<Dialog>();

        public static User GetUser(int user_id)
        {
            return users.Find(x => x.id == user_id);
        }
        public static Dialog GetDialog(int user_id)
        {
            return GetUser(user_id).Dialog;
        }

        public static void ShowOnline(int user_id,bool mobile) 
        {
            User user = GetUser(user_id);
            if(user!=null)
                user.ShowOnline(mobile);



        }
        public static void ShowOffline(int user_id)
        {
            User user = GetUser(user_id);
            if (user != null)
                user.ShowOffline();


        }
        public static void ShowTypping(int user_id)
        {
            GetDialog(user_id).ShowTypping();
        }
        public static void NewMessage(Message message)
        {
            Message maybeExist = messages.Find(x => x.id == message.id);
            if (maybeExist == null)
            {
                if (message.Mine)
                {
                    Message maybeSending = sending.Find(m => m.Body == message.Body);// устроить поиск по прикреплениям
                    if (maybeSending != null)
                    {
                        maybeSending.id = message.id;
                        return;
                    }
                    else
                    {
                        Message maybeError = errored.Find(m => m.Body == message.Body);// устроить поиск по прикреплениям
                        if (maybeError != null)
                        {
                            maybeSending.id = message.id;
                            return;
                        }
                    }
                }
                else
                {
                    NotifyNewMessage(message);
                }
                messages.Add(message);
                var dialog = dialogs.First(d => d.User.id == message.user_id);
                dialog.NewMessage(message);
                UpDialog(dialog);
            }
            

        }
        public static void Read(int message_id)
        {
            Message message =  messages.Find(m => m.id == message_id);
            if (message != null)
            {
                message.Unread = false;
            }
        }
        public static void NotifyNewMessage(Message message)
        {

            Dialog d = dialogs.First(x => x.User.id == message.user_id);

            if (d.IsOpened)
            {
                if (d.IsFocused)
                {
                    return;
                }
                else
                    Notificator.ShowMessagePopup(message);
            }
            else
            {
                Notificator.ShowMessagePopup(message);
                Dialog dialog = d;
                DialogWindow dialogWindow = new DialogWindow(d);
                dialogWindow.ShowActivated = false;
                dialogWindow.Show();
                //flash
            }
            Notificator.PlaySound(Notificator.NotificationSound.Message);




        }

        public static void UpDialog(Dialog d)
        {
            dialogs.Move(dialogs.IndexOf(d), 0);
        }
        
        public static List<Message> GetMesages(int user_id)
        {
            return messages.Where(x => x.user_id == user_id).ToList();
        }

        public static MainWindow MainWindow 
        {
            get; 
            set;
        }
        public static bool IsOpened
        {
            get
            {
                return (MainWindow == null ? false : true);
            }
        }
        public static bool IsFocused
        {
            get{
                if (IsOpened)
                   return MainWindow.IsActive;
                return false;
            }
        }

    }
}
