using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.Controller;

internal class UseController : ITelegramPartCreator
{
    public void AddBuildService(IServiceCollection services)
    {

    }
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {

    }
}

public static class UseControllerEx
{
    public static ITelegramBotBuilder UseController(this ITelegramBotBuilder telegramBotBuilder) => 
        telegramBotBuilder.AddTelegramPartCreator(new UseController());
}
