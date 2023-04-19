using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class DependencyInjectionAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; }

        public Type ServiceType { get; set; }

        public int Priority { get; set; }

        public DependencyInjectionAttribute(ServiceLifetime serviceLifetime)
        { 
            ServiceLifetime = serviceLifetime;
        }
    }
}
