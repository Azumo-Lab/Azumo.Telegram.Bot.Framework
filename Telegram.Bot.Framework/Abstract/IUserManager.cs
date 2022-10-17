using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstract
{
    public interface IUserManager
    {
        /// <summary>
        /// 获取我自己
        /// </summary>
        TelegramUser Me { get; }

        /// <summary>
        /// 获取管理员用户
        /// </summary>
        TelegramUser Admin { get; }

        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <returns></returns>
        List<TelegramUser> GetUsers();

        /// <summary>
        /// 随机获取一个用户
        /// </summary>
        /// <returns></returns>
        TelegramUser RandomUser();

        /// <summary>
        /// 屏蔽用户
        /// </summary>
        /// <param name="user"></param>
        void Block(TelegramUser user);

        /// <summary>
        /// 解除屏蔽
        /// </summary>
        /// <param name="user"></param>
        void Restore(TelegramUser user);
    }
}
