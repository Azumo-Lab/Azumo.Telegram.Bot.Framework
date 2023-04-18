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
using System.Text;
using Telegram.Bot.Framework.Abstract.Languages;

namespace Telegram.Bot.Framework.InternalImplementation.Languages
{
    /// <summary>
    /// 实现了部分 <see cref="ILanguage"/> 接口功能的语言基类
    /// </summary>
    public abstract class BaseLanguage : ILanguage
    {

        #region 语言处理

        public string LanguageName { get; protected set; } = string.Empty;

        private readonly Dictionary<string, string> LanguageDic = new Dictionary<string, string>();
        public BaseLanguage()
        {
            LoadLanguage();
        }

        
        public string this[string key]
        {
            get
            {
                if (LanguageDic.ContainsKey(key))
                    return LanguageDic[key];
                return string.Empty;
            }
            set
            {
                if (LanguageDic.ContainsKey(key))
                    LanguageDic[key] = value;
                else
                    LanguageDic.Add(key, value);
            }
        }

        /// <summary>
        /// 加载语言
        /// </summary>
        /// <remarks>
        /// 只需要写一个类似的：<br/>
        /// this[<see cref="ItemKey.StartInfo"/>] = "开始信息"; 
        /// </remarks>
        protected abstract void LoadLanguage();

        public Dictionary<string, string> GetLanguageKeyValue()
        {
            return LanguageDic;
        }

        #endregion

    }
}
