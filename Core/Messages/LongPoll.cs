using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VKDesktop.Api;

namespace VKDesktop.Core.Messages
{
    public class LongPoll
    {
        public Nullable<int> failed
        {
            get;
            set;
        }
        [JsonProperty("updates")]
        public List<Object[]> updates
        {
            get;
            set;
        }
        [JsonProperty("history")]
        public List<Object[]> History
        {
            get;
            set;
        }
        public LongPoll()
        {
        }

        
        [JsonProperty("new_pts")]
        public static int new_pts
        {
            get
            {
                return pts;
            }
            set
            {
                pts = value;
            }
        }
        [JsonProperty("pts")]
        public static int pts
        {
            get;
            set;
        }
        [JsonProperty("ts")]
        public static int ts
        {
            get;
            set;
        }
        [JsonProperty("server")]
        public static string server
        {
            get;
            set;
        }
        [JsonProperty("key")]
        public static string key
        {
            get;
            set;
        }

        private static Task<string> RequestServerTask()
        {

            string url = String.Format("http://{0}?act=a_check&key={1}&ts={2}&wait=25&mode=96 ", server, key, ts);

            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            Task<string> getStringTask = client.DownloadStringTaskAsync(new Uri(url));

            return getStringTask;

        }

        public static async Task GetNewServer()
        {

            Task<string> getLongPollTask = Api.Request.GetLoaderTask("messages.getLongPollServer", "need_pts=1");

            string responseData = await getLongPollTask; // выходит из метода

            Response<LongPoll> a = JsonConvert.DeserializeObject<Response<LongPoll>>(responseData);

        }

        public async static void Start()
        {

            await LongPoll.GetNewServer();
            while (true)
            {
                await StartNew();
            }
        }
        private static async Task StartNew()
        {
            var task = RequestServerTask();

            string responseData = await task;

            LongPoll a = JsonConvert.DeserializeObject<LongPoll>(responseData);

            var longPoll = a;
            
            foreach (Object[] update in longPoll.updates)
            {
                switch (Convert.ToInt32(update[0]))
                {
                       
                    case 4:

                        var m = Converter.Message(update);
                        Memory.NewMessage(m);
                        break;
                        
                }
            }

        }

        public static async void GetHistory()
        {

            Task<string> getLongPollTask = Api.Request.GetLoaderTask("messages.getLongPollHistory", "pts=" + pts);

            string responseData = await getLongPollTask;

            Response<LongPoll> a = JsonConvert.DeserializeObject<Response<LongPoll>>(responseData);



        }
        class Converter
        {
            public static Message Message(Object[] array)
            {
                Message m = new Message();
                m.id = Convert.ToInt32(array[1]);
                
                m.user_id = Convert.ToInt32(array[3]);
                m.date = Convert.ToInt32(array[4]);
                m.body = Convert.ToString(array[6]);



                int[] flags = Flags(Convert.ToInt32(array[2]));

                foreach (int flag in flags)
                {
                    switch (flag)
                    {
                        case 1:
                            m.unread = true;
                            break;
                        case 2:
                            m.mine = true;
                            break;
                        case 8:
                            //imprtant
                            break;
                        case 16:
                            //chat
                            break;
                        case 34:
                            //from friend
                            break;
                        case 64:
                            //spam
                            break;
                        case 128:
                            //deleted
                            break;
                        case 256:
                            //not spam
                            break;
                        case 512:
                            //has attachment
                            break;
                    }
                }

                return m;
            }
            public static int[] Flags(int summary)
            {
                List<int> flags = new List<int>();

                int flagChecker = 512;
                while (summary != 0)
                {
                    if (summary - flagChecker >= 0)
                    {
                        flags.Add(flagChecker);
                        summary -= flagChecker;
                    }

                    flagChecker = flagChecker / 2;
                }


                return flags.ToArray();
            }
        }
    }
    
}
