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

            string uri = port.HasValue ? $"{proxyHost}:{port}" : proxyHost;
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
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="telegramBotBuilder"></param>
        /// <param name="proxyHost"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ITelegramBotBuilder AddProxy(this ITelegramBotBuilder telegramBotBuilder, string proxyHost, int? port = null, string username = null, string password = null)
        {
            return telegramBotBuilder.AddTelegramPartCreator(new TelegramProxy(proxyHost, port, username, password));
        }

        /// <summary>
        /// 添加默认的Clash的代理地址
        /// </summary>
        /// <remarks>
        /// Clash 默认的代理地址是<br></br>
        /// <code>
        /// 127.0.0.1:7890
        /// </code>
        /// 如果已经更改地址端口等默认信息<br/>
        /// 请使用 <see cref="AddProxy(ITelegramBotBuilder, string, int?, string, string)"/>
        /// </remarks>
        /// <param name="telegramBotBuilder"></param>
        /// <returns></returns>
        public static ITelegramBotBuilder AddClashDefaultProxy(this ITelegramBotBuilder telegramBotBuilder)
        {
            return AddProxy(telegramBotBuilder, "localhost", 7890);
        }
    }
}
