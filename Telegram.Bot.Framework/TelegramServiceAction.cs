using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.BotBuilder;

namespace Telegram.Bot.Framework;
internal class TelegramServiceAction(Action<IServiceCollection> action) : ITelegramModule
{
    public void AddBuildService(IServiceCollection services)
    {

    }
    public void Build(IServiceCollection services, IServiceProvider builderService) => 
        action(services);
}

public static class TelegramServiceActionExtensions
{
    public static ITelegramModuleBuilder AddServiceAction(this ITelegramModuleBuilder builder, Action<IServiceCollection> action) =>
        builder.AddModule(new TelegramServiceAction(action));
}