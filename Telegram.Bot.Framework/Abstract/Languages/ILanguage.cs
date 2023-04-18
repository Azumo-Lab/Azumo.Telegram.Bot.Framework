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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalImplementation.Languages;

namespace Telegram.Bot.Framework.Abstract.Languages
{
    /// <summary>
    /// 语言接口，需要实现
    /// </summary>
    /// <remarks>
    /// <para>
    /// 推荐实现 <see cref="BaseLanguage"/> 抽象类，这个抽象类已经实现了部分 <see cref="ILanguage"/> 接口的功能，
    /// 使用这个抽象类会更加的方便一点
    /// </para>
    /// <para>
    /// 语言字典的Key值有一个方便的类：<see cref="ItemKey"/> 这个类中包含了所有的使用到的Key值，只需要添加所有这些Key值的语言，即可完美实现不同的语言
    /// </para>
    /// </remarks>
    public interface ILanguage
    {
        /// <summary>
        /// 语言名称
        /// </summary>
        public string LanguageName { get; }

        /// <summary>
        /// 获取所有的语言项目
        /// </summary>
        /// <returns>语言字典</returns>
        public Dictionary<string, string> GetLanguageKeyValue();
    }
}
