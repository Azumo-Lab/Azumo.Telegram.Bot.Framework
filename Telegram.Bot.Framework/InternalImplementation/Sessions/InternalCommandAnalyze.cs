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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalImplementation.Sessions
{
    /// <summary>
    /// 
    /// </summary>
    internal class InternalCommandAnalyze : ICommandAnalyze
    {
        public bool HasCommand { get; set; }

        private (MessageEntity, string)[] __Values;
        private readonly IServiceProvider __ServiceProvider;

        public InternalCommandAnalyze(IServiceProvider serviceProvider)
        {
            __ServiceProvider = serviceProvider;
        }

        string ICommandAnalyze.GetCommand()
        {
            if (__Values.Length > 0)
            {
                (MessageEntity type, string command) = __Values[0];
                if (type.Type == Types.Enums.MessageEntityType.BotCommand)
                    return command;
            }
            return string.Empty;
        }

        Func<TelegramController, object[], Task> ICommandAnalyze.GetFunction()
        {
            throw new NotImplementedException();
        }

        List<(MessageEntity, string)> ICommandAnalyze.GetMessages()
        {
            return __Values.ToList();
        }

        void ICommandAnalyze.SetMessage((MessageEntity, string)[] values)
        {
            __Values = values;
        }
    }
}
