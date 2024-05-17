//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller.Results;

namespace Telegram.Bot.Framework.Controller.Params
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseGetParam : IGetParam
    {
        /// <summary>
        /// 
        /// </summary>
        public ParamAttribute? ParamAttribute { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract Task<object> GetParam(TelegramActionContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task<IActionResult?> SendMessage(TelegramActionContext context)
        {
            if (ParamAttribute?.IGetParmType != null)
            {
                var empty = Array.Empty<object>();
                if (ActivatorUtilities.CreateInstance(context.ServiceProvider, ParamAttribute.IGetParmType, empty) is IGetParam iGetParam)
                    return await iGetParam.SendMessage(context);
            }
            return new TextMessageResult(ParamAttribute?.Message ?? "请输入参数");
        }
    }
}
