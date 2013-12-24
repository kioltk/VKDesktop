using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKDesktop.Core.Messages;

namespace VKDesktop.Api
{
    class Response<T>
    {


        public T response
        {
            get;
            set;
        }
        
    }
    public class Messages
    {

        public int count;
        public List<Message> items;
        public Messages()
        {
        }
    }
    
}
