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
using Telegram.Bot.Framework.Exceptions;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public class TelegramBotBuilder : IBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpClient Proxy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Type> Configs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection BuilderServices { get; } = new ServiceCollection();

        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection RuntimeServices { get; } = new ServiceCollection();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IBuilder Create()
        {
            return new TelegramBotBuilder();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private TelegramBotBuilder() { }

        /// <summary>
        /// 开始创建TelegramBot
        /// </summary>
        /// <returns>返回<see cref="ITelegramBot"/>接口</returns>
        public ITelegramBot Build()
        {
            Token.ThrowIfNullOrEmpty();

            BuilderServices.AddSingleton<IConfig, FrameworkConfig>();
            BuilderServices.AddSingleton(RuntimeServices);
            BuilderServices.AddSingleton<ITelegramBot, TelegramBot>();

            RuntimeServices.AddSingleton<ITelegramBotClient, TelegramBotClient>(x => 
            {
                return new TelegramBotClient(Token, Proxy);
            });

            IServiceProvider serviceProvider = BuilderServices.BuildServiceProvider();

            return serviceProvider.GetRequiredService<ITelegramBot>();
        }
    }

    /// <summary>
    /// 设置扩展类
    /// </summary>
    public static class Setup
    {
        /// <summary>
        /// 用于检查重复Token
        /// </summary>
        private static HashSet<string> HashTokens = new HashSet<string>();

        /// <summary>
        /// 添加Token
        /// </summary>
        /// <param name="builder">机器人创建接口</param>
        /// <param name="token">机器人的Token</param>
        /// <returns><see cref="IBuilder"/> 设置完Token的机器人创建接口</returns>
        public static IBuilder AddToken(this IBuilder builder, string token)
        {
            builder.ThrowIfNull();
            token.ThrowIfNullOrEmpty();

            if (!HashTokens.Add(token))
                throw new TheSameTokenException($"Token : {token}");

            builder.Token = token;
            return builder;
        }

        /// <summary>
        /// 添加网络代理
        /// </summary>
        /// <param name="builder">机器人创建接口</param>
        /// <param name="host">代理地址</param>
        /// <param name="port">代理端口</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns><see cref="IBuilder"/> 设置代理后的创建接口</returns>
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
                webProxy = new(Host: host, Port: port.Value);
            if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
                webProxy.Credentials = new NetworkCredential(username, password);
            builder.Proxy = new HttpClient(
                new HttpClientHandler { Proxy = webProxy, UseProxy = true, }
            );

            return builder;

        }

        /// <summary>
        /// 添加Clash的默认代理设置 'http://127.0.0.1:7890'
        /// </summary>
        /// <param name="builder">机器人创建接口</param>
        /// <returns><see cref="IBuilder"/> 设置代理后的创建接口</returns>
        public static IBuilder AddDefaultClash(this IBuilder builder)
        {
            builder.ThrowIfNull();

            builder.AddProxy("http://127.0.0.1:7890");
            return builder;
        }

        public static IBuilder AddReceiverOptions(this IBuilder builder, ReceiverOptions receiverOptions)
        {
            builder.RuntimeServices.AddSingleton(receiverOptions);
            return builder;
        }
    }
}
