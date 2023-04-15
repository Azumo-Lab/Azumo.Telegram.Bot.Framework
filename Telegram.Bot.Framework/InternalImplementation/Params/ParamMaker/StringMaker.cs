using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Attributes;

namespace Telegram.Bot.Framework.InternalImplementation.Params.ParamMaker
{
    [TypeFor(typeof(string))]
    internal class StringMaker : IParamMaker
    {
        public Task<object> GetParam(ITelegramSession session)
        {
            return Task.FromResult<object>(session.Update.Message.Text);
        }

        public Task<bool> ParamCheck(ITelegramSession session)
        {
            return Task.FromResult(true);
        }
    }
}
