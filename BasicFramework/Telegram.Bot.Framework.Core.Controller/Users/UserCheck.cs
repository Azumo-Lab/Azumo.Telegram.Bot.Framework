using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller.Filters;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Controller.Users;

/// <summary>
/// 
/// </summary>
[DependencyInjection(ServiceLifetime.Singleton, ServiceType = typeof(IUpdateFilter))]
internal class UserCheck : IUpdateFilter
{
    public bool Filter(Update update) => 
        update.GetUser() != null;
}
