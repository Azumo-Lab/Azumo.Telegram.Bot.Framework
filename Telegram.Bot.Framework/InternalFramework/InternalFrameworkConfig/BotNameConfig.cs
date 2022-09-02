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
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Framework.TelegramException;

namespace Telegram.Bot.Framework.InternalFramework.InternalFrameworkConfig
{
    /// <summary>
    /// 
    /// </summary>
    internal class BotNameConfig
    {
        public HashSet<string> ConfigBotName(MemberInfo Type, HashSet<string> UseBotNames, HashSet<string> ControllerBotNames = null)
        {
            HashSet<string> BotNamesHashSet = new HashSet<string>();
            // 获取BotName
            BotNameAttribute BotNamesAttr = (BotNameAttribute)Attribute.GetCustomAttribute(Type, typeof(BotNameAttribute));

            if (BotNamesAttr != null)
            {
                foreach (string item in BotNamesAttr.BotName)
                    if (!UseBotNames.Contains(item))
                        throw new NotFoundBotNameException(item);
                    else if (!BotNamesHashSet.Add(item))
                        throw new RepeatedBotNameException(item);
                if (!BotNamesAttr.OverWrite)
                {
                    if (ControllerBotNames != null)
                        foreach (string item in ControllerBotNames)
                            UseBotNames.Add(item);
                }
            }

            return BotNamesHashSet;
        }
    }
}
