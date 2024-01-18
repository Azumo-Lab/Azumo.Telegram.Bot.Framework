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

namespace Telegram.Bot.Framework.CorePipelines;

internal class PipelineControllerInvoker : IProcessAsync<TelegramUserChatContext>
{
    /// <summary>
    /// 控制器的执行
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="pipelineController"></param>
    /// <returns></returns>
    public async Task<TelegramUserChatContext> ExecuteAsync(TelegramUserChatContext chat, IPipelineController<TelegramUserChatContext> pipelineController)
    {
        // 获取参数
        var controllerParamManager = chat.UserScopeService.GetRequiredService<IControllerParamManager>();

        var botCommand = controllerParamManager.GetBotCommand();
        if (botCommand == null)
            return await pipelineController.StopAsync(chat);

        // 执行控制器
        try
        {
            var Invoker = botCommand.Invoker;
            await Invoker(chat, controllerParamManager);
        }
        catch (Exception)
        {

        }
        finally
        {
            controllerParamManager.Dispose();
        }

        // 执行下一个
        return await pipelineController.NextAsync(chat);
    }
}
