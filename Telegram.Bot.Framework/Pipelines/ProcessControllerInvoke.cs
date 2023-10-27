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
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Pipeline.Abstracts;
using BotCommand = Telegram.Bot.Framework.Reflections.BotCommand;

namespace Telegram.Bot.Framework.Pipelines
{
    /// <summary>
    /// 
    /// </summary>
    internal class ProcessControllerInvoke : IProcessAsync<(TGChat tGChat, IControllerContext controllerContext)>, IPipelineName
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IPipelineController<(TGChat, IControllerParamManager)> __ControllerParamManager;

        /// <summary>
        /// 
        /// </summary>
        public ProcessControllerInvoke()
        {
            __ControllerParamManager = PipelineFactory.CreateIPipelineBuilder<(TGChat, IControllerParamManager)>()
                .AddProcedure(new ProcessParams())
                .CreatePipeline("PARAM")
                .BuilderPipelineController();
        }

        public string Name => "ProcessControllerInvoke";

        public async Task<(TGChat tGChat, IControllerContext controllerContext)> ExecuteAsync((TGChat tGChat, IControllerContext controllerContext) t, IPipelineController<(TGChat tGChat, IControllerContext controllerContext)> pipelineController)
        {
            try
            {
                IControllerInvoker controllerInvoker = t.tGChat.UserService.GetService<IControllerInvoker>();
                IControllerParamManager controllerParamManager = t.tGChat.UserService.GetService<IControllerParamManager>();
                BotCommand botCommand = t.controllerContext.BotCommand;
                if (botCommand == null)
                {
                    if (controllerParamManager.BotCommand == null)
                        return await pipelineController.StopAsync(t);
                    botCommand = controllerParamManager.BotCommand;
                }
                else
                {
                    controllerParamManager.Clear();
                    controllerParamManager.BotCommand = botCommand;
                }

                (t.tGChat, controllerParamManager) = await __ControllerParamManager.SwitchTo("PARAM", (t.tGChat, controllerParamManager));
                if (__ControllerParamManager.PipelineResultEnum != PipelineResultEnum.Success)
                    return await pipelineController.StopAsync(t);

                await controllerInvoker.InvokeAsync(botCommand, t.tGChat, controllerParamManager);
            }
            catch (Exception)
            {

            }
            return await pipelineController.NextAsync(t);
        }
    }
}
