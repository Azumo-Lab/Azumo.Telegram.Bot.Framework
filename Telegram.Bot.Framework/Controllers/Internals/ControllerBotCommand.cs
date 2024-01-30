using Telegram.Bot.Framework.Controllers.Abstracts;
using Telegram.Bot.Framework.Controllers.Abstracts.Internals;

namespace Telegram.Bot.Framework.Controllers.Internals;

/// <summary>
/// 
/// </summary>
/// <param name="botFunc"></param>
/// <param name="objectFactory"></param>
/// <param name="parameterCapturers"></param>
internal class ControllerBotCommand(Func<object, object[], Task> botFunc, ObjectFactory objectFactory, List<IParameterCapturer> parameterCapturers)
        : BaseGetParamters(parameterCapturers), IExecutor
{
    /// <summary>
    /// 
    /// </summary>
    private readonly Func<object, object[], Task> __BotFunc = botFunc;

    /// <summary>
    /// 
    /// </summary>
    private readonly ObjectFactory __ObjectFactory = objectFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public Task Invoke(IServiceProvider serviceProvider) =>
        __BotFunc(__ObjectFactory(serviceProvider, []), GetParams()!);

}
