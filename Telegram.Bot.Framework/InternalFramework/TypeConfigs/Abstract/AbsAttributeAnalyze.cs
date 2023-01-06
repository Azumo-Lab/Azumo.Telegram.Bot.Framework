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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.Models;

namespace Telegram.Bot.Framework.InternalFramework.TypeConfigs.Abstract
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class AbsAttributeAnalyze : IAnalyze
    {
        public IServiceProvider ServiceProvider { set; protected get; }

        protected List<Attribute> Attributes { get; } = new List<Attribute>();

        public CommandInfos Analyze(CommandInfos commandInfos, IAnalyze analyze)
        {
            List<IAttributeAnalyze> attributeAnalyzes = ServiceProvider.GetServices<IAttributeAnalyze>().ToList();
            foreach (Attribute attr in Attributes)
            {
                IAttributeAnalyze attributeAnalyze = attributeAnalyzes.Where(x => x.AttributeType.FullName == attr.GetType().FullName).FirstOrDefault();
                if (attributeAnalyze != null)
                {
                    attributeAnalyze.Attribute = attr;
                    attributeAnalyze.Analyze(commandInfos, analyze);
                }
            }
            return commandInfos;
        }
        public virtual CommandInfos Analyze(CommandInfos commandInfos)
        {
            throw new NotImplementedException("请在子类中继承并重写本方法");
        }

        public abstract ICustomAttributeProvider GetMember();
    }
}
