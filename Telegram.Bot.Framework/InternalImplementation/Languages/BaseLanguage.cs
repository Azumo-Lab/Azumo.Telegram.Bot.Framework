using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Framework.Abstract.Languages;

namespace Telegram.Bot.Framework.InternalImplementation.Languages
{
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

        protected abstract void LoadLanguage();

        public Dictionary<string, string> GetLanguageKeyValue()
        {
            return LanguageDic;
        }

        #endregion

    }
}
