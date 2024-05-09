using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Controller;

namespace Telegram.Bot.Framework.Core.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RoleFilter : IFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="executor"></param>
        /// <returns></returns>
        async Task<bool> IFilter.InvokeAsync(TelegramUserContext context, IExecutor executor) =>
            !executor.Cache.TryGetValue(Extensions.RolesKey, out var roleObject)
            || !(roleObject is List<string> roleList)
            || await RoleCheck(context.Session.GetRoles().ToArray(), roleList.ToArray());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="executorRoles"></param>
        /// <returns></returns>
        protected abstract Task<bool> RoleCheck(string[] userRoles, string[] executorRoles);
    }
}
