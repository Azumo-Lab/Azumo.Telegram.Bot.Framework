using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Helpers;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public class TelegramBuilder : ITelegramBotBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection RuntimeService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> Arguments { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ITelegramBotBuilder Create()
        {
            return new TelegramBuilder();
        }

        /// <summary>
        /// 
        /// </summary>
        private TelegramBuilder()
        {
            RuntimeService = new ServiceCollection();
            Arguments = new Dictionary<string, object>();

            ReflectionHelper.AllTypes.Where(x => Attribute.IsDefined(x, typeof(DependencyInjectionAttribute)))
                .Select(x => (x, (DependencyInjectionAttribute)Attribute.GetCustomAttribute(x, typeof(DependencyInjectionAttribute))))
                .ToList()
                .ForEach((x) =>
                {
                    switch (x.Item2.ServiceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            RuntimeService.AddSingleton(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        case ServiceLifetime.Scoped:
                            RuntimeService.AddScoped(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        case ServiceLifetime.Transient:
                            RuntimeService.AddTransient(x.Item2.ServiceType ?? x.x, x.x);
                            break;
                        default:
                            break;
                    }
                });

            InternalInstall.StartInstall();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ITelegramBot Build()
        {
            string token = null;
            HttpClient proxy = null;
            if (Arguments.TryGetValue("TOKEN", out object tokenObj))
                token = tokenObj.ToString();
            if (Arguments.TryGetValue("PROXY", out object proxyObj))
                proxy = (HttpClient)proxyObj;

            if (token == null)
                throw new Exception($"{nameof(token)} = {token}");
            RuntimeService.AddSingleton<ITelegramBotClient>(new TelegramBotClient(token, proxy));

            RuntimeService.AddLogging();

            RuntimeService.AddSingleton<ITelegramBot, TelegramBot>();

            IServiceProvider serviceProvider = RuntimeService.BuildServiceProvider();
            return serviceProvider.GetService<ITelegramBot>();
        }
    }
}
