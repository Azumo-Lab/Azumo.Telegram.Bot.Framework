# Telegram.Bot.Net

[更多帮助和使用指南，请访问WIKI](https://github.com/sokushu/Telegram.Bot.Net/wiki)

### 简单介绍

这是一个可以帮助减少Telegram Bot开发难度的框架，如果你会使用ASP.NET，那么你也可以轻松上手这个框架。

### 快速开始

```csharp
public class Commands : TelegramController
{
    private readonly IServiceProvider serviceProvider;
    public Commands(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    [Command("Test")]
    public async Task Test([Param("请输入要说的话：", true)] string Message)
    {
        await SendTextMessage("你好，你要说的话是：");
        await SendTextMessage(Message);
    }

    [Command("Test2")]
    public async Task Test([Param("请输入第一句话：", true)] string FirstMessage, [Param("请输入第二句话：", true)] string TwoMessage)
    {
        await SendTextMessage($"你说的第一句是：{FirstMessage}");
        await SendTextMessage($"你说的第二句是：{TwoMessage}");
        await SendTextMessage($"合起来是：{FirstMessage}，{TwoMessage}。");
    }

    [Command("SayHello")]
    public async Task SayHello()
    {
        await SendTextMessage("Hello World");
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

        Task botTask = bot.Start();

        botTask.Wait();
    }

    public void Config(IServiceCollection telegramServices)
    {
        // TODO Something...
    }
}
```
