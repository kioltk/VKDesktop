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

            string url = String.Format("http://{0}?act=a_check&key={1}&ts={2}&wait=25&mode=96", server, key, ts);

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
                try
                {
                    await StartNew();
                }
                catch (Exception exp)
                {
                    string s = exp.Message;
                }
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
                    case 0:
                        //0,$message_id,0 -- удаление сообщения с указанным local_id
                        break;
                    case 1:
                        //1,$message_id,$flags -- замена флагов сообщения (FLAGS:=$flags)
                        break;
                    case 2:
                        //2,$message_id,$mask[,$user_id] -- установка флагов сообщения (FLAGS|=$mask)
                        break;
                    case 3:
                        {
                            //3,$message_id,$mask[,$user_id] -- сброс флагов сообщения (FLAGS&=~$mask) -- приходит 1 в маске, если прочитано
                            int message_id = Convert.ToInt32(update[1]);
                            int[] mask = Converter.Flags(Convert.ToInt32(update[2]));
                            if (mask.Contains(1))
                                Memory.Read(message_id);
                        }
                        break;
                    case 4:
                        //4,$message_id,$flags,$from_id,$timestamp,$subject,$text,$attachments -- добавление нового сообщения
                        var m = Converter.Message(update);
                        Memory.NewMessage(m);
                        break;
                    case 8:
                        {
                            int user_id = -Convert.ToInt32(update[1]);
                            bool mobile = (Convert.ToInt32(update[2]) != 7);
                            Memory.ShowOnline(user_id, mobile);
                        }
                        //8,-$user_id,$extra-- друг $user_id стал онлайн, $extra не равен 0, если в mode был передан флаг 64, в младшем байте (остаток от деления на 256) числа $extra лежит идентификатор платформы (таблица ниже)
                        break;
                    case 9:
                        {
                            int user_id = -Convert.ToInt32(update[1]);
                            Memory.ShowOffline(user_id);
                        }
                        //9,-$user_id,$flags -- друг $user_id стал оффлайн ($flags равен 0, если пользователь покинул сайт (например, нажал выход) и 1, если оффлайн по таймауту (например, статус away))
                        break;
                    case 61:
                        {
                            int user_id = Convert.ToInt32(update[1]);
                            Memory.ShowTypping(user_id);
                        }
                        //61,$user_id,$flags -- пользователь $user_id начал набирать текст в диалоге. событие должно приходить раз в ~5 секунд при постоянном наборе текста. $flags = 1
                        break;
                    case 62:
                        //62,$user_id,$chat_id -- пользователь $user_id начал набирать текст в беседе $chat_id.
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
                m.Date = Convert.ToInt32(array[4]);
                m.Body = Convert.ToString(array[6]);



                int[] flags = Flags(Convert.ToInt32(array[2]));

                foreach (int flag in flags)
                {
                    switch (flag)
                    {
                        case 1:
                            m.Unread = true;
                            break;
                        case 2:
                            m.Mine = true;
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
