using System.Collections.Generic;
using Telegram.Bot.Framework.Abstract.Users;

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
