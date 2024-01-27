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

namespace Telegram.Bot.Framework.Extension;

/// <summary>
/// 可扩展的内容设置
/// </summary>
/// <remarks>
/// 通过实现这个接口，来进行 <see cref="ITelegramBotBuilder"/> 接口的处理设置
/// </remarks>
public interface ITelegramPartCreator
{
    /// <summary>
    /// 添加创建时服务
    /// </summary>
    /// <remarks>
    /// 创建时服务，用于执行 <see cref="Build(IServiceCollection, IServiceProvider)"/> 方法时使用的服务。
    /// </remarks>
    /// <param name="services">服务集合</param>
    public void AddBuildService(IServiceCollection services);

    /// <summary>
    /// 创建运行时服务
    /// </summary>
    /// <remarks>
    /// 可以使用 <paramref name="builderService"/> 参数来获取创建时服务。
    /// 可以使用创建时服务来创建，添加，处理运行时服务
    /// </remarks>
    /// <param name="services">运行时服务集合</param>
    /// <param name="builderService">创建时服务</param>
    public void Build(IServiceCollection services, IServiceProvider builderService);
}
