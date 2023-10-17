using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.InternalImpl.Bots
{
    internal class TelegramServicesSetting : ITelegramPartCreator
    {
        private readonly Action<IServiceCollection> __ServiceSettingAction;
        public TelegramServicesSetting(Action<IServiceCollection> action) 
        {
            __ServiceSettingAction = action;
        }

        public void AddBuildService(IServiceCollection services)
        {
            
        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            __ServiceSettingAction(services);
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        public static ITelegramBotBuilder AddServices(this ITelegramBotBuilder telegramBotBuilder, Action<IServiceCollection> action)
        {
            return telegramBotBuilder.AddTelegramPartCreator(new TelegramServicesSetting(action));
        }
    }
}
