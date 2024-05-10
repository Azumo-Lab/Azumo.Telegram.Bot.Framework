//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.Net;
using System.Net.Http;
using Telegram.Bot.Framework.Core.BotBuilder;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// Telegram 代理功能
    /// </summary>
    internal class TelegramProxy : ITelegramModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">代理地址</param>
        /// <param name="port">代理端口</param>
        /// <param name="username">代理认证用户名</param>
        /// <param name="password">代理认证密码</param>
        public TelegramProxy(string host, int? port, string? username, string? password)
        {
            this.host = host;
            this.port = port;
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// 代理地址
        /// </summary>
        private readonly string host;

        /// <summary>
        /// 代理端口
        /// </summary>
        private readonly int? port;

        /// <summary>
        /// 代理用户名
        /// </summary>
        private readonly string? username;

        /// <summary>
        /// 代理密码
        /// </summary>
        private readonly string? password;

        /// <summary>
        /// 添加创建时期服务
        /// </summary>
        /// <remarks>
        /// 创建时期，创建一个代理
        /// </remarks>
        /// <param name="services">服务集合</param>
        public void AddBuildService(IServiceCollection services)
        {
            var webProxy = port == null ? new WebProxy(host) : new WebProxy(host, port.Value);
            if (!string.IsNullOrEmpty(username))
                webProxy.Credentials = new NetworkCredential(username, password);

            var httpClient = new HttpClient(new HttpClientHandler() { Proxy = webProxy, UseProxy = true });

            _ = services.AddSingleton(webProxy);
            _ = services.AddSingleton(httpClient);
        }

        /// <summary>
        /// 空方法
        /// </summary>
        /// <param name="services">运行时期的服务集合</param>
        /// <param name="builderService">创建时期的服务提供</param>
        public void Build(IServiceCollection services, IServiceProvider builderService)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static partial class TelegramModuleExtensions
    {
        /// <summary>
        /// 使用代理
        /// </summary>
        /// <param name="builder">模块构建器</param>
        /// <param name="host">代理地址</param>
        /// <param name="port">端口</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>模块构建器</returns>
        public static ITelegramModuleBuilder UseProxy(this ITelegramModuleBuilder builder, string host, int port, string? username = null, string? password = null) =>
            builder.AddModule(new TelegramProxy(host, port, username, password));

        /// <summary>
        /// 使用 Clash 默认的代理端口
        /// </summary>
        /// <param name="builder">模块构建器</param>
        /// <returns>模块构建器</returns>
        public static ITelegramModuleBuilder UseClashDefaultProxy(this ITelegramModuleBuilder builder) =>
            builder.UseProxy("127.0.0.1", 7890);
    }
}
