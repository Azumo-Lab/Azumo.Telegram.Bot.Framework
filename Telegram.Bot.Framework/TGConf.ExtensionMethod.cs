using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.MiddlewarePipelines;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 框架配置的一些扩展方法
    /// </summary>
    internal static class TGConf_ExtensionMethod
    {
        public static void AddMiddlewarePipeline(this IServiceCollection serviceDescriptors)
        {
            AddSingleton<IMiddlewarePipeline>(serviceDescriptors);
        }

        public static void AddMiddlewareTemplate(this IServiceCollection serviceDescriptors)
        {
            AddSingleton<IMiddlewareTemplate>(serviceDescriptors);
        }

        public static void AddDependencyInjection(this IServiceCollection serviceDescriptors)
        {

            Type objType = typeof(object);
            Type diAttr = typeof(DependencyInjectionAttribute);
            ObjectHelper.GetAllTypes()
                .Where(x => Attribute.IsDefined(x, diAttr))
                .ToList()
                .ForEach(x =>
                {
                    if (Attribute.GetCustomAttribute(x, diAttr) is not DependencyInjectionAttribute dependencyInjectionAttribute)
                        return;

                    Type baseType;
                    Type[] interFaceType;
                    Type serviceType = dependencyInjectionAttribute.ServiceType ??
                            (((baseType = x.BaseType).FullName == objType.FullName)
                            ? ((interFaceType = x.GetInterfaces()).Length > 1
                                ? throw new Exception()
                                : interFaceType.Length == 0 ? x : interFaceType[0])
                            : baseType);
                    switch (dependencyInjectionAttribute.ServiceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            serviceDescriptors.AddSingleton(serviceType, x);
                            break;
                        case ServiceLifetime.Scoped:
                            serviceDescriptors.AddScoped(serviceType, x);
                            break;
                        case ServiceLifetime.Transient:
                            serviceDescriptors.AddTransient(serviceType, x);
                            break;
                        default:
                            break;
                    }
                });
        }

        #region 将IServiceCollection 中的添加服务的方法扩展了

        private static void AddSingleton<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                serviceDescriptors.AddSingleton(baseType, implType);
            });
        }

        private static void AddScoped<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                serviceDescriptors.AddScoped(baseType, implType);
            });
        }

        private static void AddTransient<T>(IServiceCollection serviceDescriptors)
        {
            AddTemplate<T>(serviceDescriptors, (serviceDescriptors, baseType, implType) =>
            {
                serviceDescriptors.AddTransient(baseType, implType);
            });
        }

        private static void AddTemplate<T>(IServiceCollection serviceDescriptors, Action<IServiceCollection, Type, Type> action)
        {
            Type baseType = typeof(T);
            foreach (Type item in ObjectHelper.GetSameType(baseType))
            {
                action.Invoke(serviceDescriptors, baseType, item);
            }
        }

        #endregion
    }
}
