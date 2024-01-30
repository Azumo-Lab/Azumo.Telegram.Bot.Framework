using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Controllers.Abstracts.Internals;
internal interface IGetParameters
{
    public Task<EnumParametersResults> GetParametersAsync(TelegramUserChatContext telegramUserChatContext);

    public object?[] GetParams();

    public void Init();
}
