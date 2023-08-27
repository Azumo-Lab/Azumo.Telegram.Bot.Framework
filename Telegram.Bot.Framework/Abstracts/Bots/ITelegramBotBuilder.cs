using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Bots
{
    public interface ITelegramBotBuilder
    {
        public IServiceCollection RuntimeService { get; }

        public Dictionary<string, object> Arguments { get; }

        public ITelegramBot Build();
    }
}
