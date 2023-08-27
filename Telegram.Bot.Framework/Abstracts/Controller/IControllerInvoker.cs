using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;
using BotCommand = Telegram.Bot.Framework.Reflections.BotCommand;

namespace Telegram.Bot.Framework.Abstracts.Controller
{
    internal interface IControllerInvoker
    {
        Task InvokeAsync(BotCommand command, TGChat tGChat);

        BotCommand GetCommand(Update update);
    }
}
