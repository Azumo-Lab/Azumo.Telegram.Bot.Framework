using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalInterface.ControllerParams
{
    [TypeFor(typeof(PhotoSize))]
    internal class PhotoSizeParams : BaseControllerParam
    {
        public override Task<object> CatchObjs(TelegramUserChatContext tGChat) => Task.FromResult<object>(tGChat.Message?.Photo?.OrderBy(x => x.FileSize)?.FirstOrDefault());

        public override async Task Send(ITelegramBotClient botClient, ChatId chatId, ParamAttribute paramAttribute)
        {
            var name = paramAttribute?.Name ?? string.Empty;
            await botClient.SendTextMessageAsync(chatId, $"请发送{name}");
        }
    }
}
