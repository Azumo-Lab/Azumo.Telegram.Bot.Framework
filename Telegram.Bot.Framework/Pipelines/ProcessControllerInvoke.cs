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
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.Pipelines
{
    /// <summary>
    /// 
    /// </summary>
    internal class ProcessControllerInvoke : IProcessAsync<TGChat>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pipelineController"></param>
        /// <returns></returns>
        public async Task<TGChat> ExecuteAsync(TGChat t, IPipelineController<TGChat> pipelineController)
        {
            try
            {
                IControllerInvoker controllerInvoker = t.UserService.GetService<IControllerInvoker>();
                IControllerParamManager controllerParamManager = t.UserService.GetService<IControllerParamManager>();
                BotCommand botCommand = controllerInvoker.GetCommand(t);
                if (botCommand == null)
                {
                    if (controllerParamManager.BotCommand == null)
                        await pipelineController.StopAsync(t);
                }
                else
                {
                    controllerParamManager.BotCommand = botCommand;
                }
                
                (t, controllerParamManager) = await __ControllerParamManager.NextAsync((t, controllerParamManager));
                if (__ControllerParamManager.PipelineResultEnum != PipelineResultEnum.Success)
                    return await pipelineController.StopAsync(t);

                await controllerInvoker.InvokeAsync(botCommand, t, controllerParamManager);
            }
            catch (Exception)
            {

            }
            return await pipelineController.NextAsync(t);
        }
    }
}
