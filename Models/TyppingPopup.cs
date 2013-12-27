using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKDesktop.Core.Users;

namespace VKDesktop.Models
{
    public class TyppingPopup : SimplePopup
    {
        User user;
        public override string BalloonText
        {
            get { return "Набирает сообщение . . ."; }
        }

        public override string BalloonTitle
        {
            get 
            {
                return user.Name; }
        }

        public override string BalloonImage
        {
            get { return user.photo_50; }
        }

        public override void grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            user.Dialog.Open();
        }
        public TyppingPopup(User user, int? closeTiming) :base(closeTiming)
        {
            this.user = user;
            Initialize();
        }
    }
}
