//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Authentication.Attribute;
using Telegram.Bot.Framework.Controller.Attribute;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.InternalImplementation.Controller
{
    /// <summary>
    /// 
    /// </summary>
    internal class ControllerContext : IControllerContext
    {
        internal ControllerContext(
            MethodInfo MethodInfo,
            Func<TelegramController, object[], Task> Action,
            List<Attribute> AttributeList,
            List<ParameterInfo> ParameterInfoList)
        {
            this.MethodInfo = MethodInfo;
            ControllerType = MethodInfo.DeclaringType;
            this.Action = Action;

            RuntimeHelpers.PrepareDelegate(this.Action);

            if (AttributeList.Where(x => x is BotCommandAttribute).FirstOrDefault() is BotCommandAttribute botCommandAttribute)
                BotCommandAttribute = botCommandAttribute;

            if (AttributeList.Where(x => x is AuthenticationAttribute).Count() > 1)
                AuthenticationAttribute = Attribute.GetCustomAttribute(MethodInfo, typeof(AuthenticationAttribute)) as AuthenticationAttribute;
            else if (AttributeList.Where(x => x is AuthenticationAttribute).FirstOrDefault() is AuthenticationAttribute authenticationAttribute)
                AuthenticationAttribute = authenticationAttribute;

            if (AttributeList.Where(x => x is DefaultTypeAttribute).FirstOrDefault() is DefaultTypeAttribute defaultTypeAttribute)
                DefaultTypeAttribute = defaultTypeAttribute;

            if (AttributeList.Where(x => x is DefaultMessageAttribute).FirstOrDefault() is DefaultMessageAttribute defaultMessageAttribute)
                DefaultMessageAttribute = defaultMessageAttribute;

            foreach (ParameterInfo item in ParameterInfoList)
            {
                ParamAttribute paramAttribute = Attribute.GetCustomAttribute(item, typeof(ParamAttribute)) as ParamAttribute;
                if (paramAttribute.IsNull())
                    throw new ArgumentNullException(nameof(paramAttribute));
                ParamModels.Add(new ParamModel
                {
                    ParamAttr = paramAttribute,
                    ParameterInfo = item,
                    ParamMaker = paramAttribute.ParamCatchClass,
                    ParamMsg = paramAttribute.MessageClass,
                    ParamType = item.ParameterType,
                });
            }
        }

        public BotCommandAttribute BotCommandAttribute { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public Type ControllerType { get; set; }

        public Func<TelegramController, object[], Task> Action { get; set; }

        public AuthenticationAttribute AuthenticationAttribute { get; set; }

        public DefaultTypeAttribute DefaultTypeAttribute { get; set; }

        public DefaultMessageAttribute DefaultMessageAttribute { get; set; }

        public List<ParamModel> ParamModels { get; } = new List<ParamModel>();
    }
}
