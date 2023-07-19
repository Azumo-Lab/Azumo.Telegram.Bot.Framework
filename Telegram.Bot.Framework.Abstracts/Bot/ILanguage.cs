using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Bot
{
    public interface ILanguage
    {
        public string LanguageName { get; }

        public Dictionary<string, string> LanguageDescriptions { get; }
    }

    public static class LanguageKey
    {
        /// <summary>
        /// 默认的指令详细信息
        /// </summary>
        public const string DefaultCommandDetails = nameof(DefaultCommandDetails);
    }
}
