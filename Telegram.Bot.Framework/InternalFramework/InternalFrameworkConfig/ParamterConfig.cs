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
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Framework.InternalFramework.InternalFrameworkConfig
{
    /// <summary>
    /// 
    /// </summary>
    internal class ParamterConfig
    {
        private static List<Type> IParamMakerTypes;

        static ParamterConfig()
        {
            IParamMakerTypes = ServiceCollextionHelper.FilterBaseType(typeof(IParamMaker)).Where(x =>
            {
                return Attribute.IsDefined(x, typeof(ParamMakerAttribute));
            }).ToList();
        }

        public IEnumerable<ParamInfos> ConfigParamter(MethodInfo methodInfo)
        {
            ParameterInfo[] Paras = methodInfo.GetParameters();
            foreach (ParameterInfo item in Paras)
            {
                ParamAttribute attrParam = (ParamAttribute)Attribute.GetCustomAttribute(item, typeof(ParamAttribute));

                string message = null;
                Type MessageType = null;

                if (attrParam != null)
                {
                    message = attrParam.CustomInfos;
                    MessageType = attrParam.CustomMessageType;
                }

                Type maker = IParamMakerTypes.Where(x =>
                {
                    ParamMakerAttribute paramMaker = (ParamMakerAttribute)Attribute.GetCustomAttribute(x, typeof(ParamMakerAttribute));
                    return paramMaker.MakerType.FullName == item.ParameterType.FullName;
                }).FirstOrDefault();

                yield return new ParamInfos()
                {
                    MessageInfo = message,
                    MessageType = maker,
                };
            }
        }
    }
}
