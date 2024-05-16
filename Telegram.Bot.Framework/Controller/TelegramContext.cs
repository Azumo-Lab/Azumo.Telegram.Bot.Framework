using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Storage;

namespace Telegram.Bot.Framework.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TelegramContext
    {
        /// <summary>
        /// 
        /// </summary>
        public TelegramRequest TelegramRequest { get; }

        /// <summary>
        /// 
        /// </summary>
        public ISession Session { get; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceScope ServiceScope { get; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider => ServiceScope.ServiceProvider; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="telegramRequest"></param>
        internal TelegramContext(IServiceProvider serviceProvider, TelegramRequest telegramRequest)
        {
            Session = serviceProvider.GetRequiredService<ISession>();

            ServiceScope = serviceProvider.CreateScope();
            TelegramRequest = telegramRequest;
        }
    }
}
