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
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.InternalFramework.ParameterManager;
using Telegram.Bot.Framework.InternalFramework.TypeConfigs.Interface;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Framework.InternalFramework.TypeConfigs.ParamConfig
{
    /// <summary>
    /// 
    /// </summary>
    internal class ParamAttrConf : IParamAttrConf
    {
        public void AttributeConfig(ParameterInfo parameterInfo, ref ParamInfos paramInfos)
        {
            ParamAttribute paramAttribute = (ParamAttribute)Attribute.GetCustomAttribute(parameterInfo, typeof(ParamAttribute));
            if (paramAttribute != null)
            {
                paramInfos.CustomMessageType = paramAttribute.CustomMessageType;
                paramInfos.MessageInfo = paramAttribute.CustomInfos;
                paramInfos.CustomParamMaker = paramAttribute.CustomParamMaker;
            }
            //默认值
            paramInfos.MessageInfo ??= "请输入值：";
            //发送文本消息
            paramInfos.CustomMessageType ??= typeof(StringParamMessage);
            //默认根据参数类型选择
            paramInfos.CustomParamMaker ??= TypesHelper.GetTypes<IParamMaker>().Where(x =>
            {
                ParamMakerAttribute paramMakerAttribute = (ParamMakerAttribute)Attribute.GetCustomAttribute(x, typeof(ParamMakerAttribute));
                if (paramMakerAttribute != null)
                {
                    return paramMakerAttribute.MakerType.FullName == parameterInfo.ParameterType.FullName;
                }
                return false;
            }).FirstOrDefault();
            paramInfos.ParamType = parameterInfo.ParameterType;
        }
    }
}
