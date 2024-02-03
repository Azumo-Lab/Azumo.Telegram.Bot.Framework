namespace Telegram.Bot.Framework.Core.Controller.Controller;
internal class FuncInvoker(Delegate func, List<IGetParam> paramList, Attribute[] attributes)
    : IExecutor
{
    public IReadOnlyList<IGetParam> Parameters { get; } = new List<IGetParam>(paramList);

    public Attribute[] Attributes { get; } = attributes;

    public Task Invoke(IServiceProvider serviceProvider, object[] param) => 
        func.DynamicInvoke(param) is Task task ? task : Task.CompletedTask;
}
