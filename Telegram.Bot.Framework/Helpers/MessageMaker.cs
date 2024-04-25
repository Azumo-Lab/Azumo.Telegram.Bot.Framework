//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

namespace Telegram.Bot.Framework.Helpers;

/// <summary>
/// 
/// </summary>
public static class MessageMaker
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string Link(string text, string url) =>
        $"<a href=\"{url}\">{text}</a>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Bold(string text) =>
        $"<b>{text}</b>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Italic(string text) =>
        $"<i>{text}</i>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Underline(string text) =>
        $"<u>{text}</u>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Strikethrough(string text) =>
        $"<s>{text}</s>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Spoiler(string text) =>
        $"<tg-spoiler>{text}</tg-spoiler>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Code(string text) =>
        $"<code>{text}</code>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Pre(string text) =>
        $"<pre>{text}</pre>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Blockquote(string text) =>
        $"<blockquote>{text}</blockquote>";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string HashTag(string text) =>
        $"<a>{text}</a>";
}
