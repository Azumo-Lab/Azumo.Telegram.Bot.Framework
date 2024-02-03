using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework.Core.Controller.Controller;
internal class BotCommandInvoker(ObjectFactory objectFactory, Func<object, object[], object> func, List<IGetParam> paramList, Attribute[] attributes)
    : IExecutor
{
    private readonly ObjectFactory _objectFactory = objectFactory;

    private readonly Func<object, object[], object> _func = func;

    public IReadOnlyList<IGetParam> Parameters { get; } = new List<IGetParam>(paramList);

    public Attribute[] Attributes { get; } = attributes;

    public Task Invoke(IServiceProvider serviceProvider, object[] param)
    {
        var obj = _objectFactory(serviceProvider, []);
        return _func(obj, param) is Task task ? task : Task.CompletedTask;
    }
}
