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
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Storage;

namespace Telegram.Bot.Framework.Core.Controller.Controller;

/// <summary>
/// 
/// </summary>
/// <param name="serviceProvider"></param>
[DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(ICommandScopeService))]
internal class CommandScopeService(IServiceProvider serviceProvider) : ICommandScopeService
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IServiceProvider UserScopeServiceProvider = serviceProvider;

    /// <summary>
    /// 
    /// </summary>
    private IServiceScope? _serviceScope;

    /// <summary>
    /// 
    /// </summary>
    public IServiceProvider? Service => _serviceScope?.ServiceProvider;

    public ISession? Session { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public void Create()
    {
        _serviceScope = UserScopeServiceProvider.CreateScope();
        Session = Service!.GetRequiredService<ISession>();
    }
    /// <summary>
    /// 
    /// </summary>
    public void Delete()
    {
        _serviceScope?.Dispose();
        _serviceScope = null;
    }

    public void DeleteOldCreateNew()
    {
        Delete();
        Create();
    }
}
