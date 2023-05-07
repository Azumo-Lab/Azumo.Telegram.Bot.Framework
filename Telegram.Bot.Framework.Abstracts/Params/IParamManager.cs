using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Params
{
    public interface IParamManager
    {
        public Task Invoke(IParamMaker paramMaker);
    }
}
