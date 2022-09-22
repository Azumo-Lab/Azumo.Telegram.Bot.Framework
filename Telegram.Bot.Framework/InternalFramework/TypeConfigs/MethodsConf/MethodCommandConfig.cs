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

namespace Telegram.Bot.Framework.InternalFramework.TypeConfigs.MethodsConf
{
    /// <summary>
    /// 
    /// </summary>
    internal class MethodCommandConfig : IMethodConfig
    {
        public List<CommandInfos> ConfigMethod(MethodInfo methodInfo, IEnumerable<Attribute> attributes)
        {
            HashSet<string> BotNames = null;

            //获取Class上的Bot名称
            BotNameAttribute botNameAttribute = attributes.Select(x => x as BotNameAttribute).Where(x => x != null).FirstOrDefault();
            if (botNameAttribute != null)
                BotNames = new HashSet<string>(botNameAttribute.BotName);

            CommandAttribute methodCommandAttr = (CommandAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(CommandAttribute));
            BotNameAttribute methodBotNameAttr = (BotNameAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(BotNameAttribute));
            if (methodCommandAttr != null)
            {
                //直接覆盖类上的Bot信息
                if (methodCommandAttr.BotName != null || methodBotNameAttr != null)
                {
                    BotNames = new HashSet<string>();
                    if (methodCommandAttr.BotName != null)
                        methodCommandAttr.BotName.ToList().ForEach(x => BotNames.Add(x));
                    if (methodBotNameAttr != null)
                        methodBotNameAttr.BotName.ToList().ForEach(x => BotNames.Add(x));
                }

                return new List<CommandInfos> { new CommandInfos
                {
                    CommandAttribute = methodCommandAttr,
                    BotNames = BotNames ?? new HashSet<string>(),
                    CommandMethod = methodInfo,
                    CommandName = methodCommandAttr.CommandName,
                    Controller = methodInfo.DeclaringType,
                }};
            }
            return Array.Empty<CommandInfos>().ToList();
        }

        public List<CommandInfos> ConfigMethod(List<CommandInfos> commandInfos)
        {
            throw new NotImplementedException();
        }
    }
}
