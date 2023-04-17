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

        /// <summary>
        /// 随机获取一个用户
        /// </summary>
        /// <returns></returns>
        public TelegramUser GetRandomUser();

        /// <summary>
        /// 获取管理员用户
        /// </summary>
        /// <returns></returns>
        public TelegramUser GetAdmin();
    }
}
