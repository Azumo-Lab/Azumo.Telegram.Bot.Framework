using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.FrameworkHelper;

namespace Telegram.Bot.Net
{
    class Program : ISetUp
    {
        static void Main(string[] args)
        {
            var Secrets = new ConfigurationBuilder().AddUserSecrets("98def42c-77dc-41cb-abf6-2c402535f4cb").Build();

            string Token = Secrets.GetSection("Token").Value;
            string Proxy = Secrets.GetSection("Proxy").Value;
            int Port = int.Parse(Secrets.GetSection("Port").Value);

            var bot = TelegramBotManger.Config()
                .SetToken(Token)
                .Proxy(Proxy, Port)
                .SetUp(new Program())
                .Build();

            bot.Start();
            
            //TestHelper.Test();

        }

        public void Config(IServiceCollection telegramServices)
        {
            //throw new NotImplementedException();
        }
    }
}
