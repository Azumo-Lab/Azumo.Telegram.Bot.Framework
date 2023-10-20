//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TelegramController
    {
        /// <summary>
        /// 
        /// </summary>
        protected TGChat Chat { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Chat"></param>
        /// <param name="func"></param>
        /// <param name="controllerParamManager"></param>
        /// <returns></returns>
        internal async Task ControllerInvokeAsync(TGChat Chat, Func<TelegramController, object[], Task> func, IControllerParamManager controllerParamManager)
        {
            this.Chat = Chat;
            Logger = this.Chat.UserService.GetService<ILogger<TelegramController>>();
            try
            {
                if (func(this, controllerParamManager.GetObjects() ?? Array.Empty<object>()) is Task task)
                    await task;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                controllerParamManager.Clear();
            }
        }
    }
}
