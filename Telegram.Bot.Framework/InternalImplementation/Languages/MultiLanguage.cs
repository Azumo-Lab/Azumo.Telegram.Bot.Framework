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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Framework.Abstract.Languages;
using Telegram.Bot.Framework.Attributes;

namespace Telegram.Bot.Framework.InternalImplementation.Languages
{
    /// <summary>
    /// 多语言切换管理的接口实现
    /// </summary>
    [DependencyInjection(ServiceLifetime.Singleton)]
    internal class MultiLanguage : IMultiLanguage
    {
        /// <summary>
        /// 加载所有的语言
        /// </summary>
        private readonly List<ILanguage> _languages;

        private Dictionary<string, string> __LanguageItems;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceProvider"></param>
        public MultiLanguage(IServiceProvider serviceProvider)
        {
            _languages = serviceProvider.GetServices<ILanguage>()?.ToList() ?? new List<ILanguage>();
            __NowLanguage = _languages.FirstOrDefault();
            __LanguageItems = __NowLanguage.GetLanguageKeyValue();
        }

        /// <summary>
        /// 获取当前语言的Key项目
        /// </summary>
        /// <param name="key">语言的key</param>
        /// <returns>返回对应的语言项目</returns>
        public string this[string key] => __LanguageItems[key];

        /// <summary>
        /// 获取当前语言的名称
        /// </summary>
        public string LanguageName
        {
            get
            {
                return __NowLanguage.LanguageName;
            }
        }

        /// <summary>
        /// 当前的语言
        /// </summary>
        private ILanguage __NowLanguage;

        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="name">想要切换的语言名称</param>
        public void ChangeLanguage(string name)
        {
            __NowLanguage = _languages.Where(x => x.LanguageName == name).FirstOrDefault();
            __LanguageItems = __NowLanguage.GetLanguageKeyValue();
        }

        public List<string> GetAllLanguageNames()
        {
            return _languages.Select(x => x.LanguageName).ToList();
        }
    }
}
