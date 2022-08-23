using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.DependencyInjection.Extends
{
    public static class ServiceProviderExtends
    {
        public static T GetService<T>(this IServiceProvider serviceProvider, Type serviceType)
        {
            return (T)serviceProvider.GetService(serviceType);
        }
    }
}
