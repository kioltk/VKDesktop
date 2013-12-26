using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VKDesktop.Core.Messages;
using VKDesktop.Core.Users;

namespace VKDesktop.Api
{
    class Request
    {
        
        static string getUrl(string method_name, string parameters)
        {
            return "https://api.vk.com/method/" + method_name + "?" + parameters + "&v=5.2&access_token=" + Access.access_token;
        }
        
        public static Task<string> GetLoaderTask(string method, string parameters)
        {

            string url = getUrl(method, parameters);

            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            
            Task<string> getStringTask = (client).DownloadStringTaskAsync(new Uri(url));
            
            return getStringTask;
        }

        public async static Task<User> GetUser()
        {
            var loadingTask = GetLoaderTask("users.get", "");

            string responseData = await loadingTask;
            Response<User[]> a = JsonConvert.DeserializeObject<Response<User[]>>(responseData);
            return a.response[0];
        }
        public async static Task<List<Core.Users.User>> GetUsers(int[] user_ids, string fields)
        {
            string parameters = "user_ids=";
            foreach (int id in user_ids)
            {
                parameters += id + ",";
            }
            parameters.Substring(0, parameters.Length);
            parameters += "&fields=" + fields;

            var loadingTask = GetLoaderTask("users.get", parameters);
            
            string responseData = await loadingTask;
            Response<User[]> a = JsonConvert.DeserializeObject<Response<User[]>>(responseData);
            return a.response.ToList();
        }

        public async static Task<int> SendMessage(int user_id, string messageText)
        {
            var loadingTask = GetLoaderTask("messages.send", "message=" + messageText + "&user_id=" + user_id);
            string responseData = await loadingTask;

            Response<int> a = JsonConvert.DeserializeObject<Response<int>>(responseData);
            return a.response;
        }
        public async static Task<int> MarkAsRead(int user_id)
        {
            var loadingTask = GetLoaderTask("messages.markAsRead", "user_id=" + user_id);
            string responseData = await loadingTask;

            Response<int> a = JsonConvert.DeserializeObject<Response<int>>(responseData);
            return a.response;
        }
        public async static Task<List<Message>> GetMessages(int user_id,int offset, string fields)
        {

            var loadingTask = GetLoaderTask("messages.getHistory", "user_id=" + user_id + "&offset=" + offset);
            
            string responseData = await loadingTask; // выходит из метода

            Response<Messages> a = JsonConvert.DeserializeObject<Response<Messages>>(responseData);
            return a.response.items.ToList();
            
        }

        public async static Task<List<Message>> GetDialogs()
        {

            var loadingTask = GetLoaderTask("messages.getDialogs", "");

            string responseData = await loadingTask;

            Response<Messages> a = JsonConvert.DeserializeObject<Response<Messages>>(responseData);
            return a.response.items;

        }



        public async static Task<bool> SetOffline()
        {
            var loadingTask = GetLoaderTask("account.setOffline", "voip=0");
            string responseData = await loadingTask;

            Response<int> a = JsonConvert.DeserializeObject<Response<int>>(responseData);
            return (a.response == 0 ? true : false);

        }
        public async static Task<bool> SetOnline()
        {
            var loadingTask = GetLoaderTask("account.setOnline", "voip=0");
            string responseData = await loadingTask;

            Response<int> a = JsonConvert.DeserializeObject<Response<int>>(responseData);
            return (a.response == 0 ? true : false);

        }

    }
}
