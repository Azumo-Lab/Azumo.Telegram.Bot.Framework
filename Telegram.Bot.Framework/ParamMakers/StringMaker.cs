using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Framework.ParamMakers
{
    [ParamMakerFor(typeof(string))]
    public class StringMaker : IParamMaker
    {
        public async Task<object> GetParam(string Message, TelegramContext context, IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
