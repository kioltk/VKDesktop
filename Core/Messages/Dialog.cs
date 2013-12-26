using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKDesktop.Core.Users;
using System.Net;
using System.Windows.Controls;
using VKDesktop.Helpers;
using System.ComponentModel;
using System.Windows; 
namespace VKDesktop.Core.Messages
{
    public class Dialog : INotifyPropertyChanged
    {
        
        ObservableCollection<Message> messages;
        User user;
        public User User
        {
            get
            {
                return user;
            }
        }
        public Message Last
        {
            get
            {
                return messages.Last();
            }
        }
        public ObservableCollection<Message> Messages
        {
            get
            {
                return messages;
            }
        }
        public bool HasUnread
        {
            get
            {
                return (messages.First(x => x.Unread) != null);
            }
        }
        public bool MarkingAsRead
        {
            get;
            set;
        }
        public Dialog(int user_id)
        {
            user = Memory.GetUser(user_id);
            user.Dialog = this;
            messages = new ObservableCollection<Message>(Memory.GetMesages(user_id));
        }

        public async Task LoadMore()
        {

            //but.Content = "Берём таск";
            Task<List<Message>> getMoreMessagesTask = Api.Request.GetMessages(user.id,messages.Count,"");

            //but.Content = "Взяли таск";
            List<Message> messagesFromResponse = await getMoreMessagesTask;
            Core.Memory.messages.AddRange(messagesFromResponse);
            foreach (Message message in messagesFromResponse)
            {
                this.messages.Insert(0,message);
            }
            //but.Content = "Таск ответил";
            
        }
        public async Task MarkAsRead()
        {
            MarkingAsRead = true;
            Task<int> getMarkAsReadTask = Api.Request.MarkAsRead(user.id);
            bool isMarked = (await getMarkAsReadTask == 1 ? true : false);
            MarkingAsRead = false;
        }
        
        public void ShowTypping()
        {
            if (IsOpened)
                Window.ShowTypping();
        }
        public void SendTypping()
        {
            // по таймеру
            // отправлять писалку
            // https://vk.com/dev/messages.setActivity
        }
        public void NewMessage(Message message)
        {   
            messages.Add(message);
            OnPropertyChanged("Last");
            ScrollDown();
            
        }
        public async void SendMessage(string messageText)
        {
            Message m = new Message()
            {
                Mine = true,
                Body = messageText,
                Sending = true,
                read_state = 0,
                user_id = user.id,
                Date = Time.EpochNow
            };
            Memory.messages.Add(m);
            messages.Add(m);
            ScrollDown();
            OnPropertyChanged("Last");
            Task<int> sendMessageTask = Api.Request.SendMessage(user.id, messageText);
            int mid = await sendMessageTask;
            if (mid != 0)
            {
                m.id = mid;
                m.Sending = false;
            }

        }
        public void ScrollDown()
        {
            if (IsOpened)
            {
                Window.DialogsList.ScrollIntoView(Last);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string arg)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(arg));
            }
        }

        public bool IsFocused
        {
            get
            {
                if (IsOpened)
                    return Window.IsActive;
                return false;
            }
        }
        public bool IsOpened 
        {
            get
            {
                return (Window == null ? false : true);
            } 
        }

        public DialogWindow Window
        {
            get;
            set;
        }

        public void Open()
        {
            Show.DialogWindow(this);
            
        }
    }

}
