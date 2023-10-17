using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Bots
{
    public interface ITelegramBotBuilder
    {
        public ITelegramBotBuilder AddTelegramPartCreator(ITelegramPartCreator telegramPartCreator);

        public ITelegramBot Build();
    }
}
