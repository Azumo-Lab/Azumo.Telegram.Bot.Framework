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

namespace Telegram.Bot.Framework.InternalFramework.LogImpl
{
    /// <summary>
    /// 
    /// </summary>
    internal class Logger : ILogger
    {
        public void Error()
        {
            throw new NotImplementedException();
        }

        public void Error(string message)
        {
            ConsoleLog.ConsoleWriteln(new List<ConsoleContext>
            {
                new ConsoleContext(){ Color = ConsoleColor.Red, Message = message },
            }.ToArray());
        }

        public void Error(Exception exception)
        {
            Error(exception.Message);
        }

        public void Info()
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            ConsoleLog.ConsoleWriteln(new List<ConsoleContext>
            {
                new ConsoleContext(){ Color = ConsoleColor.Green, Message = message }
            }.ToArray());
        }

        public void Warn()
        {
            throw new NotImplementedException();
        }

        public void Warn(string message)
        {
            ConsoleLog.ConsoleWriteln(new List<ConsoleContext>
            {
                new ConsoleContext(){ Color = ConsoleColor.Yellow, Message = message }
            }.ToArray());
        }
    }
}
