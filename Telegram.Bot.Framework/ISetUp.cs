using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework
{
    public interface ISetUp
    {
        public void Config(IServiceCollection telegramServices);
    }
}
