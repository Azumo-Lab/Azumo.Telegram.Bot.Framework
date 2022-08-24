using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework
{
    internal class BaseSetUp : ISetUp
    {

        private readonly List<ISetUp> setUps;

        public BaseSetUp(List<ISetUp> setUps)
        {
            this.setUps = setUps;
        }

        public void Config(IServiceCollection telegramServices)
        {
            setUps.ForEach(x => x.Config(telegramServices));
        }
    }
}
