using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.Bots
{
    internal class TelegramConfiguration(string settingPath) : ITelegramPartCreator
    {
        private readonly string __SettingPath = settingPath;

        public void AddBuildService(IServiceCollection services)
        {
            _ = services.RemoveAll<IConfiguration>();
            services.TryAddSingleton<IConfiguration>(new ConfigurationBuilder().AddJsonFile(__SettingPath).Build());
        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            _ = services.AddSingleton(builderService.GetRequiredService<IConfiguration>());
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        public static ITelegramBotBuilder AddConfiguration(this ITelegramBotBuilder builder, string appSettingPath)
        {
            return builder.AddTelegramPartCreator(new TelegramConfiguration(appSettingPath));
        }
    }
}
