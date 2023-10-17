using System.Net;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.InternalImpl.Bots
{
    internal class TelegramProxy : ITelegramPartCreator
    {
        private readonly HttpClient __HttpClient;
        public TelegramProxy(string proxyHost, int? port = null, string username = null, string password = null)
        {
            if (proxyHost == null) throw new ArgumentNullException(nameof(proxyHost));

            Uri uri = port.HasValue ? new Uri($"{proxyHost}:{port}") : new Uri(proxyHost);
            WebProxy webProxy = new(uri);
            if (!string.IsNullOrEmpty(username))
                webProxy.Credentials = new NetworkCredential(username, password);

            __HttpClient = new(
                new HttpClientHandler { Proxy = webProxy, UseProxy = true, }
            );
        }

        public void AddBuildService(IServiceCollection services)
        {
            _ = services.AddSingleton(__HttpClient);
        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {

        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        public static ITelegramBotBuilder AddProxy(this ITelegramBotBuilder telegramBotBuilder, string proxyHost, int? port = null, string username = null, string password = null)
        {
            return telegramBotBuilder.AddTelegramPartCreator(new TelegramProxy(proxyHost, port, username, password));
        }
    }
}
