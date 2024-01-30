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

namespace Telegram.Bot.Framework.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = false)]
public class BotCommandAttribute : Attribute
{
    /// <summary>
    /// 
    /// </summary>
    public string BotCommandName { get; }

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    private const char BotCommandStartChar = '/';

    /// <summary>
    /// 
    /// </summary>
    /// <param name="MessageType"></param>
    /// <param name="BotCommandName"></param>
    public BotCommandAttribute(string BotCommandName)
    {
        BotCommandName = BotCommandName.ToLower();
        if (!BotCommandName.StartsWith(BotCommandStartChar))
            BotCommandName = $"/{BotCommandName}";
        this.BotCommandName = BotCommandName.ToLower();
    }

    /// <summary>
    /// 
    /// </summary>
    public BotCommandAttribute() => BotCommandName = null!;
}
