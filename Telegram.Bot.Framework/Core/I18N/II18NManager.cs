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

namespace Telegram.Bot.Framework.Core.I18N;

/// <summary>
/// 
/// </summary>
public interface II18NManager
{
    /// <summary>
    /// 
    /// </summary>
    public const string Chinese = "Chinses";

    /// <summary>
    /// 
    /// </summary>
    public II18N Current { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="i18N"></param>
    public void Add(II18N i18N);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void Change(string name);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void Remove(string name);
}
