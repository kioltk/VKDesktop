using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKDesktop.Core.Messages;

namespace VKDesktop.Core.Users
{
    public class User : INotifyPropertyChanged
    {
        [JsonProperty("online")]
        public int _online
        {
            set
            {
                Online = (value == 1 ? true : false);
                OnPropertyChanged("Online");
                OnPropertyChanged("Seen");
            }
        }
        [JsonProperty("online_mobile")]
        public int _online_mobile
        {
            set
            {
                OnlineMobile = (value == 1 ? true : false);
                OnPropertyChanged("OnlineMobile");
            }
        }

        public int id
        {
            get;
            set;
        }
        public string Name
        {
            get
            {
                return last_name + " " + FirstName;
            }
        }
        [JsonProperty("first_name")]
        public string FirstName
        {
            get;
            set;
        }
        public string last_name
        {
            get;
            set;
        }
        public string photo_50
        {
            get;
            set;
        }
        public string photo_100
        {
            get;
            set;
        }
        
        public Sex sex
        {
            get;
            set;
        }
        public LastSeen last_seen
        {
            get;
            set;
        }

        [JsonIgnore]
        public bool OnlineMobile
        {
            get;
            set;
        }
        [JsonIgnore]
        public bool Online
        {
            get;
            set;
        }
        public string Seen
        {
            get
            {
                if (!Online)
                {
                    var time = Helpers.Time.FromEpoch(last_seen.time);
                    if (time.Length < 6)
                       time = time.Insert(0, "в ");
                    return (IsWoman ? "Была" : "Был") + " тут " + time +(last_seen.platform != 7 ? " с мобильного " : "");
                }
                return "";
            }
        }
        
            public bool IsWoman
        {
            get
            {
                return (sex == Sex.Woman ? true : false);
            }
        }
        
        public void SetOnline(bool mobile)
        {
            _online = 1;
            _online_mobile = (mobile ? 1 : 0);
            last_seen = null;
        }
        public void SetOffline()
        {
            _online = 0;
            
            last_seen = new LastSeen()
            {
                time = Helpers.Time.EpochNow,
                platform = ( OnlineMobile ? 1 : 7)
            };
        }
        public Dialog Dialog
        {
            get;
            set;
        }

        public User()
        {
        }

        
        public class LastSeen
        {

            public int time
            {
                get;
                set;
            }
            public int platform
            {
                get;
                set;
            }
            
        }
        public enum Sex{
            None = 0,
            Woman = 1,
            Man = 2
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
