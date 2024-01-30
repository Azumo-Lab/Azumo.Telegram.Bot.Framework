using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controllers.Abstracts;
using Telegram.Bot.Framework.Controllers.Abstracts.Internals;

namespace Telegram.Bot.Framework.Controllers.Internals;
internal class FunctionBotCommand(Delegate func, List<IParameterCapturer> parameterCapturers)
    : BaseGetParamters(parameterCapturers), IExecutor
{
    private readonly Delegate _Func = func;

    public Task Invoke(IServiceProvider serviceProvider) =>
        _Func.DynamicInvoke(GetParams()) is Task task ? task : Task.CompletedTask;
        
}
