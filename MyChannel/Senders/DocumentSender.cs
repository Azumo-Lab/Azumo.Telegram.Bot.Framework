using Telegram.Bot;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.InternalInterface;
using Telegram.Bot.Types;

namespace MyChannel.Senders
{
    [TypeFor(typeof(Document))]
    internal class DocumentSender : BaseControllerParam
    {
        public override IControllerParamSender? ParamSender { get => this; set => _ = value; }

        public override Task<object> CatchObjs(TGChat tGChat) => Task.FromResult<object>(tGChat.Message?.Document!);

        public override async Task Send(ITelegramBotClient botClient, ChatId chatId, ParamAttribute paramAttribute)
        {
            var name = paramAttribute?.Name ?? string.Empty;
            if (string.IsNullOrEmpty(name))
                await base.Send(botClient, chatId, paramAttribute);
            else
                _ = await botClient.SendTextMessageAsync(chatId, $"请发送{name}");
        }
    }
}
