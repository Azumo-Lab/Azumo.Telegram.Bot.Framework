using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Controller;

namespace Telegram.Bot.Framework.Core.Filters
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IFilter
    {
        public Task<bool> InvokeAsync(TelegramUserContext context, IExecutor executor);
    }
}
