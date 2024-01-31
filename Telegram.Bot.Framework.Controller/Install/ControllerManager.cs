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
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller.Install.Attr;

namespace Telegram.Bot.Framework.Controller.Install;
internal class ControllerManager
{
    public static ControllerManager Instance { get; } = new ControllerManager();

    private readonly static List<Type> _AllTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(x => x.GetTypes()).ToList();

    private readonly static List<Type> _ControllerType = [];

    static ControllerManager()
    {
        _ControllerType.AddRange(_AllTypes
            .Where(x => Attribute.IsDefined(x, typeof(TelegramControllerAttribute)))
            .ToList());

        IBotCommandBuilder builder = null;
        builder.AddAttribute(new BotCommandAttributeBuilder());

        foreach (var type in _ControllerType)
        {
            foreach (var item in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                builder.Build(item);
            }
        }
    }

    private ControllerManager()
    {

    }
}
