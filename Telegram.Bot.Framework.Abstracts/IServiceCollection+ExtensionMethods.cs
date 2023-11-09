using Azumo.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.InternalInterface;

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
