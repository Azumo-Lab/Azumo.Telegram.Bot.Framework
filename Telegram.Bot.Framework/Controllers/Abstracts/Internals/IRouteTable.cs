using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controllers.Abstracts.Internals;
internal interface IRouteTable
{
    public IExecutor Find(string botCommand, MessageType messageType = MessageType.Unknown);
}
