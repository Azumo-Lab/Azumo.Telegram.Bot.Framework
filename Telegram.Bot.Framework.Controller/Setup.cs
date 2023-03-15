using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Controller.Interface;
using Telegram.Bot.Framework.Controller.Internal;

namespace Telegram.Bot.Framework.Controller
{
    public static class Setup
    {
        public static IBuilder UseController(this IBuilder services)
        {
            services.RuntimeServices.AddScoped<IFrameworkInfo, FrameworkInfo>();
            return services;
        }
    }
}
