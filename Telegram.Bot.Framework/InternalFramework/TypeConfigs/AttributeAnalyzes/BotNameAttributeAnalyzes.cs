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
using Telegram.Bot.Framework.InternalFramework.TypeConfigs.Abstract;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Framework.InternalFramework.TypeConfigs.AttributeAnalyzes
{
    /// <summary>
    /// 
    /// </summary>
    internal class BotNameAttributeAnalyzes : IAttributeAnalyze
    {
        public Type AttributeType => typeof(BotNameAttribute);

        public Attribute Attribute { set; private get; }

        public CommandInfos Analyze(CommandInfos commandInfos, IAnalyze analyze)
        {
            BotNameAttribute botNameAttribute = (BotNameAttribute)Attribute;
            if (commandInfos.BotNames != null || botNameAttribute.OverWrite)
                commandInfos.BotNames = new HashSet<string>(botNameAttribute.BotName);
            else
                botNameAttribute.BotName.ToList().ForEach(x => commandInfos.BotNames.Add(x));

            return commandInfos;
        }

        public ICustomAttributeProvider GetMember()
        {
            throw new NotImplementedException();
        }
    }
}
