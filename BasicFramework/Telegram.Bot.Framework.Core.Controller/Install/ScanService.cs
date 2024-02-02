using Azumo.ReflectionEnhancementPack;
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
        var list = typeof(DependencyInjectionAttribute).GetHasAttributeType();
        foreach ((var t, var attr) in list)
        {
            var attrDep = (DependencyInjectionAttribute)attr;

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
