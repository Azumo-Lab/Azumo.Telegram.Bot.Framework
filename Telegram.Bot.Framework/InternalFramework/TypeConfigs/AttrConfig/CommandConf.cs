//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.InternalFramework.TypeConfigs.Interface;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Framework.InternalFramework.TypeConfigs.AttrConfig
{
    /// <summary>
    /// 
    /// </summary>
    internal class CommandConf : IAttributeConfig
    {
        public void AttributeConfig(MethodInfo methodInfo, IEnumerable<Attribute> ClassAttrs, ref CommandInfos commandInfos)
        {
            CommandAttribute commandAttribute = (CommandAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(CommandAttribute));
            if (commandAttribute == null)
                return;

            commandInfos.CommandAttribute = commandAttribute;
            commandInfos.CommandMethod = methodInfo;
            commandInfos.CommandName = commandAttribute.CommandName.ToLower();
            if (commandAttribute.BotName != null)
                commandInfos.BotNames = new HashSet<string>(commandAttribute.BotName);
            else
            {
                string[] botname = ClassAttrs.Where(x => (x as BotNameAttribute) != null).Select(x => x as BotNameAttribute).FirstOrDefault()?.BotName;
                if (botname != null)
                    commandInfos.BotNames = new HashSet<string>(botname);
            }
            commandInfos.Controller = methodInfo.DeclaringType;
        }
    }
}
