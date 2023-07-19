using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.User
{
    public interface IUserManager
    {
        public int UserCount { get; }

        public IChat CreateIChat(ITelegramBotClient botClient, Update update, IServiceScope serviceScope);
    }
}
