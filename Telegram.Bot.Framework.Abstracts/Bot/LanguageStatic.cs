using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
