using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controllers.Abstracts.Internals;

namespace Telegram.Bot.Framework.Controllers.Internals;
internal class FunctionBotCommand
    : IExecutor
{
    public Task Invoke(params object[] objs) => throw new NotImplementedException();
}
