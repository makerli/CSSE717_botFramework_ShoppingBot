using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Murph_Bot1.Controllers;
using System.Collections.Generic;

namespace Murph_Bot1
{
    //The BotAuthentication decoration on the method is used to validate your Bot Connector credentials over HTTPS.
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public static Dictionary<string, int> pro_Dic = new Dictionary<string, int>();
        public static Dictionary<int, string> back_Dic = new Dictionary<int, string>();
        public static string StartLocation = null;
        public static string EndLocation = null;
        public static string ProductName = null;
        public static Luis_JSON JSON_Obj1 = null;
        public static Luis_JSON JSON_Obj0 = null;

        public static int[,] array =
        {
            {1,1,1,1,0,1 },
            {1,0,0,0,1,1 },
            {1,0,0,0,1,1 },
            {1,1,0,1,1,1 }
        };
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// 程序的入口点
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            Add_dic();
            Add_backDic();
            if (activity.Type == ActivityTypes.Message)
            {
                JSON_Obj0 = JSON_Obj1;
                JSON_Obj1 = await GetEntityFromLUIS(activity.Text);
                string answer_From_handler = null;
                using (MessageHandler handler = new MessageHandler(JSON_Obj1.query, JSON_Obj1.intents[0].intent, JSON_Obj1.entities[0].entity))
                {
                    answer_From_handler = handler.handleMessage();
                }
                //以下是将回复发送出去的程序块
                Activity reply = activity.CreateReply($"{answer_From_handler}");
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                await connector.Conversations.ReplyToActivityAsync(reply);

                //// calculate something for us to return
                //int length = (activity.Text ?? string.Empty).Length;
                //// return our reply to the user
                //Activity reply = activity.CreateReply($"HeyBoy!You sent {activity.Text} which was {length} characters");
                ////await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                await connector.Conversations.ReplyToActivityAsync(HandleSystemMessage(activity));
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
                return message.CreateReply("Hello! what are you pinging for ?");
            }

            return null;
        }



        private static async Task<Luis_JSON> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            Luis_JSON Data = new Luis_JSON();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://api.projectoxford.ai/luis/v1/application?id=038d176f-e645-4b41-86b8-f92b081335d8&subscription-key=6800a40f195e47dcab2af8cf29e97c11&q=" + Query;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<Luis_JSON>(JsonDataResponse);
                }
            }
            return Data;
        }

        public void Add_dic()
        {
            pro_Dic.Add("拖鞋", 0);
            pro_Dic.Add("内衣", 1);
            pro_Dic.Add("零食", 2);
            pro_Dic.Add("酒水", 3);
            pro_Dic.Add("进口食品", 4);
            pro_Dic.Add("母婴用品", 5);
            pro_Dic.Add("沐浴露", 6);
            pro_Dic.Add("电梯1", 7);
            pro_Dic.Add("天井1", 8);
            pro_Dic.Add("天井2", 9);
            pro_Dic.Add("饮料", 10);
            pro_Dic.Add("奶粉", 11);
            pro_Dic.Add("牙膏", 12);
            pro_Dic.Add("电梯2", 13);
            pro_Dic.Add("天井3", 14);
            pro_Dic.Add("电梯3", 15);
            pro_Dic.Add("肉类", 16);
            pro_Dic.Add("海鲜", 17);
            pro_Dic.Add("清洁用品", 18);
            pro_Dic.Add("纸巾", 19);
            pro_Dic.Add("休息区", 20);
            pro_Dic.Add("家电", 21);
            pro_Dic.Add("水果", 22);
            pro_Dic.Add("蔬菜", 23);
        }

        public void Add_backDic()
        {
            back_Dic.Add(0, "拖鞋");
            back_Dic.Add(1, "内衣");
            back_Dic.Add(2, "零食");
            back_Dic.Add(3, "酒水");
            back_Dic.Add(4, "进口食品");
            back_Dic.Add(5, "母婴用品");
            back_Dic.Add(6, "沐浴露");
            back_Dic.Add(7, "电梯1");
            back_Dic.Add(8, "天井1");
            back_Dic.Add(9, "天井2");
            back_Dic.Add(10, "饮料");
            back_Dic.Add(11, "奶粉");
            back_Dic.Add(12, "牙膏");
            back_Dic.Add(13, "电梯2");
            back_Dic.Add(14, "天井3");
            back_Dic.Add(15, "电梯3");
            back_Dic.Add(16, "肉类");
            back_Dic.Add(17, "海鲜");
            back_Dic.Add(18, "清洁用品");
            back_Dic.Add(19, "纸巾");
            back_Dic.Add(20, "休息区");
            back_Dic.Add(21, "家电");
            back_Dic.Add(22, "水果");
            back_Dic.Add(23, "蔬菜");
        }
    }
}