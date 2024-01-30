using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Controller.Params;

namespace Telegram.Bot.Framework.Controller.ControllerInvoker;
internal class BotCommandInvoker(Func<object, object[], object> func, ObjectFactory objectFactory, List<BaseParameterGetter> parameterGetters) : IExecutor
{
    private readonly Func<object, object[], object> _func = func;

    private readonly ObjectFactory _objectFactory = objectFactory;

    private readonly ParameterManager _parameterManager = new(parameterGetters);

    public async Task Invoke(IServiceProvider serviceProvider, TelegramUserChatContext context)
    {
        var result = await _parameterManager.Read(context);
        if (result == EnumReadParam.OK)
            return;

        var obj = _objectFactory(serviceProvider, []);
        await (_func(obj, _parameterManager.GetParams()) is Task task ? task : Task.CompletedTask);
    }
}
