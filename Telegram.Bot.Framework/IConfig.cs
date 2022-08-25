using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework
{
    public interface IConfig
    {
        public void Config(IServiceCollection telegramServices);
    }
}
