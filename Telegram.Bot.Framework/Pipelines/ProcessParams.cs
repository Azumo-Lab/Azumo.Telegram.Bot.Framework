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
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.Pipelines
{
    /// <summary>
    /// 
    /// </summary>
    internal class ProcessParams : IProcessAsync<(TGChat, IControllerParamManager)>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pipelineController"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(TGChat, IControllerParamManager)> ExecuteAsync((TGChat, IControllerParamManager) t, IPipelineController<(TGChat, IControllerParamManager)> pipelineController)
        {
            BotCommandParams botCommandParams = t.Item2.BotCommand.BotCommandParams[t.Item2.Index];

            switch (t.Item2.ParamStauts)
            {
                case ParamStauts.Read:
                    IMessage message = (IMessage)ActivatorUtilities.CreateInstance(t.Item1.UserService, botCommandParams.MessageType, Array.Empty<object>());
                    await message.SendAsync(t.Item1);
                    t.Item2.ParamStauts = ParamStauts.Write;
                    return await pipelineController.StopAsync(t);
                case ParamStauts.Write:
                    Type newCatchType = null;
                    if (botCommandParams.CatchType == null)
                    {
                        ICatchManager catchManager = t.Item1.UserService.GetService<ICatchManager>();
                        newCatchType = catchManager.GetCatch(botCommandParams.ParameterInfo.ParameterType);
                    }
                    ICatch mycatch = (ICatch)ActivatorUtilities.CreateInstance(t.Item1.UserService, newCatchType, Array.Empty<object>());
                    if (!mycatch.Catch(t.Item1, out object obj))
                        return await pipelineController.StopAsync(t);

                    t.Item2.AddObject(obj);
                    t.Item2.ParamStauts = ParamStauts.Read;
                    t.Item2.Index++;
                    if (t.Item2.Index >= t.Item2.BotCommand.BotCommandParams.Count)
                        return await pipelineController.NextAsync(t);

                    return await pipelineController.StopAsync(t);
                default:
                    throw new Exception($"t.Item2.ParamStauts: {t.Item2.ParamStauts}");
            }
        }
    }
}
