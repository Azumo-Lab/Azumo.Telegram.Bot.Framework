using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.ControllerInvoker;

namespace Telegram.Bot.Framework.Controller.Install;
internal class BotCommandBuilder : IBotCommandBuilder
{
    public void AddAttribute(IAttributeBuilder builder)
    {

    }
    public IExecutor Build(MethodInfo methodInfo)
    {
        return null!;
    }
}
