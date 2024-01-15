//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Azumo.Pipeline.Abstracts;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.CorePipelines
{
    /// <summary>
    /// 
    /// </summary>
    internal class PipelineControllerParams : IProcessAsync<TelegramUserChatContext>
    {
        /// <summary>
        /// 开始进行方法参数的获取
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="t"></param>
        /// <param name="pipelineController"></param>
        /// <returns></returns>
        public async Task<TelegramUserChatContext> ExecuteAsync(TelegramUserChatContext t, IPipelineController<TelegramUserChatContext> pipelineController)
        {
            var controllerManager = t.UserScopeService.GetRequiredService<IControllerParamManager>();
            var resultEnum = await controllerManager.NextParam(t);

            return resultEnum != ResultEnum.Finish ? await pipelineController.StopAsync(t) : await pipelineController.NextAsync(t);
        }
    }
}
