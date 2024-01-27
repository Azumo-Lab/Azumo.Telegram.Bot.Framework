using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Controllers.Abstracts.Internals;
internal interface IGetParameters
{
    public object[] GetParams();
}
