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

namespace Telegram.Bot.Framework;
public static partial class Extensions
{
    /// <summary>
    /// 将数组转换成字典
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ToDictionary(this string[] args)
    {
        Dictionary<string, string> result = [];
        for (var i = 0; i < args.Length; i++)
            if (i + 1 >= args.Length)
                result.Add(args[i], string.Empty);
            else
                result.Add(args[i], args[++i]);
        return result;
    }
}
