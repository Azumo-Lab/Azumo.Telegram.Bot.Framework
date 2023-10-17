using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.InternalImpl.Bots
{
    internal class TelegramToken : ITelegramPartCreator
    {
        private readonly string __Token;
        public TelegramToken(string token)
        {
            __Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public void AddBuildService(IServiceCollection services)
        {

        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            HttpClient proxy;
            if ((proxy = builderService.GetService<HttpClient>()) != null)
                _ = services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(__Token, proxy));
            else
                _ = services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(__Token));
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        public static ITelegramBotBuilder AddToken(this ITelegramBotBuilder builder, string token)
        {
            return builder.AddTelegramPartCreator(new TelegramToken(token));
        }
    }
}
