using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKDesktop.Core.Users;

namespace VKDesktop.Core
{
    public class Account
    {
        public static User CurrentUser
        {
            get;
            set;
        }
        public async static void SetOnline()
        {
             await Api.Request.SetOnline();
        }
        public async static void SetOffline()
        {
            await Api.Request.SetOffline();
        }
    }
}
