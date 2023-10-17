using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Helpers;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.InternalImpl.Bots
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramBasic : ITelegramPartCreator
    {
        public void AddBuildService(IServiceCollection services)
        {
            
        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {

            ReflectionHelper.AllTypes.Where(x => Attribute.IsDefined(x, typeof(DependencyInjectionAttribute)))
                .Select(x => (x, (DependencyInjectionAttribute)Attribute.GetCustomAttribute(x, typeof(DependencyInjectionAttribute))))
                .ToList()
                .ForEach((x) =>
                {
                    switch (x.Item2.ServiceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            _ = services.AddSingleton(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        case ServiceLifetime.Scoped:
                            _ = services.AddScoped(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        case ServiceLifetime.Transient:
                            _ = services.AddTransient(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        default:
                            break;
                    }
                });

            InternalInstall.StartInstall();

            services.AddLogging(option =>
            {
                option.AddConsole();
                option.AddSimpleConsole();
            });
            services.AddSingleton<ITelegramBot, TelegramBot>();
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        public static ITelegramBotBuilder AddBasic(this ITelegramBotBuilder builder)
        {
            return builder.AddTelegramPartCreator(new TelegramBasic());
        }
    }
}
