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
namespace VKDesktop.Core.Messages
{
    public class Dialog : INotifyPropertyChanged
    {
        public Message Last
        {
            get
            {
                return messages.Last();
            }
        }
        ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get
            {
                return messages;
            }
        }
        User user;
        public User User
        {
            get
            {
                return user;
            }
        }
        public Dialog(int user_id)
        {
            user = Memory.GetUser(user_id);
            messages = new ObservableCollection<Message>(Memory.GetMesages(user_id));
        }


        public async void LoadMore()
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



        public void NewMessage(Message message)
        {   
            messages.Add(message);
            OnPropertyChanged("Last");
        }
        public async void SendMessage(string messageText)
        {
            Message m = new Message()
            {
                mine = true,
                body = messageText,
                sending = true,
                read_state = 0,
                user_id = user.id,
                date = Time.EpochNow
            };
            Memory.messages.Add(m);
            messages.Add(m);
            Task<int> sendMessageTask = Api.Request.SendMessage(user.id,messageText);
            int mid = await sendMessageTask;
            if (mid!=0)
            {
                m.id = mid;
                m.sending = false;
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
            get; set;
        }
    }
}
