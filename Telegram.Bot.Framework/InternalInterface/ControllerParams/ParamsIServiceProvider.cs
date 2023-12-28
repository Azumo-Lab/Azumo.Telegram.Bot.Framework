using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.InternalInterface.ControllerParams
{
    [TypeFor(typeof(IServiceProvider))]
    internal class ParamsIServiceProvider : BaseControllerParam
    {
        public override Task<object> CatchObjs(TelegramUserChatContext tGChat) =>
            Task.FromResult<object>(tGChat.UserScopeService);

        public override Task<bool> SendMessage(TelegramUserChatContext tGChat, ParamAttribute paramAttribute) =>
            Task.FromResult(true);
    }
}
