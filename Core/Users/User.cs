using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKDesktop.Core.Users
{
    public class User
    {
        [JsonProperty("online")]
        public int _online
        {
            set
            {
                online = (value == 1 ? true : false);
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
                return last_name + " " + first_name;
            }
        }
        public string first_name
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
        [JsonIgnore]
        public bool online
        {
            get;
            set;
        }
        public int last_seen
        {
            get;
            set;
        }

        public User()
        {
        }

    }
}
