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
    public class ListItems<T>
    {

        public int count;
        public List<T> items;
        public ListItems()
        {
        }
    }
    
    
}
