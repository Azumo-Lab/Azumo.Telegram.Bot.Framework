using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.PaymentMethod
{
    internal class TelegramPaymentMethod : ITelegramPartCreator
    {
        public void AddBuildService(IServiceCollection services)
        {

        }
        public void Build(IServiceCollection services, IServiceProvider builderService)
        {

        }
    }

    public static class Install
    {
        public static ITelegramBotBuilder AddPaymentService(this ITelegramBotBuilder telegramBotBuilder) => 
            telegramBotBuilder.AddTelegramPartCreator(new TelegramPaymentMethod());

        public static ITelegramBotBuilder AddPaidRole(this ITelegramBotBuilder telegramBotBuilder, string[] roles) =>
            telegramBotBuilder.AddTelegramPartCreator(new TelegramPaymentMethod());
    }
}
