using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKDesktop.Core.Messages
{
    public class LongPoll
    {
        public static async void getServer()
        {
            Task<string> getLongPollTask = Api.Request.GetLoaderTask("messages.getLongPollServer", "");
            
        }
        public Nullable<int> failed
        {
            get;
            set;
        }
        public int ts
        {
            get;
            set;
        }
        public string server
        {
            get;
            set;
        }
        public string key
        {
            get;
            set;
        }
        public List<Object[]> updates
        {
            get;
            set;
        }
        public LongPoll()
        {
        }
    }
}
