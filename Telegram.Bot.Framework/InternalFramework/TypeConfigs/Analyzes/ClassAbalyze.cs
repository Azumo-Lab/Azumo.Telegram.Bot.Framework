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
    /// 类的解析
    /// </summary>
    internal class ClassAbalyze : AbsAttributeAnalyze
    {
        private Type Type { get; }
        public ClassAbalyze(Type Type)
        {
            this.Type = Type;
        }

        List<CommandInfos> commands = new List<CommandInfos>();

        public List<CommandInfos> Analyze()
        {
            Analyze(null);
            return commands;
        }

        public override CommandInfos Analyze(CommandInfos command)
        {
            Attributes.AddRange(Attribute.GetCustomAttributes(Type));
            foreach (MethodInfo item in Type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                CommandInfos commandInfos = new CommandInfos();
                Analyze(commandInfos, this);
                MethodAnalyze methodAnalyze = new MethodAnalyze(item);
                methodAnalyze.ServiceProvider = ServiceProvider;
                methodAnalyze.Analyze(commandInfos);
                if (commandInfos.CommandMethod == null)
                    continue;
                commands.Add(commandInfos);
            }
            return default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICustomAttributeProvider GetMember()
        {
            return Type;
        }
    }
}
