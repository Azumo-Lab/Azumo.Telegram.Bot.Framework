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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Config;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.Bots
{
    /// <summary>
    /// 
    /// </summary>
    public class TelegramBotBuilder : IBuilder
    {
        public string Token { get; set; }

        public HttpClient Proxy { get; set; }

        public List<Type> Configs { get; set; }

        public IServiceCollection BuilderServices { get; } = new ServiceCollection();
        public IServiceCollection RuntimeServices { get; } = new ServiceCollection();

        public static IBuilder Create()
        {
            return new TelegramBotBuilder();
        }

        private TelegramBotBuilder() 
        {
            
        }

        public ITelegramBot Build()
        {
            Token.ThrowIfNullOrEmpty();
            Configs.ThrowIfNullOrEmpty();

            BuilderServices.AddSingleton<IConfig, FrameworkConfig>();
            BuilderServices.AddSingleton(RuntimeServices);
            BuilderServices.AddSingleton<ITelegramBot, TelegramBot>();

            IServiceProvider serviceProvider = BuilderServices.BuildServiceProvider();

            return serviceProvider.GetRequiredService<ITelegramBot>();
        }
    }

    public static class Setup
    {
        public static IBuilder AddToken(this IBuilder builder, string token)
        {
            builder.ThrowIfNull();
            token.ThrowIfNullOrEmpty();

            builder.Token = token;
            return builder;
        }

        public static IBuilder AddProxy(this IBuilder builder, string host, int? port = null, string username = null, string password = null)
        {
            builder.ThrowIfNull();
            host.ThrowIfNullOrEmpty();

            WebProxy webProxy;
            if (port.IsNull())
            {
                Uri uri = new Uri(host);
                webProxy = new(uri.Host, uri.Port);
            }
            else
            {
                webProxy = new(Host: host, Port: port.Value);
            }
            if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
            {
                webProxy.Credentials = new NetworkCredential(username, password);
            }
            builder.Proxy = new HttpClient(
                new HttpClientHandler { Proxy = webProxy, UseProxy = true, }
            );

            return builder;

        }
    }
}
