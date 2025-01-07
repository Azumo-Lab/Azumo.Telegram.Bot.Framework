//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
//
//  Author: 牛奶

using System.Threading.Tasks;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller.Results;

namespace Telegram.Bot.Framework.Controller.Params
{
    /// <summary>
    /// 参数获取接口
    /// </summary>
    /// <remarks>
    /// 用于获取指令接口，接口的实现类需要添加 <see cref="TypeForAttribute"/>
    /// </remarks>
    public interface IGetParam
    {
        /// <summary>
        /// 参数的标签
        /// </summary>
        public ParamAttribute? ParamAttribute { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <remarks>
        /// 返回值的类型是 <see cref="IActionResult"/> 类型，将会由程序进行处理
        /// </remarks>
        /// <param name="context">指令上下文</param>
        /// <returns>消息执行结果</returns>
        public Task<IActionResult?> SendMessage(TelegramActionContext context);

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="context">指令上下文</param>
        /// <returns>读取的参数</returns>
        public Task<object?> GetParam(TelegramActionContext context);
    }
}
