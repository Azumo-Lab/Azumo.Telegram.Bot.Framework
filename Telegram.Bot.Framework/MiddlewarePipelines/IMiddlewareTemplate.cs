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

namespace Telegram.Bot.Framework.MiddlewarePipelines
{
    /// <summary>
    /// 中间件的模板方法(用于向中间件流水线中添加固定的执行中间件)
    /// </summary>
    internal interface IMiddlewareTemplate
    {
        /// <summary>
        /// 在用户的中间件执行之前
        /// </summary>
        /// <param name="ServiceProvider">服务提供</param>
        void BeforeAddMiddlewareHandles(IServiceProvider ServiceProvider);

        /// <summary>
        /// 在用户的中间件执行之后
        /// </summary>
        /// <param name="ServiceProvider">服务提供</param>
        void AfterAddMiddlewareHandles(IServiceProvider ServiceProvider);
    }
}
