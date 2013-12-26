using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VKDesktop.Helpers
{
    public class Version
    {
        public static string GetCurrent()
        {
            XmlDocument xmlDoc = new XmlDocument();
            Assembly asmCurrent = System.Reflection.Assembly.GetExecutingAssembly();
            
            var v = asmCurrent.GetName().Version;
            string resp = v.Major + "." + v.Minor;
            return resp;
        }
    }
}
