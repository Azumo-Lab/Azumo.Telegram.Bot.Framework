using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Params
{
    public interface IParamMaker : ISenderContext
    {
        public Task SendParamInfo();

        public Task<bool> ParamCheck();

        public Task<object> GetParam();
    }
}
