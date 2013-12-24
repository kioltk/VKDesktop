using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKDesktop.Helpers
{
    public class Time
    {
        public static int EpochNow
        {
            get
            {
                var value = DateTime.UtcNow;
                TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

                return (int) span.TotalSeconds;
            }
        }
    }
}
