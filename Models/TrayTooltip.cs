using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKDesktop.Core;

namespace VKDesktop.Models
{
    public class TrayTooltip : INotifyPropertyChanged
    {
        public string State
        {
            get
            {
                return Memory.State;
            }
            set
            {
                OnPropertyChanged("State");
            }
        }
        public string Name
        {
            get
            {
                return 
                    Core.Account.CurrentUser.Name;
            }
        }
        public string App
        {
            get
            {
                return "Вконтакте v" + Helpers.Version.GetCurrent();
            }
        }
        public TrayTooltip()
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
        public static TrayTooltip Get = new TrayTooltip();
    }
}
