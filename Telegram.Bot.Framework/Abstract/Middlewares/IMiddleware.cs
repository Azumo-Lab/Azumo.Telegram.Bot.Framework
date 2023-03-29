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
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.InternalImplementation.Sessions;

namespace Telegram.Bot.Framework.Abstract.Middlewares
{
    /// <summary>
    /// Action接口
    /// </summary>
    public interface IMiddleware
    {
        /// <summary>
        /// 开始执行Action
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="NextMiddleware">下一个中间件</param>
        /// <returns></returns>
        public Task Execute(ITelegramSession Session, MiddlewareHandle NextMiddleware);
    }
}
