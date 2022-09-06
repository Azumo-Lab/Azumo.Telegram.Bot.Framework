# Telegram.Bot.Framework

https://github.com/sokushu/Telegram.Bot.Net/wiki

### 简单介绍

[更多帮助和使用指南，请访问WIKI](https://github.com/sokushu/Telegram.Bot.Net/wiki)

这是一个可以帮助减少Telegram Bot开发难度的框架，如果你会使用ASP.NET，那么你也可以轻松上手这个框架。

### 快速开始

```csharp
[BotName("DEV1")]
public class Commands : TelegramController
{
    private readonly IServiceProvider serviceProvider;
    public Commands(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    // 使用指令/Test
    [Command("Test")]
    public async Task Test([Param("请输入要说的话：", true)] string Message)
    {
        await SendTextMessage("你好，你要说的话是：");
        await SendTextMessage(Message);
    }
}
```

```csharp
public class TGBotDEV : IConfig
{
    static void Main(string[] args)
    {
        var Secrets = new ConfigurationBuilder().AddUserSecrets("98def42c-77dc-41cb-abf6-2c402535f4cb").Build();

        string Token = Secrets.GetSection("Token").Value;
        string Proxy = Secrets.GetSection("Proxy").Value;
        int Port = int.Parse(Secrets.GetSection("Port").Value);

        var bot = TelegramBotManger.CreateConfig()
            .SetToken(Token)
            .SetProxy(Proxy, Port)
            .SetConfig<TGBotDEV>()
            .SetBotName("DEV1")
            .Build();

        bot.Start();
    }

    public void Config(IServiceCollection telegramServices)
    {
        // 配置各类服务。
        // TODO Something...
    }
}
```
