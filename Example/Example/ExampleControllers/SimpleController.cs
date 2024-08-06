using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller.Results;
using Telegram.Bot.Framework.Helpers;

namespace Example.ExampleControllers
{
    public class SimpleController : TelegramController
    {
        [BotCommand("/SayHello", Description = "A simple message reply")]
        public IActionResult HelloWorld()
        {
            var message = TelegramMessageBuilder.Html
                .Bold("Hello World");

            message.ToString();

            var btns = new ActionButtonResult[]
            {
                new("Say Hello", () => MessageResult(message)),
                new("Empty Btn", () => { })
            };

            return MessageResult(message, btns);
        }
    }
}
