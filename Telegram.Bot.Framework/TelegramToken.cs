using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.BotBuilder;

namespace Telegram.Bot.Framework;
internal class TelegramToken(string token) : ITelegramModule
{
    protected string Token { get; set; } = token;
    public virtual void AddBuildService(IServiceCollection services)
    {

    }
    public virtual void Build(IServiceCollection services, IServiceProvider builderService)
    {
        
    }
}

internal class TelegramToken<SettingModel>(Func<SettingModel, string> tokenFunc) 
    : TelegramToken(string.Empty) where SettingModel : class
{
    private readonly Type type = typeof(SettingModel);

    public override void AddBuildService(IServiceCollection services)
    {
        services.AddSingleton<SettingModel>();
        base.AddBuildService(services);
    }

    public override void Build(IServiceCollection services, IServiceProvider builderService)
    {
        var settingModel = builderService.GetService<SettingModel>();
        if (settingModel == null)
            throw new NullReferenceException(nameof(settingModel));

        Token = tokenFunc(settingModel);

        base.Build(services, builderService);
    }
}

public static class TelegramTokenExtensions
{
    public static ITelegramModuleBuilder UseToken<SettingModel>(this ITelegramModuleBuilder builder, Func<SettingModel, string> tokenFunc) => 
        builder.AddModule(new TelegramToken<SettingModel>(tokenFunc));
}
