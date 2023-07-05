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

using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.CallBack;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Models;

namespace Telegram.Bot.Framework.MiddlewarePipelines.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    internal class ActionCallBack : IMiddleware
    {
        public async Task Execute(ITelegramChat Chat, IPipelineController PipelineController)
        {
            ICallBack callBack = Chat.CallBackManager.GetCallBack();
            if (callBack != null)
            {
                CallBackResult callBackResult = callBack.Invoke(Chat);
                if (callBackResult.Success)
                {
                    Task task = (Task)callBackResult.Result;
                    if (task != null)
                        try
                        {
                            await task;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                }
                else
                {
                    Console.WriteLine(callBackResult.Exception.Message);
                }
            }

            await PipelineController.Next(Chat);
        }
    }
}
