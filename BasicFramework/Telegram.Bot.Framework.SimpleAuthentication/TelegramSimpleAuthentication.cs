using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.BotBuilder;

namespace Telegram.Bot.Framework.SimpleAuthentication;
internal class TelegramSimpleAuthentication : ITelegramModule
{
    public void AddBuildService(IServiceCollection services)
    {

    }
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {

    }
}

public static class TelegramSimpleAuthenticationExtensions
{
    public static ITelegramModuleBuilder UseSimpleAuthentication(this ITelegramModuleBuilder builder) => 
        builder.AddModule(new TelegramSimpleAuthentication());
}
