using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Internal.Params;

[TypeFor(typeof(CancellationToken))]
internal class ParamsCancellationToken : BaseGetParamDirect
{
    public override Task<object> GetParam(TelegramUserContext context) =>
        Task.FromResult<object>(context.UserServiceProvider.GetRequiredService<CancellationTokenSource>().Token);
}
