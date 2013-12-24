using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
namespace VKDesktop.Core.Messages
{
    public class Message : INotifyPropertyChanged
    {

        public int read_state
        {
            set
            {
                unread = (value == 0 ? true : false);
            }
        }
        [JsonProperty("out")]
        public int _out
        {
            set
            {
                mine = (value == 1 ? true : false);
            }
        }
        private bool _sending;
        public bool sending
        {
            get;
            set;
        }
        private int _id;
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                if (sending)
                {
                    sending = false;
                    OnPropertyChanged("sending");
                }

            }
        }
        public int date
        {
            get;
            set;
        }
        public int user_id
        {
            get;
            set;
        }
        public bool unread
        {
            get;
            set;
        }
        public bool mine
        {
            get;
            set;
        }
        public string body
        {
            get;
            set;
        }
        public Message()
        {
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
