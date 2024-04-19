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
using System.Net;
using Telegram.Bot.Framework.Core.BotBuilder;

namespace Telegram.Bot.Framework;

/// <summary>
/// Telegram 代理功能
/// </summary>
/// <param name="host">代理地址</param>
/// <param name="port">代理端口</param>
/// <param name="username">代理认证用户名</param>
/// <param name="password">代理认证密码</param>
internal class TelegramProxy(string host, int? port, string? username, string? password) : ITelegramModule
{
    /// <summary>
    /// 代理地址
    /// </summary>
    private readonly string host = host;

    /// <summary>
    /// 代理端口
    /// </summary>
    private readonly int? port = port;

    /// <summary>
    /// 代理用户名
    /// </summary>
    private readonly string? username = username;

    /// <summary>
    /// 代理密码
    /// </summary>
    private readonly string? password = password;

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
public static class TelegramProxyExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static ITelegramModuleBuilder UseProxy(this ITelegramModuleBuilder builder, string host, int port, string? username = null, string? password = null) =>
        builder.AddModule(new TelegramProxy(host, port, username, password));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ITelegramModuleBuilder UseClashDefaultProxy(this ITelegramModuleBuilder builder) =>
        builder.UseProxy("127.0.0.1", 7890);
}
