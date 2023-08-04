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
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.User;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalProc.User
{
    internal class InternalRequest : IRequest
    {
        private readonly IServiceProvider serviceProvider;
        public Update Update { get; set; }

        public InternalRequest(Update update, IServiceProvider serviceProvider)
        {
            Update = update;
            this.serviceProvider = serviceProvider;
        }

        public string GetCommand()
        {
            return HasCommand() ? Update.Message.Text : string.Empty;
        }

        public Message GetMessage()
        {
            return Update.Message;
        }

        public bool HasCommand()
        {
            if (Update.Type != Types.Enums.UpdateType.Message)
                return false;

            string command = Update.Message?.Text ?? string.Empty;
            return command.StartsWith('/') && command.Length > 1;
        }

        public ICommandInfo Find()
        {
            IControllerFinder controllerFinder = serviceProvider.GetService<IControllerFinder>();
            string command = GetCommand();
            if (string.IsNullOrEmpty(command))
                return controllerFinder.Find(GetMessage().Type);
            return controllerFinder.Find(command);
        }
    }
}
