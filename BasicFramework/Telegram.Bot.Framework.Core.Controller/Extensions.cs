using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Storage;

namespace Telegram.Bot.Framework.Core.Controller;
public static class Extensions
{
    internal static void AddCommand(this ISession session, IExecutor executor) => 
        session.Add("{ADD76730-6FE8-4B6C-8E40-AAD5D6883DC8}", executor);

    internal static IExecutor GetCommand(this ISession session) =>
        session.Get<IExecutor>("{ADD76730-6FE8-4B6C-8E40-AAD5D6883DC8}");
}
