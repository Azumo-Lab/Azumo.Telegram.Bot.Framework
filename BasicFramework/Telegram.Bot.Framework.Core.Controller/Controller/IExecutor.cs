using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Core.Controller.Controller;
internal interface IExecutor
{
    public IReadOnlyList<IGetParam> Parameters { get; }

    public Task Invoke(IServiceProvider serviceProvider, object[] param);
}
