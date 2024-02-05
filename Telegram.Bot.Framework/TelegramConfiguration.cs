using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.BotBuilder;

namespace Telegram.Bot.Framework;
internal class TelegramConfiguration<SettingModel>(string path) : ITelegramModule where SettingModel : class
{
    private readonly string ConfigPath = path;

    private readonly Type type = typeof(SettingModel);

    public void AddBuildService(IServiceCollection services) => 
        services.AddSingleton<SettingModel>();
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {
        services.AddSingleton(builderService.GetRequiredService<SettingModel>());

        if (!File.Exists(ConfigPath))
            throw new FileNotFoundException(ConfigPath);

        services.TryAddSingleton<IConfiguration>(new ConfigurationBuilder().AddJsonFile(ConfigPath).Build());
    }
}

public static class TelegramConfigurationExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="SettingModel"></typeparam>
    /// <param name="builder"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static ITelegramModuleBuilder AddConfiguration<SettingModel>(this ITelegramModuleBuilder builder, string path) where SettingModel : class => 
        builder.AddModule(new TelegramConfiguration<SettingModel>(path));
}
