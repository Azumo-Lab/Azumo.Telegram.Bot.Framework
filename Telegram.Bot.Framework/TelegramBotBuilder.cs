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
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Config;
using Telegram.Bot.Framework.Abstract.Languages;
using Telegram.Bot.Framework.Exceptions;
using Telegram.Bot.Framework.ExtensionMethods;
using Telegram.Bot.Framework.InternalImplementation.Bots;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 机器人创建配置类
    /// </summary>
    public class TelegramBotBuilder : IBuilder
    {
        /// <summary>
        /// Telegram机器人的Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 代理设置
        /// </summary>
        public HttpClient Proxy { get; set; }

        /// <summary>
        /// 创建用
        /// </summary>
        public IServiceProvider BuilderServices { get; }

        /// <summary>
        /// 运行用
        /// </summary>
        public IServiceCollection RuntimeServices { get; }

        /// <summary>
        /// 用于创建本类的实例
        /// </summary>
        /// <returns><see cref="IBuilder"/> 机器人创建接口 </returns>
        public static IBuilder Create()
        {
            return new TelegramBotBuilder().AddConfig(serviceCollection =>
            {
                // 使用
                serviceCollection.UseDependencyInjectionAttribute();
            });
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <remarks>
        /// 这里使用了私有的初始化方法，目的是为了要用户调用 <see cref="Create"/> 方法
        /// </remarks>
        private TelegramBotBuilder()
        {
            BuilderServices = new ServiceCollection()
                .AddSingleton<IServiceCollection>()
                .AddSingleton<IConfig, TGConf>()
                .AddSingleton<ITelegramBot, TelegramBot>()
                .AddSingleton<IBotInfo, BotInfo>()
                .BuildServiceProvider();

            RuntimeServices = BuilderServices.GetRequiredService<IServiceCollection>();
        }

        /// <summary>
        /// 开始创建TelegramBot
        /// </summary>
        /// <returns>返回 <see cref="ITelegramBot"/> 机器人接口</returns>
        public ITelegramBot Build()
        {
            Token.ThrowIfNullOrEmpty();

            _ = RuntimeServices.AddSingleton<ITelegramBotClient, TelegramBotClient>(x =>
            {
                return new TelegramBotClient(Token, Proxy);
            });

            // 返回创建的 ITelegramBot 实例
            return BuilderServices.GetRequiredService<ITelegramBot>();
        }
    }

    /// <summary>
    /// 设置扩展类，用于扩展 <see cref="IBuilder"/> 的内容
    /// </summary>
    public static class BuilerSetup
    {
        /// <summary>
        /// 用于检查重复Token
        /// </summary>
        private static readonly HashSet<string> __HashTokens = new();

        /// <summary>
        /// 添加Token，如果添加了相同的Token，则会抛出异常 <see cref="TheSameTokenException"/>
        /// </summary>
        /// <remarks>
        /// Token 是类似这样的字符串：<br/>
        /// <c>5298058194:AAFa9N1GiF_i7W0fV4aWgz22IGv8kzVZ13Q</c><br/>
        /// 其中，<c>5298058194</c> 的部分，是机器人的User ID
        /// <para>
        /// Token可以通过 BotFather 来进行获取
        /// </para>
        /// <para>
        /// <see cref="https://t.me/BotFather"/>
        /// </para>
        /// </remarks>
        /// <param name="builder">机器人创建接口</param>
        /// <param name="token">机器人的Token</param>
        /// <returns><see cref="IBuilder"/> 设置完Token的机器人创建接口</returns>
        /// <exception cref="TheSameTokenException"></exception>
        public static IBuilder AddToken(this IBuilder builder, string token)
        {
            builder.ThrowIfNull();
            token.ThrowIfNullOrEmpty();

            if (!__HashTokens.Add(token))
                throw new TheSameTokenException($"Token : {token}");

            builder.Token = token;
            return builder;
        }

        /// <summary>
        /// 添加网络代理
        /// </summary>
        /// <remarks>
        /// 网络代理可以使用 <see cref="http://127.0.0.1:7890/"/> 这样的URL形式来进行设置，也可以单独进行设置：<br/>
        /// <paramref name="host"/> : 127.0.0.1 <br/>
        /// <paramref name="port"/> : 7890 <br/>
        /// <paramref name="username"/> : 用户名称  <br/>
        /// <paramref name="password"/> : 用户密码  <br/>
        /// </remarks>
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
                Uri uri = new(host);
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
        /// 添加Clash的默认代理设置
        /// </summary>
        /// <remarks>
        /// 如果你的Clash是使用默认设置的话，可以使用这个方法，这个方法将会设置代理为 <see cref="http://127.0.0.1:7890"/>
        /// </remarks>
        /// <param name="builder">机器人创建接口</param>
        /// <returns><see cref="IBuilder"/> 设置代理后的创建接口</returns>
        public static IBuilder AddDefaultClash(this IBuilder builder)
        {
            builder.ThrowIfNull();

            _ = builder.AddProxy("http://127.0.0.1:7890");
            return builder;
        }

        /// <summary>
        /// 添加要处理的消息
        /// </summary>
        /// <remarks>
        /// 如果 <see cref="ReceiverOptions.AllowedUpdates"/> 为空的话，则会处理全部的消息类型
        /// </remarks>
        /// <param name="builder">机器人创建接口</param>
        /// <param name="receiverOptions">要处理的消息类型</param>
        /// <returns><see cref="IBuilder"/> 设置处理消息后的创建接口</returns>
        public static IBuilder AddReceiverOptions(this IBuilder builder, ReceiverOptions receiverOptions)
        {
            receiverOptions.ThrowIfNull();

            _ = builder.RuntimeServices.AddSingleton(receiverOptions);
            return builder;
        }

        /// <summary>
        /// 添加配置类
        /// </summary>
        /// <remarks>
        /// 添加配置类，其中配置类要实现 <see cref="IConfig"/> 接口
        /// </remarks>
        /// <typeparam name="T">类型是 <see cref="IConfig"/></typeparam>
        /// <param name="builder">机器人创建接口</param>
        /// <returns><see cref="IBuilder"/> 添加配置类后的创建接口</returns>
        public static IBuilder AddConfig<T>(this IBuilder builder) where T : class, IConfig
        {
            builder.ThrowIfNull();

            _ = builder.RuntimeServices.AddSingleton<IConfig, T>();
            return builder;
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <remarks>
        /// 添加配置，实现 <see cref="Action"/> 来进行框架的配置
        /// </remarks>
        /// <param name="builder">机器人创建接口</param>
        /// <param name="setting">用于配置的 <see cref="Action"/> 委托</param>
        /// <returns><see cref="IBuilder"/> 添加配置后的创建接口</returns>
        public static IBuilder AddConfig(this IBuilder builder, Action<IServiceCollection> setting)
        {
            setting.ThrowIfNull();

            setting.Invoke(builder.RuntimeServices);
            return builder;
        }

        /// <summary>
        /// 添加语言
        /// </summary>
        /// <typeparam name="T">类型是 <see cref="ILanguage"/></typeparam>
        /// <param name="builder">机器人创建接口</param>
        /// <returns><see cref="IBuilder"/> 添加语言后的创建接口</returns>
        public static IBuilder AddLanguage<T>(this IBuilder builder) where T : class, ILanguage
        {
            _ = builder.RuntimeServices.AddSingleton<ILanguage, T>();
            return builder;
        }

        /// <summary>
        /// 添加一个Bot的名称
        /// </summary>
        /// <param name="builder">机器人创建接口</param>
        /// <param name="botName">机器人的名称</param>
        /// <returns><see cref="IBuilder"/> 添加机器人名称后的创建接口</returns>
        public static IBuilder AddBotName(this IBuilder builder, string botName)
        {
            IBotInfo botInfo = builder.BuilderServices.GetService<IBotInfo>();
            botInfo.BotName = botName;
            builder.RuntimeServices.TryAddSingleton(botInfo);
            
            return builder;
        }
    }
}
