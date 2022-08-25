# Telegram.Bot.Net

https://github.com/sokushu/Telegram.Bot.Net/wiki

### 简单介绍

这是一个可以帮助减少Telegram Bot开发难度的框架，如果你会使用ASP.NET，那么你也可以轻松上手这个框架。

### 快速开始

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Net.Example
{
    public class Commands : TelegramController
    {
        private readonly IServiceProvider serviceProvider;
        public Commands(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [Command("Test")]
        public async Task Test()
        {
            await SendTextMessage("你好");
            await SendTextMessage("打开百度", new List<InlineKeyboardButton> { InlineKeyboardButton.WithUrl("百度一下", "https://www.baidu.com/") });
        }
    }
}
```

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework;

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
        }

        public void Config(IServiceCollection telegramServices)
        {
            //实现
        }
    }
```

[更多帮助和使用指南，请访问WIKI](https://github.com/sokushu/Telegram.Bot.Net/wiki)
