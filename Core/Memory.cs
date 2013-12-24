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


        public static List<User> users = new List<User>();
        public static List<Message> messages = new List<Message>();
        public static List<Message> sending
        {
            get
            {
                return messages.Where(m => m.sending).ToList();
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

        public static void NewMessage(Message message)
        {
            Message maybeExist = messages.Find(x => x.id == message.id);
            if (maybeExist == null)
            {
                if (message.mine)
                {
                    Message maybeSending = sending.Find(m => m.body == message.body);// устроить поиск по прикреплениям
                    if (maybeSending != null)
                    {
                        maybeSending.id = message.id;
                        return;
                    }
                    else
                    {
                        Message maybeError = errored.Find(m => m.body == message.body);// устроить поиск по прикреплениям
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

            }
            

        }
        public static List<Message> GetMesages(int user_id)
        {
            return messages.Where(x => x.user_id == user_id).ToList();
        }
        public static void NotifyNewMessage(Message message)
        {

            Dialog d = dialogs.First(x => x.User.id == message.user_id);

            if (!d.IsFocused)
            {
                Notificator.PlaySound(Notificator.NotificationSound.Message);
                
            }

        }
    }
}
