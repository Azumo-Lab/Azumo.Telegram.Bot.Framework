using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependencyInjectionAttribute : Attribute
{
    public string? Key { get; set; }

    public Type? ServiceType { get; set; }
}
