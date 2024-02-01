using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Core.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependencyInjectionAttribute(ServiceLifetime serviceLifetime) : Attribute
{
    /// <summary>
    /// 
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Type? ServiceType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ServiceLifetime Lifetime { get; } = serviceLifetime;
}
