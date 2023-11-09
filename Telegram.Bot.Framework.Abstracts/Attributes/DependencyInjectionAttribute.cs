using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependencyInjectionAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; }

        public Type? ServiceType { get; }

        public DependencyInjectionAttribute(ServiceLifetime ServiceLifetime, Type ServiceType)
        {
            this.ServiceLifetime = ServiceLifetime;
            this.ServiceType = ServiceType;
        }

        public DependencyInjectionAttribute(ServiceLifetime ServiceLifetime)
        {
            this.ServiceLifetime = ServiceLifetime;
        }
    }
}
