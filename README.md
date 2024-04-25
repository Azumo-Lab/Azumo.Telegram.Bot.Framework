# Telegram.Bot.Framework

### 简单介绍

[更多帮助和使用指南，请访问WIKI](https://github.com/sokushu/Telegram.Bot.Net/wiki)

这是一个可以帮助减少Telegram Bot开发难度的框架，仿照了ASP.NET的方式，如果你会一点ASP.NET，那么你也可以轻松上手这个框架。

### 快速开始

* 控制器的编写：

```csharp
[TelegramController]
public class TestController
{
    [BotCommand("/Hello", Description = "Say Hello")]
    public static async Task Hello(TelegramUserContext userContext)
    {
        await userContext.BotClient.SendTextMessageAsync(userContext.ScopeChatID, "Hello");
    }
}
```

* 执行入口：

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        ITelegramBot telegramBot = TelegramBotBuilder.Create()
                .AddToken("<Your Token>")
                .AddProxy("http://127.0.0.1:7890")
                .UseController()
                .Build();

        Task botTask = telegramBot.BotStart();
        botTask.Wait();
    }
}
```