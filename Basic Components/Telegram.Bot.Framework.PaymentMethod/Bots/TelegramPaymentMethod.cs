using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.PaymentMethod.Bots;

internal class TelegramPaymentMethod(PaymentOption paymentOption) : ITelegramPartCreator
{
    private readonly PaymentOption __PaymentOption = paymentOption;

    public void AddBuildService(IServiceCollection services)
    {

    }
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {
        _ = new RequestOptions()
        {
            ApiKey = __PaymentOption.Token,
        };
        _ = new ChargeService();
        //s.Get()
        _ = Task.CompletedTask;
    }
}

public static class Install
{
    public static ITelegramBotBuilder AddPaymentService(this ITelegramBotBuilder telegramBotBuilder, PaymentOption option) =>
        telegramBotBuilder.AddTelegramPartCreator(new TelegramPaymentMethod(option));
}

public class PaymentOption
{
    public string Token { get; set; }
}
