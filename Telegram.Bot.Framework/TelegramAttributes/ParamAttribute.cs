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
using System.Text;
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;

namespace Telegram.Bot.Framework.TelegramAttributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParamAttribute : Attribute
    {
        /// <summary>
        /// 自定义的信息
        /// </summary>
        public string CustomInfos { get;}

        /// <summary>
        /// 自定义的消息发送
        /// </summary>
        public Type CustomMessageType { get; set; }

        /// <summary>
        /// 自定义的消息获取
        /// </summary>
        public Type CustomParamMaker { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Infos"></param>
        /// <param name="UseCustom"></param>
        public ParamAttribute(string Infos)
        {
            ThrowHelper.ThrowIfNull(Infos);

            CustomInfos = Infos;
        }
    }
}
