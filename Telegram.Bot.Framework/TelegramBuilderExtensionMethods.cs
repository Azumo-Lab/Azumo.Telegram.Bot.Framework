using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework
{
    public static class TelegramBuilderExtensionMethods
    {
        public static ITelegramBotBuilder AddToken(this ITelegramBotBuilder builder, string token)
        {
            if (!builder.Arguments.TryAdd("TOKEN", token))
                throw new ArgumentException(token, nameof(token));
            return builder;
        }

        public static ITelegramBotBuilder AddProxy(this ITelegramBotBuilder builder, string proxyHost, int? port = null, string username = null, string password = null)
        {
            Uri uri;
            if (port.HasValue)
                uri = new Uri($"{proxyHost}:{port}");
            else
                uri = new Uri(proxyHost);

            WebProxy webProxy = new(uri);
            if (!string.IsNullOrEmpty(username))
                webProxy.Credentials = new NetworkCredential(username, password);

            HttpClient httpClient = new(
                new HttpClientHandler { Proxy = webProxy, UseProxy = true, }
            );

            if (!builder.Arguments.TryAdd("PROXY", httpClient))
                throw new ArgumentException(proxyHost, nameof(proxyHost));

            return builder;
        }
    }
}
