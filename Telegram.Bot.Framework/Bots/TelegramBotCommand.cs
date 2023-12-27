using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.Bots
{
    internal class TelegramBotCommand(Delegate handle) : ITelegramPartCreator
    {
        public void AddBuildService(IServiceCollection services) => 
            services.AddSingleton(handle);

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        public static ITelegramBotBuilder AddCommand(this ITelegramBotBuilder telegramBotBuilder, Func<Task> func) =>
            telegramBotBuilder.AddTelegramPartCreator(new TelegramBotCommand(func));

        public static ITelegramBotBuilder AddCommand<T1>(this ITelegramBotBuilder telegramBotBuilder, Func<T1, Task> func) =>
            telegramBotBuilder.AddTelegramPartCreator(new TelegramBotCommand(func));

        public static ITelegramBotBuilder AddCommand<T1, T2>(this ITelegramBotBuilder telegramBotBuilder, Func<T1, T2, Task> func) =>
            telegramBotBuilder.AddTelegramPartCreator(new TelegramBotCommand(func));

        public static ITelegramBotBuilder AddCommand<T1, T2, T3>(this ITelegramBotBuilder telegramBotBuilder, Func<T1, T2, T3, Task> func) =>
            telegramBotBuilder.AddTelegramPartCreator(new TelegramBotCommand(func));

        public static ITelegramBotBuilder AddCommand<T1, T2, T3, T4>(this ITelegramBotBuilder telegramBotBuilder, Func<T1, T2, T3, T4, Task> func) =>
            telegramBotBuilder.AddTelegramPartCreator(new TelegramBotCommand(func));

        public static ITelegramBotBuilder AddCommand<T1, T2, T3, T4, T5>(this ITelegramBotBuilder telegramBotBuilder, Func<T1, T2, T3, T4, T5, Task> func) =>
            telegramBotBuilder.AddTelegramPartCreator(new TelegramBotCommand(func));

        public static ITelegramBotBuilder AddCommand<T1, T2, T3, T4, T5, T6>(this ITelegramBotBuilder telegramBotBuilder, Func<T1, T2, T3, T4, T5, T6, Task> func) =>
            telegramBotBuilder.AddTelegramPartCreator(new TelegramBotCommand(func));
    }
}
