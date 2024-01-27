using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controllers.Abstracts.Internals;

namespace Telegram.Bot.Framework.Controllers.Internals;
internal class ControllerBotCommand(Func<object, object[], Task> botFunc, ObjectFactory objectFactory)
    : IExecutor, IGetParameters
{
    private readonly Func<object, object[], Task> __BotFunc = botFunc;

    private readonly ObjectFactory __ObjectFactory = objectFactory;

    public object[] GetParams() => throw new NotImplementedException();

    public Task Invoke(IServiceProvider serviceProvider) =>
        __BotFunc(__ObjectFactory(serviceProvider, []), GetParams());

}
