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
using System.Reflection;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstract.Controller
{
    /// <summary>
    /// 创建IControllerContext
    /// </summary>
    internal interface IControllerContextBuilder
    {
        /// <summary>
        /// 开始创建
        /// </summary>
        /// <returns>创建好的IControllerContext</returns>
        public IControllerContext Build();

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="attribute">标签</param>
        /// <returns>这个建造类</returns>
        public IControllerContextBuilder AddAttribute(Attribute attribute);

        /// <summary>
        /// 添加多个标签
        /// </summary>
        /// <param name="attributes">多个标签</param>
        /// <returns>这个建造类</returns>
        public IControllerContextBuilder AddAttributes(Attribute[] attributes);

        /// <summary>
        /// 添加一个方法委托
        /// </summary>
        /// <param name="Action">委托</param>
        /// <returns>这个建造类</returns>
        public IControllerContextBuilder AddDelegate(Func<TelegramController, object[], Task> Action);

        /// <summary>
        /// 添加一个参数信息
        /// </summary>
        /// <param name="paramType">单个参数信息</param>
        /// <returns>这个建造类</returns>
        public IControllerContextBuilder AddParam(ParameterInfo paramType);

        /// <summary>
        /// 添加多个参数的信息
        /// </summary>
        /// <param name="paramTypes">多个参数信息</param>
        /// <returns>这个建造类</returns>
        public IControllerContextBuilder AddParams(ParameterInfo[] paramTypes);

        /// <summary>
        /// 添加一个方法信息
        /// </summary>
        /// <param name="methodInfo">一个方法信息</param>
        /// <returns>这个建造类</returns>
        public IControllerContextBuilder AddMethodInfo(MethodInfo methodInfo);
    }
}
