using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Framework.Abstract.Languages;

namespace Telegram.Bot.Framework.Language
{
    /// <summary>
    /// 多语言切换管理的接口实现
    /// </summary>
    internal class MultiLanguage : IMultiLanguage
    {
        /// <summary>
        /// 加载所有的语言
        /// </summary>
        private readonly List<ILanguage> _languages;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceProvider"></param>
        public MultiLanguage(IServiceProvider serviceProvider) 
        {
            _languages = serviceProvider.GetServices<ILanguage>()?.ToList() ?? new List<ILanguage>();
            __NowLanguage = _languages.FirstOrDefault();
        }

        /// <summary>
        /// 获取当前语言的Key项目
        /// </summary>
        /// <param name="key">语言的key</param>
        /// <returns>返回对应的语言项目</returns>
        public string this[string key] => __NowLanguage.GetLanguageKeyValue()[key];

        /// <summary>
        /// 获取当前语言的名称
        /// </summary>
        public string Name 
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
        }

        public List<string> GetAllLanguageNames()
        {
            return _languages.Select(x => x.LanguageName).ToList();
        }
    }
}
