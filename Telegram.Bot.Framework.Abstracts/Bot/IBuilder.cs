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

namespace Telegram.Bot.Framework.Abstracts.Bot
{
    /// <summary>
    /// 机器人创建接口
    /// </summary>
    /// <remarks>
    /// 用于创建机器人的接口，推荐使用扩展方法来进行扩展，添加功能
    /// </remarks>
    public interface IBuilder
    {
        /// <remarks>
        /// Token 是类似这样的字符串：<br/>
        /// <c>5298058194:AAFa9N1GiF_i7W0fV4aWgz22IGv8kzVZ13Q</c><br/>
        /// 其中，<c>5298058194</c> 的部分，是机器人的User ID
        /// <para>
        /// Token可以通过 BotFather 来进行获取
        /// </para>
        /// <para>
        /// 详细可以访问：<see cref="https://t.me/BotFather"/>
        /// </para>
        /// </remarks>
        public string Token { get; set; }

        /// <summary>
        /// 代理设置用
        /// </summary>
        /// <remarks>
        /// 众所周知，中国大陆无法访问Telegram，运行，测试，等简单操作都成了大问题，可以通过设置代理的方式，来进行连接运行
        /// </remarks>
        public HttpClient Proxy { get; set; }

        /// <summary>
        /// 用于创建一些 <see cref="IBuilder"/> 用到的一些服务。
        /// </summary>
        public IServiceProvider BuilderServices { get; }

        /// <summary>
        /// 用于创建整个机器人运行时候需要的服务
        /// </summary>
        /// <remarks>
        /// 例如 <see cref="IStartup"/> 接口的添加等操作，需要用到这个
        /// </remarks>
        public IServiceCollection RuntimeServices { get; }

        /// <summary>
        /// 创建 <see cref="ITelegramBot"/> 对象
        /// </summary>
        /// <returns>返回 <see cref="ITelegramBot"/> 机器人接口</returns>
        public ITelegramBot Build();
    }
}
