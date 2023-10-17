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
using Telegram.Bot.Framework.Pipeline.Abstracts;

namespace Telegram.Bot.Framework.Pipelines
{
    /// <summary>
    /// 
    /// </summary>
    internal class ProcessControllerInvoke : IProcess<TGChat>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pipelineController"></param>
        /// <returns></returns>
        public async Task<TGChat> Execute(TGChat t, IPipelineController<TGChat> pipelineController)
        {
            IControllerInvoker controllerInvoker = t.UserService.GetService<IControllerInvoker>();
            try
            {
                await controllerInvoker.InvokeAsync(controllerInvoker.GetCommand(t), t);
            }
            catch (Exception)
            {

            }
            return await pipelineController.Next(t);
        }
    }
}
