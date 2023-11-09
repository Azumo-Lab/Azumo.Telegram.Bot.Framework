using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Users
{
    public interface IChatManager
    {
        /// <summary>
        /// 创建 <see cref="TGChat"/> 对象
        /// </summary>
        /// <param name="telegramBotClient"></param>
        /// <param name="update"></param>
        /// <param name="BotServiceProvider"></param>
        /// <returns></returns>
        public TGChat Create(ITelegramBotClient telegramBotClient, Update update, IServiceProvider BotServiceProvider);
    }
}
