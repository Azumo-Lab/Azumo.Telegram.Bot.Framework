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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.User;
using Telegram.Bot.Framework.Pipeline.Abstracts;

namespace Telegram.Bot.Framework.CorePipelines.Proc
{
    /// <summary>
    /// 获取参数
    /// </summary>
    /// <remarks>
    /// 用于获取一个请求中包含的参数，例如：
    /// 指定方法中需要的是String类型的参数，则会获取Message的信息作为参数传递
    /// </remarks>
    internal class PipelineGetParameters : IProcess<IChat>
    {
        private const string PARAMSKEY = nameof(PARAMSKEY);
        private const string PARAMINDEX = nameof(PARAMINDEX);
        public Task<IChat> Execute(IChat t, IPipelineController<IChat> pipelineController)
        {
            ICommandInfo commandInfo;
            List<(ParameterInfo ParameterInfo, Type Message, Type Catch)> values;
            if (t.SessionCache.ContainsKey(PARAMSKEY))
                values = t.SessionCache.Get(PARAMSKEY, new List<(ParameterInfo ParameterInfo, Type Message, Type Catch)>());
            else if ((commandInfo = t.Request.Find()) != null)
                values = SaveParam(t, commandInfo.Params);
            else
                return pipelineController.Next(t);

            if (Catch(t, values))
            {

            }
            if (!SendMessage(t, values))
            {
                return pipelineController.Stop(t);
            }
            return pipelineController.Next(t);
        }

        private class ParamInfo
        {

        }

        private static bool Catch(IChat t, List<(ParameterInfo ParameterInfo, Type Message, Type Catch)> values)
        {
            return false;
        }

        private static bool SendMessage(IChat t, List<(ParameterInfo ParameterInfo, Type Message, Type Catch)> values)
        {
            if (values.IsEmpty())
            {
                t.SessionCache.Remove(PARAMSKEY);
                return true;
            }

            int i = t.SessionCache.Get(PARAMSKEY, 0);
            do
            {
                (ParameterInfo ParameterInfo, Type Message, Type Catch) = values[i];

                IParamMessage paramMessage = (IParamMessage)ActivatorUtilities.CreateInstance(t.ChatService, Message, Array.Empty<object>());
                Attribute.GetCustomAttribute(ParameterInfo, typeof(Type));
                paramMessage.SendMessage(t, "");

                i++;
                t.SessionCache.Save(PARAMINDEX, i);
                return false;
            } while (i < values.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static List<(ParameterInfo ParameterInfo, Type Message, Type Catch)> SaveParam(IChat t, List<(ParameterInfo ParameterInfo, Type Message, Type Catch)> values)
        {
            t.SessionCache.Save(PARAMSKEY, values);
            return values;
        }
    }
}
