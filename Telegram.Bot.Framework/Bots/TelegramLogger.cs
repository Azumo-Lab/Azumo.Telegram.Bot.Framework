using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.Bots
{
    internal class TelegramLogger : ITelegramPartCreator
    {
        private readonly Action<ILoggingBuilder> _LogAction;
        public TelegramLogger(Action<ILoggingBuilder> action) 
        {
            _LogAction = action;
        }
        public void AddBuildService(IServiceCollection services)
        {
            services.AddSingleton(_LogAction);
        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        /// <summary>
        /// 添加Log设置
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ITelegramBotBuilder AddLogger(this ITelegramBotBuilder builder, Action<ILoggingBuilder> action)
        {
            return builder.AddTelegramPartCreator(new TelegramLogger(action));
        }
    }
}
