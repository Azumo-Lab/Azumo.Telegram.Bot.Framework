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
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.CorePipeline
{
    /// <summary>
    /// 
    /// </summary>
    internal class PipelineControllerInvoke : IProcessAsync<TelegramUserChatContext>
    {
        /// <summary>
        /// 开始执行控制器流程
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="pipelineController"></param>
        /// <returns></returns>
        public async Task<TelegramUserChatContext> ExecuteAsync(TelegramUserChatContext chat, IPipelineController<TelegramUserChatContext> pipelineController)
        {
            // 获取必要的数据
            var controllerManager = chat.UserScopeService.GetRequiredService<IControllerManager>();
            var botCommand = controllerManager.GetCommand(chat);

            // 控制器执行的过滤器，可自定义的流程
            foreach (var item in chat.UserScopeService.GetServices<IControllerFilter>()?.ToList() ?? [])
            {
                var result = await item.Execute(chat, botCommand);
                if (!result)
                    return await pipelineController.StopAsync(chat);
            }

            // 获取参数
            var controllerParamManager = chat.UserScopeService.GetRequiredService<IControllerParamManager>();
            if (botCommand != null)
            {
                controllerParamManager.Clear();
                controllerParamManager.SetBotCommand(botCommand);
                controllerParamManager.ControllerParams = new List<IControllerParam>(botCommand.ControllerParams);
            }
            var resultEnum = await controllerParamManager.NextParam(chat);
            if (resultEnum != ResultEnum.Finish)
                return await pipelineController.StopAsync(chat);

            botCommand ??= controllerParamManager.GetBotCommand();
            if (botCommand == null)
                return await pipelineController.StopAsync(chat);

            // 执行控制器
            try
            {
                var telegramController = (TelegramController)ActivatorUtilities.CreateInstance(chat.UserScopeService, botCommand!.Controller, []);
                await telegramController.ControllerInvokeAsync(chat, botCommand.Func, controllerParamManager);
            }
            catch (Exception)
            { }

            // 执行下一个
            return await pipelineController.NextAsync(chat);
        }
    }
}
