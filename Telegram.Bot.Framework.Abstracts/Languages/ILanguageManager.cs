using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.Abstracts.Languages
{
    public interface ILanguageManager
    {
        public string this[string key] { get; }

        public void ChangeTo(string language);

        public ILanguage GetCurrentInstance();

        public ILanguage GetLanguage(string language);
    }
}
