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

using Azumo.SuperExtendedFramework;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.BotBuilder;

namespace Telegram.Bot.Framework.Core.Controller.Install;

/// <summary>
/// 
/// </summary>
internal class ScanService : ITelegramModule
{
    public void AddBuildService(IServiceCollection services)
    {

    }
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {
        var list = Extensions.GetTypesWithAttribute<DependencyInjectionAttribute>();
        foreach ((var t, var attr) in list)
        {
            foreach (var item in attr)
            {
                var attrDep = (DependencyInjectionAttribute)item;

                var keyType = attrDep.ServiceType;
                if (keyType == null)
                {
                    Type[] interfaceTypes;
                    Type? basetype;
                    keyType = (interfaceTypes = t.GetInterfaces()).Length == 1
                        ? interfaceTypes.First()
                        : (basetype = t.BaseType) != null && basetype.IsAbstract ? basetype : t;
                }

                if (!string.IsNullOrEmpty(attrDep.Key))
                    switch (attrDep.Lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            _ = services.AddKeyedSingleton(keyType, attrDep.Key, t);
                            break;
                        case ServiceLifetime.Scoped:
                            _ = services.AddKeyedScoped(keyType, attrDep.Key, t);
                            break;
                        case ServiceLifetime.Transient:
                            _ = services.AddKeyedTransient(keyType, attrDep.Key, t);
                            break;
                        default:
                            break;
                    }
                else
                    switch (attrDep.Lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            _ = services.AddSingleton(keyType, t);
                            break;
                        case ServiceLifetime.Scoped:
                            _ = services.AddScoped(keyType, t);
                            break;
                        case ServiceLifetime.Transient:
                            _ = services.AddTransient(keyType, t);
                            break;
                        default:
                            break;
                    }
            }
        }
    }
}
