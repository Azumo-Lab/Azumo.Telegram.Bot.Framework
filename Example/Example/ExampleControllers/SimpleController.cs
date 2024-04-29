using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Core;
using Telegram.Bot.Framework.Core.Attributes;

namespace Example.ExampleControllers;

[TelegramController]
public class SimpleController
{
    [BotCommand("/SayHello", Description = "A simple message reply")]
    public static async Task HelloWorld(TelegramUserContext userContext)
    {
        var str = userContext.Message?.Text ?? string.Empty;
        _ = await userContext.SendTextMessageAsync($"Hello World! What you sent is {str}");
    }
}
