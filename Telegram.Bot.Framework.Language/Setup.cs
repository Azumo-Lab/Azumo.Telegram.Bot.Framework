using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Languages;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.Language
{
    public static class Setup
    {
        public static IBuilder AddMultiLanguage(this IBuilder builder)
        {
            builder.RuntimeServices.TryAddSingleton<IMultiLanguage, MultiLanguage>();
            List<Type> languageTypes = typeof(ILanguage).GetSameType();
            foreach (Type languageType in languageTypes)
                builder.RuntimeServices.TryAddScoped(typeof(ILanguage), languageType);
            return builder;
        }
    }
}
