using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Internal.Params;

/// <summary>
/// 
/// </summary>
[TypeFor(typeof(string))]
internal class ParamsString : BaseGetParam
{
    public override Task<object> GetParam(TelegramUserContext context) =>
        Task.FromResult<object>(context.Message?.Text!);
}
