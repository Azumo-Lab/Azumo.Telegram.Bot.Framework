using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Framework.Abstract.Models;

namespace Telegram.Bot.Framework.Abstract.PrivateChats
{
    public interface IOnline
    {
        /// <summary>
        /// 获取全部在线用户
        /// </summary>
        /// <returns></returns>
        public List<TelegramUser> GetOnlineUser();
    }
}
