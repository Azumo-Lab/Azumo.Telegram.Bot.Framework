//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ActionResult : IActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        protected List<IMessageFragment> MessageFragments { get; } =
#if NET8_0_OR_GREATER
            [];
#else
            new List<IMessageFragment>();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task ExecuteResultAsync(TelegramActionContext context, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var item in MessageFragments)
                {
                    if (item.Data == null)
                        continue;

                    //await combination[item.FragmentType](context, item);
                }
            }
            finally
            {
                foreach (var item in MessageFragments)
                {
                    if (item.Data == null)
                        continue;

                    foreach (var data in item.Data)
                        await data.DisposeAsync();
                }
            }
        }
    }
}
