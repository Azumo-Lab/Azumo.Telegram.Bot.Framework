//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Net;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.InternalImpl.Bots
{
    /// <summary>
    /// 添加代理设置
    /// </summary>
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

        public static ITelegramBotBuilder AddClashDefaultProxy(this ITelegramBotBuilder telegramBotBuilder)
        {
            return AddProxy(telegramBotBuilder, "127.0.0.1", 7890);
        }
    }
}
