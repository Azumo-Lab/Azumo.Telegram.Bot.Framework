using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Internal.Params;

[TypeFor(typeof(ITelegramBotClient))]
internal class ParamsITelegramBotClient : IGetParam
{
    public ParamAttribute? ParamAttribute { get; set; }

    public Task<object> GetParam(TelegramUserContext context) =>
        Task.FromResult<object>(context.BotClient);
    public Task<bool> SendMessage(TelegramUserContext context) => 
        Task.FromResult(true);
}
