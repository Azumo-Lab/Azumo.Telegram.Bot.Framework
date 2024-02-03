using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Controller.Controller;

namespace Telegram.Bot.Framework.Core.Controller.Install;
internal class Factory
{
    public static IExecutor GetExecutorInstance(EnumCommandType enumCommandType, params object[] objects)
    {
        IExecutor executor = enumCommandType switch
        {
            EnumCommandType.BotCommand => new BotCommandInvoker(
                                (objects[0] as ObjectFactory)!,
                                (objects[1] as Func<object, object[], object>)!,
                                (objects[2] as List<IGetParam>)!,
                                (objects[3] as Attribute[])!),
            EnumCommandType.Func => new FuncInvoker(
                                (objects[0] as Delegate)!,
                                (objects[1] as List<IGetParam>)!,
                                (objects[2] as Attribute[])!),
            _ => throw new ArgumentException($"{nameof(enumCommandType)} 值非法"),
        };
        return executor;
    }
}
