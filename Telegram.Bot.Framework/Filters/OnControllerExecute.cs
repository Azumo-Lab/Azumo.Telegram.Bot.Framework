//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.Controller.Results;

namespace Telegram.Bot.Framework.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class OnControllerExecute : IOnControllerExecute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramActionContext"></param>
        /// <returns></returns>
        public abstract Task<IActionResult> OnForbidden(TelegramActionContext telegramActionContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramActionContext"></param>
        /// <returns></returns>
        public Task<IActionResult> OnParamterError(TelegramActionContext telegramActionContext) => 
            Task.FromResult<IActionResult>(new TextMessageResult("您输入的参数错误"));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramActionContext"></param>
        /// <returns></returns>
        public abstract Task<IActionResult> OnUnauthorized(TelegramActionContext telegramActionContext);
    }
}
