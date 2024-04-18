//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
/// 
/// </summary>
/// <param name="host"></param>
/// <param name="port"></param>
/// <param name="username"></param>
/// <param name="password"></param>
internal class TelegramProxy(string host, int? port, string? username, string? password) : ITelegramModule
{
    /// <summary>
    /// 
    /// </summary>
    private readonly string host = host;

    /// <summary>
    /// 
    /// </summary>
    private readonly int? port = port;

    /// <summary>
    /// 
    /// </summary>
    private readonly string? username = username;

    /// <summary>
    /// 
    /// </summary>
    private readonly string? password = password;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
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
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builderService"></param>
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {

    }
}

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
