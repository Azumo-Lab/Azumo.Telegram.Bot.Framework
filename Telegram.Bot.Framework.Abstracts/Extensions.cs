using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Telegram.Bot.Framework.Abstracts
{
    public static class Extensions
    {
        #region 这里是对 IServiceCollection 接口进行的扩展
        /// <summary>
        /// 对所有的服务进行替换
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="serviceDescriptors"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection ReplaceAll<TService, TImplementation>(this IServiceCollection serviceDescriptors, ServiceLifetime serviceLifetime)
            where TService : class
            where TImplementation : class, TService
        {
            _ = serviceDescriptors.RemoveAll<TService>();
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    serviceDescriptors.TryAddSingleton<TService, TImplementation>();
                    break;
                case ServiceLifetime.Scoped:
                    serviceDescriptors.TryAddScoped<TService, TImplementation>();
                    break;
                case ServiceLifetime.Transient:
                    serviceDescriptors.TryAddTransient<TService, TImplementation>();
                    break;
                default:
                    throw new ArgumentException($"Not Found {nameof(serviceLifetime)} Value, Value : {(int)serviceLifetime}");
            }
            return serviceDescriptors;
        }
        #endregion
    }
}
