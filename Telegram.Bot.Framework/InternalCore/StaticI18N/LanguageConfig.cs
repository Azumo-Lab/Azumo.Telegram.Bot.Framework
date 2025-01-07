//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

namespace Telegram.Bot.Framework.InternalCore.StaticI18N
{
    /// <summary>
    /// 
    /// </summary>
    internal class LanguageConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Language? Language { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class Language
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Error_NotFound { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Customize { get; set; } = "{A0}";
    }
}
