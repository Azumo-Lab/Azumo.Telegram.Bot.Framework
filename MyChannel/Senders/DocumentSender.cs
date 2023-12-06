using Telegram.Bot;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.InternalInterface;
using Telegram.Bot.Types;

namespace MyChannel.Senders
{
    [TypeFor(typeof(Document))]
    internal class DocumentSender : BaseControllerParam, IControllerParamSender
    {
        public override IControllerParamSender? ParamSender { get => this; set => _ = value; }

        public override Task<object> CatchObjs(TGChat tGChat) => Task.FromResult<object>(tGChat.Message?.Document!);

        public async Task Send(ITelegramBotClient botClient, ChatId chatId) => _ = await botClient.SendTextMessageAsync(chatId, "请发送文件");
    }
}
