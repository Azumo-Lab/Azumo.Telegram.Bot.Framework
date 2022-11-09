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

namespace Telegram.Bot.Framework.InternalFramework.TypeConfigs.Analyzes
{
    /// <summary>
    /// 方法的解析
    /// </summary>
    internal class MethodAnalyze : AbsAttributeAnalyze
    {
        private MethodInfo MethodInfo { get; }

        public MethodAnalyze(MethodInfo MethodInfo)
        {
            this.MethodInfo = MethodInfo;
        }
        public override CommandInfos Analyze(CommandInfos commandInfos)
        {
            Attributes.AddRange(Attribute.GetCustomAttributes(MethodInfo));
            Analyze(commandInfos, this);
            List<ParameterInfo> parameterInfos = MethodInfo.GetParameters().ToList();
            foreach (ParameterInfo item in parameterInfos)
            {
                ParamAnalyze paramAnalyze = new ParamAnalyze(item);
                paramAnalyze.ServiceProvider = ServiceProvider;
                paramAnalyze.Analyze(commandInfos);
            }
            return commandInfos;
        }

        public override ICustomAttributeProvider GetMember()
        {
            return MethodInfo;
        }
    }
}
