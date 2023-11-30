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

using System.Reflection;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    public class BotCommand
    {
        private string __BotCommandName = string.Empty;
        public string BotCommandName
        {
            get => __BotCommandName;
            set
            {
                string botCommandName = value;
                if (!string.IsNullOrEmpty(botCommandName))
                {
                    if (!botCommandName.StartsWith('/'))
                        botCommandName = $"/{botCommandName}";
                }
                __BotCommandName = botCommandName;
            }
        }

        public string Description { get; set; } = string.Empty;

        public MessageType MessageType { get; set; } = MessageType.Unknown;

        public Func<TelegramController, object[], Task> Func { get; set; } = (controller, objs) => Task.CompletedTask;

        public MethodInfo MethodInfo { get; set; } = null!;

        public Type Controller { get; set; } = null!;

        public List<IControllerParam> ControllerParams { get; set; } = [];

        public AuthenticateAttribute AuthenticateAttribute { get; set; } = null!;
    }
}
