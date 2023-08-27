using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Bots
{
    public interface ITelegramBot
    {
        public Task StartAsync();

        public Task StopAsync();
    }
}
