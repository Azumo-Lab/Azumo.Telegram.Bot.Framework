using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Authentication.Interface;

namespace Telegram.Bot.Framework.Authentication
{
    public static class Setup
    {
        public static void AddAuthenticationRole<RoleType>(this IBuilder builder) where RoleType : class, IAuthenticationRole
        {
            builder.RuntimeServices.AddScoped<IAuthenticationRole, RoleType>();
        }
    }
}
