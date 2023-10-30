using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework
{
    public static class IServiceCollection_ExtensionMethods
    {
        public static IServiceCollection ScanTGService(this IServiceCollection services)
        {
            ReflectionHelper.AllTypes.Where(x => Attribute.IsDefined(x, typeof(DependencyInjectionAttribute)))
                .Select(x => (x, (DependencyInjectionAttribute)Attribute.GetCustomAttribute(x, typeof(DependencyInjectionAttribute))))
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
