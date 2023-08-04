using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.User
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// 用户的总数
        /// </summary>
        /// <remarks>
        /// 用于显示当前用户的总数，当前用户的总数计算是24小时内向Bot发送过任意消息的用户
        /// </remarks>
        public int UserCount { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="serviceScope"></param>
        /// <returns></returns>
        public IChat CreateIChat(ITelegramBotClient botClient, Update update, IServiceScope serviceScope);

        public void UpdateUserCount();
    }
}
