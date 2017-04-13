using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Murph_Bot1.Controllers
{
    public class MessageHandler : IDisposable
    {
        public string query { get; set; }
        public string intent { get; set; }
        public string entity { get; set; }

        public MessageHandler(string query, string intent, string entity)
        {
            this.query = query;
            this.intent = intent;
            this.entity = entity;
        }

        public string handleMessage()
        {
            string response = null;
            switch (intent)
            {
                case "问好":
                    response = $"{entity}哟\n" + "我是导购机器人murph , 欢迎为你服务";
                    break;

                case "查询商品位置":
                    response = searchGoods(entity);
                    break;

                case "查询路线":
                    if (MessagesController.StartLocation != null && MessagesController.EndLocation != null)
                        response = searchPath(MessagesController.StartLocation, MessagesController.EndLocation);
                    //调用searchPath方法算出
                    else if (MessagesController.StartLocation == null && MessagesController.EndLocation != null)
                        response = "我还不知道你现在的位置哦，把你现在的位置告诉我或者扫一下旁边的二维码吧";
                    else if (MessagesController.StartLocation != null && MessagesController.EndLocation == null)
                        response = "我不清楚你要去哪里呢，告诉我你要去哪里吧";
                    else response = "我还不知道你要去哪里也不知道你在什么地方呢，把这些告诉我就带你去哈";
                    break;

                case "确认":
                    MessagesController.EndLocation = null;
                    MessagesController.StartLocation = null;
                    response = "好的，很高兴帮到你";
                    break;

                case "获取位置":
                    MessagesController.StartLocation = entity;
                    //如果最后顾客找到了商品位置要将这个位置信息删掉；
                    response = "ok, 我已经知道了哈";
                    break;
                default:
                    break;
            }

            return response;
        }

        public string searchGoods(string productName)
        {
            int location_Code = MessagesController.pro_Dic[productName];
            return $"你要找的{productName}就在第{Y(location_Code)}区域的第{X(location_Code)}货架上哟 ";
        }

        public string searchPath(string StartLoacation, string EndLocation)
        {
            int Start_Code = MessagesController.pro_Dic[StartLoacation];
            int End_Code = MessagesController.pro_Dic[EndLocation];
            int y = Y(Start_Code);
            int x = X(Start_Code);
            int y2 = Y(End_Code);
            int x2 = X(End_Code);
            string near_name = null;
            string middle_name = null;
            //if (MessagesController.array[x,y] != 0)
            //{
            //}
            bool up = (x > x2);
            bool right = (y2 > y);
            if (up && right)  //右上
            {
                if (MessagesController.array[x - 1, y] != 0)   //向上
                {
                    near_name = MessagesController.back_Dic[back_Num(x - 1, y)];
                    middle_name = MessagesController.back_Dic[back_Num(x2, y)];
                    return $"你可以向着{near_name}的方向走{(x - x2) * 50}米，"
                        + $"到达{middle_name}，"
                        + $"然后再向右拐{(y2 - y) * 50}米，"
                        + $"然后你就能见到{EndLocation}了";
                }
                else if (MessagesController.array[x, (y + 1)] != 0)  //向右
                {
                    near_name = MessagesController.back_Dic[back_Num(x, y + 1)];
                    middle_name = MessagesController.back_Dic[back_Num(x, y2)];
                    return $"你可以向着{near_name}的方向走{(y2 - y) * 50}米，"
                        + $"到达{middle_name}，"
                        + $"然后再向左拐{(x - x2) * 50}米，"
                        + $"然后你就能见到{EndLocation}了";
                }
                else return "what?";
            }
            else if ((!up) && (right))  //右下
            {
                if (MessagesController.array[x + 1, y] != 0)   //向下
                {
                    near_name = MessagesController.back_Dic[back_Num(x + 1, y)];
                    middle_name = MessagesController.back_Dic[back_Num(x2, y)];
                    return $"你可以向着{near_name}的方向走{(x - x2) * 50}米，"
                        + $"到达{middle_name}，"
                        + $"然后再向左拐{(y2 - y) * 50}米，"
                        + $"然后你就能见到{EndLocation}了";
                }
                else if (MessagesController.array[x, (y + 1)] != 0)  //向右
                {
                    near_name = MessagesController.back_Dic[back_Num(x, y + 1)];
                    middle_name = MessagesController.back_Dic[back_Num(x, y2)];
                    return $"你可以向着{near_name}的方向走{(y2 - y) * 50}米，"
                        + $"到达{middle_name}，"
                        + $"然后再向右拐{(x - x2) * 50}米，"
                        + $"然后你就能见到{EndLocation}了";
                }
                else return "what?";
            }
            else if ((!up) && (!right))  //左下
            {
                if (MessagesController.array[x + 1, y] != 0)   //向下
                {
                    near_name = MessagesController.back_Dic[back_Num(x + 1, y)];
                    middle_name = MessagesController.back_Dic[back_Num(x2, y)];
                    return $"你可以向着{near_name}的方向走{(x2 - x) * 50}米，"
                        + $"到达{middle_name}，"
                        + $"然后再向右拐{(y - y2) * 50}米，"
                        + $"然后你就能见到{EndLocation}了";
                }
                else if (MessagesController.array[x, (y + 1)] != 0)  //向左
                {
                    near_name = MessagesController.back_Dic[back_Num(x, y + 1)];
                    middle_name = MessagesController.back_Dic[back_Num(x, y2)];
                    return $"你可以向着{near_name}的方向走{(y - y2) * 50}米，"
                        + $"到达{middle_name}，"
                        + $"然后再向左拐{(x - x2) * 50}米，"
                        + $"然后你就能见到{EndLocation}了";
                }
                else return "what?";
            }
            else if ((!up) && (!right))  //左上
            {
                if (MessagesController.array[x - 1, y] != 0)   //向上
                {
                    near_name = MessagesController.back_Dic[back_Num(x - 1, y)];
                    middle_name = MessagesController.back_Dic[back_Num(x2, y)];
                    return $"你可以向着{near_name}的方向走{(x - x2) * 50}米，"
                        + $"到达{middle_name}，"
                        + $"然后再向右拐{(y - y2) * 50}米，"
                        + $"然后你就能见到{EndLocation}了";
                }
                else if (MessagesController.array[x, (y - 1)] != 0)  //向左
                {
                    near_name = MessagesController.back_Dic[back_Num(x, y - 1)];
                    middle_name = MessagesController.back_Dic[back_Num(x, y2)];
                    return $"你可以向着{near_name}的方向走{(y - y2) * 50}米，"
                        + $"到达{middle_name}，"
                        + $"然后再向右拐{(x - x2) * 50}米，"
                        + $"然后你就能见到{EndLocation}了";
                }
                else return "what?";
            }
            else return "what?";
        }

        public int Y(int code) { return code - 6 * ((int)(code / 6)); }

        public int X(int code) { return (int)(code / 6); }

        public int back_Num(int x, int y) { return (x * 6 + y); }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~MessageHandler() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}