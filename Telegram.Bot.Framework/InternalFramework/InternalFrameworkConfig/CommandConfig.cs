//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Framework.InternalFramework.InternalFrameworkConfig
{
    /// <summary>
    /// 
    /// </summary>
    internal class CommandConfig
    {
        public IEnumerable<CommandInfos> ConfigCommand(Type ControllerType, HashSet<string> BotNames, HashSet<string> ControllerNames)
        {
            MethodInfo[] methods = ControllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo item in methods)
            {
                HashSet<string> Names = new BotNameConfig().ConfigBotName(item, BotNames, ControllerNames);

                CommandAttribute commandAttr = (CommandAttribute)Attribute.GetCustomAttribute(item, typeof(CommandAttribute));

                List<ParamInfos> Parainfo = new ParamterConfig().ConfigParamter(item).ToList();

                yield return new CommandInfos
                {
                    CommandName = commandAttr.CommandName,
                    BotName = Names,
                    Controller = ControllerType,
                    ParamInfos = Parainfo
                };
            }
        }
    }
}
