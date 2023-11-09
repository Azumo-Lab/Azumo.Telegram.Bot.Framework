using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.InternalInterface.ControllerParams
{
    [TypeFor(typeof(string))]
    internal class StringParams : BaseControllerParam
    {
        public override async Task<object> CatchObjs(TGChat tGChat)
        {
            return await Task.FromResult<object>(tGChat.Message?.Text ?? string.Empty);
        }
    }
}
