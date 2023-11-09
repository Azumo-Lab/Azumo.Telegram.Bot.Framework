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

using Azumo.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Abstracts.Attributes;

namespace Telegram.Bot.Framework.Abstracts
{
    internal static class IServiceCollection_ExtensionMethods
    {
        public static IServiceCollection ScanService(this IServiceCollection services)
        {
            AzReflectionHelper.GetAllTypes().Where(x => Attribute.IsDefined(x, typeof(DependencyInjectionAttribute)))
                .Select(x => (x, (DependencyInjectionAttribute)Attribute.GetCustomAttribute(x, typeof(DependencyInjectionAttribute))!))
                .ToList()
                .ForEach((x) =>
                {
                    switch (x.Item2.ServiceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            _ = services.AddSingleton(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        case ServiceLifetime.Scoped:
                            _ = services.AddScoped(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        case ServiceLifetime.Transient:
                            _ = services.AddTransient(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        default:
                            break;
                    }
                });
            return services;
        }
    }
}
