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

namespace Telegram.Bot.Framework.Abstracts.Bot
{
    /// <summary>
    /// 
    /// </summary>
    public class LanguageStatic
    {
        /// <summary>
        /// 
        /// </summary>
        public static LanguageStatic Instance { get; } = new();

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<string, string> __Language = new();

        /// <summary>
        /// 
        /// </summary>
        private static readonly List<ILanguage> __LanguageList = new List<ILanguage>();

        /// <summary>
        /// 
        /// </summary>
        static LanguageStatic()
        {
            __LanguageList.AddRange(typeof(ILanguage).FindTypes<ILanguage>());
        }

        /// <summary>
        /// 
        /// </summary>
        private LanguageStatic()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (__Language.TryGetValue(key, out string? val))
                    return val;
                return string.Empty;
            }
        }

        public void Change(string name)
        {
            ILanguage language = __LanguageList.Where(x => x.LanguageName == name || x.LanguageName.ToLower() == name.ToLower()).FirstOrDefault()!;
            if (language == null)
                return;

            __Language.Clear();
            foreach (KeyValuePair<string, string> item in language.LanguageDescriptions)
                __Language.Add(item.Key, item.Value);
        }
    }
}
