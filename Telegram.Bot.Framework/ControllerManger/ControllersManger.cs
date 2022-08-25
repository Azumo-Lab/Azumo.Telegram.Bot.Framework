//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Sokushu> see <https://github.com/sokushu/Telegram.Bot.Net/>
//
//  This program is free software: you can redistribute it and/or modify
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
using System.Text;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal class ControllersManger : IControllersManger
    {
        private readonly Dictionary<string, Type> Command_ControllerMap;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Command_ControllerMap"></param>
        internal ControllersManger(Dictionary<string, Type> Command_ControllerMap)
        {
            this.Command_ControllerMap = Command_ControllerMap;
        }

        /// <summary>
        /// 获取控制器
        /// </summary>
        /// <param name="CommandName"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public object GetController(string CommandName, IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService(Command_ControllerMap[CommandName]);
        }

        /// <summary>
        /// 检查是否有支持该指令的控制器
        /// </summary>
        /// <param name="CommandName">要测试的指令</param>
        /// <returns></returns>
        public bool HasCommand(string CommandName)
        {
            return Command_ControllerMap.ContainsKey(CommandName);
        }
    }
}
