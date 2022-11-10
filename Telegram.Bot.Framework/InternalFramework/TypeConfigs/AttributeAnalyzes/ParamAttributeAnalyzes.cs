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
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.InternalFramework.ParameterManager;
using Telegram.Bot.Framework.InternalFramework.TypeConfigs.Abstract;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Framework.InternalFramework.TypeConfigs.AttributeAnalyzes
{
    /// <summary>
    /// 
    /// </summary>
    internal class ParamAttributeAnalyzes : IAttributeAnalyze
    {
        public Type AttributeType => typeof(ParamAttribute);

        public Attribute Attribute { set; private get; }

        public CommandInfos Analyze(CommandInfos commandInfos, IAnalyze analyze)
        {
            ParameterInfo parameterInfo = (ParameterInfo)analyze.GetMember();
            ParamAttribute paramAttribute = (ParamAttribute)Attribute;

            commandInfos.ParamInfos.Add(new ParamInfos
            {
                ParamType = parameterInfo.ParameterType,
                CustomMessageType = paramAttribute.CustomMessageType ?? typeof(StringParamMessage),
                CustomParamMaker = paramAttribute.CustomParamMaker ?? GetParamMaker(parameterInfo.ParameterType),
                MessageInfo = paramAttribute.CustomInfos,
            });
            return commandInfos;
        }

        public ICustomAttributeProvider GetMember()
        {
            return default;
        }

        private static Type GetParamMaker(Type type)
        {
            return TypesHelper.GetTypes<IParamMaker>().Where(x =>
                ((ParamTypeForAttribute)Attribute.GetCustomAttribute(x, typeof(ParamTypeForAttribute)))?.MakerType?.FullName == type.FullName
                ).FirstOrDefault();
        }
    }
}
