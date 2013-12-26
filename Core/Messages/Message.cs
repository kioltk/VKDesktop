using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using VKDesktop.Core.Users;
namespace VKDesktop.Core.Messages
{
    public class Message : INotifyPropertyChanged
    {
        [JsonProperty("out")]
        public int _out
        {
            set
            {
                Mine = (value == 1 ? true : false);
            }
        }
        private int _id;
        private bool unread;
        public int read_state
        {
            set
            {
                unread = (value == 0 ? true : false);
            }
        }
        
        public bool Sending
        {
            get;
            set;
        }
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                if (Sending)
                {
                    Sending = false;
                    OnPropertyChanged("sending");
                }

            }
        }
        [JsonProperty("date")]
        public int Date
        {
            get;
            set;
        }
        public int user_id
        {
            get;
            set;
        }
        public bool Unread
        {
            get
            {
                return unread;
            }
            set
            {
                unread = value;
                OnPropertyChanged("Unread");
            }
        }
        public bool Mine
        {
            get;
            set;
        }
        [JsonProperty("body")]
        public string Body
        {
            get;
            set;
        }
        public Message()
        {
        }

        public User User
        {
            get
            {
                return Memory.GetUser(user_id);
            }
        }
        public User Owner
        {
            get
            {
                return (Mine ? Account.CurrentUser : User);
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

    }
}
