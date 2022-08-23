using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Framework.DependencyInjection;

namespace Telegram.Bot.Framework
{
    public interface ISetUp
    {
        public void Config(IServiceCollection telegramServices);
    }
}
