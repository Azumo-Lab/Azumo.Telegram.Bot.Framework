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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstract.Languages
{
    /// <summary>
    /// 多语言对应
    /// </summary>
    /// <remarks>
    /// 要添加一种语言，请实现：<see cref="ILanguage"/> 接口。 <br/>
    /// 实现 <see cref="ILanguage"/> 接口之后，使用 <see cref="BuilerSetup.AddLanguage{T}(Bots.IBuilder)"/> 来注册安装接口。 <br/>
    /// 默认已经实现了中文接口 名称是 ：‘Chinese’
    /// </remarks>
    public interface IMultiLanguage
    {
        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="language">语言的名称</param>
        public void ChangeLanguage(string name);

        /// <summary>
        /// 当前语言名称
        /// </summary>
        public string LanguageName { get; }

        /// <summary>
        /// 获取所有语言
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllLanguageNames();

        /// <summary>
        /// 获取当前语言的指定Key项目
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] { get; }
    }
}
