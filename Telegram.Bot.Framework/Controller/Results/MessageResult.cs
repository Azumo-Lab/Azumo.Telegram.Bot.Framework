using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Controller.Results
{
    internal class MessageResult : ActionResult
    {
        public MessageResult(params IMessageFragment[] entities) => 
            MessageFragments.AddRange(entities);
    }
}
