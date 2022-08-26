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
using System.Reflection;
using System.Text;
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal class DelegateManger : IDelegateManger
    {
        private readonly Dictionary<string, MethodInfo> Command_MethodMap;
        private readonly Dictionary<string, Delegate> Command_DelegateMap = new Dictionary<string, Delegate>();

        internal DelegateManger(Dictionary<string, MethodInfo> Command_MethodMap)
        {
            this.Command_MethodMap = Command_MethodMap;
        }

        public Delegate CreateDelegate(string Command, object controller)
        {
            if (!Command_DelegateMap.ContainsKey(Command))
            {
                Delegate action = DelegateHelper.CreateDelegate(Command_MethodMap[Command], controller);
                Command_DelegateMap.Add(Command, action);
            }
            return Command_DelegateMap[Command];
        }
    }
}
