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
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstract.Bots
{
    /// <summary>
    /// 机器人创建接口
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpClient Proxy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection BuilderServices { get; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection RuntimeServices { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITelegramBot Build();
    }
}
