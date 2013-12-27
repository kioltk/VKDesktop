using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKDesktop.Core.Users;

namespace VKDesktop.Models
{
    public class OnlinePopup : SimplePopup
    {
        User user;
        public override string BalloonText
        {
            get { return (user.Online ? "Зашёл в сеть" : "Вышел из сети"); }
        }

        public override string BalloonTitle
        {
            get { return user.Name; }
        }

        public override string BalloonImage
        {
            get { return user.photo_50; }
        }

        public override void grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Close();
        }
        public OnlinePopup(User user)
            : base(2500)
        {
            this.user = user;
            Initialize();
        }
    }
}
