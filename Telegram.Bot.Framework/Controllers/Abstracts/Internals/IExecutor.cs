using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Controllers.Abstracts.Internals;

/// <summary>
/// 
/// </summary>
internal interface IExecutor
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public Task Invoke(IServiceProvider serviceProvider);
}
