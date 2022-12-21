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
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Models;

namespace Telegram.Bot.Framework.Managers
{
    /// <summary>
    /// 
    /// </summary>
    internal class CommandManager : ICommandManager
    {
        internal ITypeManager _TypeManager;
        public CommandManager(ITypeManager typeManager)
        {
            _TypeManager = typeManager;
        }

        public bool ContainsCommand(string commandName)
        {
            return _TypeManager.ContainsCommandName(commandName);
        }

        public Dictionary<string, string> GetCommandInfos()
        {
            List<CommandInfos> commandInfos = _TypeManager.GetCommandInfos();
            return commandInfos
                .GroupBy(x => x.CommandAttribute.CommandName)
                .ToDictionary(x => x.Key, v => v.FirstOrDefault()?.CommandAttribute?.CommandInfo);
        }

        public string GetCommandInfoString()
        {
            StringBuilder sb = new StringBuilder();

            List<CommandInfos> commandInfos = _TypeManager.GetCommandInfos();

            foreach (CommandInfos item in commandInfos)
            {
                sb.AppendLine($"{item.CommandAttribute.CommandName} : {item.CommandAttribute.CommandInfo}");
            }
            return sb.ToString();
        }

        public void RegisterCommand(string commandName, string commandInfo)
        {
            throw new NotImplementedException();
        }

        public void RemoveCommand(string commandName)
        {
            throw new NotImplementedException();
        }

        public void RestoreCommand(string commandName)
        {
            throw new NotImplementedException();
        }
    }
}
