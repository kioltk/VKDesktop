using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKDesktop.Models
{
    public class Start : INotifyPropertyChanged
    {
        private string state;
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }
        public string Summary
        {
            get
            {
                return 
                    "Версия " + Helpers.Version.GetCurrent() + 
                    "\n" +
                    " Это неофициальное приложение";
            }
        }
        public Start()
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
