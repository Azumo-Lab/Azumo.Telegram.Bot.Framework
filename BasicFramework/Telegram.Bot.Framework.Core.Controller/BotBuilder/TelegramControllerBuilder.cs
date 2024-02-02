using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Framework.Core.Controller.CorePipeline;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Framework.Core.Controller.BotBuilder;

internal class TelegramControllerBuilder : ITelegramModule
{
    public void AddBuildService(IServiceCollection services)
    {

    }
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {
        services.AddSingleton<IUpdateHandler, UpdateHandle>();
    }
}

public static class TelegramControllerBuilderExtensions
{
    public static ITelegramModuleBuilder UseController(this ITelegramModuleBuilder builder) => 
        builder.AddModule(new TelegramControllerBuilder());
}
