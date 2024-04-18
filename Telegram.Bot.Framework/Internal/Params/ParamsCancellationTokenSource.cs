using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Internal.Params;

[TypeFor(typeof(CancellationTokenSource))]
internal class ParamsCancellationTokenSource : BaseGetParamDirect
{
    public override Task<object> GetParam(TelegramUserContext context) => 
        Task.FromResult<object>(context.UserServiceProvider.GetRequiredService<CancellationTokenSource>());
}
