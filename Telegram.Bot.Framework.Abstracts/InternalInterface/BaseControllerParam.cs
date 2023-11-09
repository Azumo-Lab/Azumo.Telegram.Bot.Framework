using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.InternalInterface
{
    internal abstract class BaseControllerParam : IControllerParam
    {
        public IControllerParamSender? ParamSender { get; set; }

        public abstract Task<object> CatchObjs(TGChat tGChat);

        public async Task SendMessage(TGChat tGChat)
        {
            await (ParamSender ?? new NullControllerParamSender()).Send(tGChat.BotClient, tGChat.ChatId);
        }
    }

    internal class NullControllerParamSender : IControllerParamSender
    {
        public async Task Send(ITelegramBotClient botClient, ChatId chatId)
        {
            _ = await botClient.SendTextMessageAsync(chatId, "请输入参数");
        }
    }
}
