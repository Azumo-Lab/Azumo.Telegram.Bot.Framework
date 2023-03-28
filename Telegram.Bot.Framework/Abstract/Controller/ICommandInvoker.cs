﻿//  <Telegram.Bot.Framework>
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

using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalImplementation.Controller;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstract.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommandInvoker
    {
        public Task CommandInvoke(string command, params object[] param);

        public Task CommandInvoke(MessageType messageType, params object[] param);

        public Task CommandInvoke(CommandInfo commandInfo, TelegramController controller, params object[] param);

    }
}