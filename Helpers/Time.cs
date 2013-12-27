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
                TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0));

                return (int) span.TotalSeconds;
            }
        }

        public static string FromEpoch(int epoch)
        {
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            var epochConverted = epochStart.AddSeconds(epoch);
            var now = DateTime.Now;

            string fullTime = "";

            string time = epochConverted.ToShortTimeString();
            int daysAgo = (now.Day - epochConverted.Day);
            if (daysAgo > 0)
            {
                if (daysAgo == 1)
                    fullTime += "вчера ";
                else
                {
                    fullTime += epochConverted.ToShortDateString() + " ";
                }
            }
            fullTime += time;

            return fullTime;
        }
    }
}
