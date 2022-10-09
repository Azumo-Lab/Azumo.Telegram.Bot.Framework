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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.InternalFramework.TypeConfigs.Interface;

namespace Telegram.Bot.Framework.InternalFramework.TypeConfigs.ParamConf
{
    /// <summary>
    /// 
    /// </summary>
    internal class MethodParamConfig : IParamConfig
    {
        private IServiceProvider serviceProvider;
        public MethodParamConfig(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public void ParamConfig(MethodInfo methodInfo, ref CommandInfos commandInfos)
        {
            IEnumerable<IParamAttrConf> paramAttrConfs = serviceProvider.GetServices<IParamAttrConf>();
            List<ParamInfos> paramInfos = new();
            foreach (ParameterInfo item in methodInfo.GetParameters())
            {
                ParamInfos paramInfo = new();
                foreach (IParamAttrConf conf in paramAttrConfs)
                {
                    conf.AttributeConfig(item, ref paramInfo);
                }
                paramInfos.Add(paramInfo);
            }
            commandInfos.ParamInfos = paramInfos;
        }
    }
}
