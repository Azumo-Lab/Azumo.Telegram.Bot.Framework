using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.DependencyInjection
{
    internal class TelegramServiceProvider : IServiceProvider
    {
        internal TelegramServiceProvider()
        {

        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
