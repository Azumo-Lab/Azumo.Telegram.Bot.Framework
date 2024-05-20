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

using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.Controller.Results;
using Telegram.Bot.Framework.Helpers;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TelegramController
    {
        /// <summary>
        /// 
        /// </summary>
        protected TelegramContext TelegramContext { get; private set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        protected UserPermissions User { get; private set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public virtual void OnActionExecutionAsync(TelegramActionContext actionContext)
        {
            TelegramContext = actionContext.TelegramContext;
            User = actionContext.TelegramRequest.UserPermissions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual IActionResult MessageResultAsync(TelegramMessageBuilder message) =>
            new TextMessageResult(message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="images"></param>
        /// <returns></returns>
        protected virtual IActionResult MessageResultAsync(TelegramMessageBuilder message, string images) =>
            new PhotoMessageResult(images, message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionButtonResults"></param>
        /// <returns></returns>
        protected virtual IActionResult MessageResultAsync(TelegramMessageBuilder message, params ActionButtonResult[] actionButtonResults) =>
            new TextMessageResult(message, actionButtonResults);

        /// <summary>
        /// 空结果
        /// </summary>
        /// <remarks>
        /// 空结果，不进行任何的处理
        /// </remarks>
        /// <returns>返回空执行结果</returns>
        protected virtual IActionResult EmptyResultAsync() =>
            new NullActionResult();
    }
}
