using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller.Results;

namespace Example.ExampleControllers
{
    public class SimpleController : TelegramController
    {
        [BotCommand("/SayHello", Description = "A simple message reply")]
        public Task<IActionResult> HelloWorld()
        {
            return MessageResultAsync("Hello World");
        }
    }
}
