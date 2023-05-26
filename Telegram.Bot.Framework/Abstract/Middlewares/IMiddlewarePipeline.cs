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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Managements;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstract.Middlewares
{
    /// <summary>
    /// 中间件流水线
    /// </summary>
    public interface IMiddlewarePipeline
    {
        /// <summary>
        /// 该流水线处理的请求类型
        /// </summary>
        public UpdateType InvokeType { get; }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="session">访问的请求对话</param>
        /// <returns>异步方法</returns>
        public Task Execute(IChat chat);
    }
}
