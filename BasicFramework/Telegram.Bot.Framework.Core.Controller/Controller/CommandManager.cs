//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Core.Controller.Controller;
internal class CommandManager : ICommandManager
{

    private readonly Dictionary<string, IExecutor> CommandExecutor = [];

    private readonly Dictionary<MessageType, IExecutor> MessageTypeExecutor = [];

    public void AddExecutor(IExecutor executor)
    {
        BotCommandAttribute? botCommandAttribute;
        if ((botCommandAttribute = executor.Attributes.Where(x => x is BotCommandAttribute).Select(x => x as BotCommandAttribute).FirstOrDefault()) != null)
            CommandExecutor.Add(botCommandAttribute.BotCommand, executor);
    }
    public IExecutor? GetExecutor(TelegramUserContext userContext)
    {
        var commands = userContext.GetCommand();
        if (commands != null)
        {
            if (CommandExecutor.TryGetValue(commands[0], out var executor))
                return executor;
        }
        else
        {
            if (userContext?.Message != null && MessageTypeExecutor.TryGetValue(userContext.Message.Type, out var executor))
                return executor;
        }
        return null;
    }

    public IReadOnlyList<IExecutor> GetExecutorList() =>
        CommandExecutor.Values.ToList();
}
