using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.TelegramAttributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParamAttribute : Attribute
    {
        /// <summary>
        /// 自定义的信息
        /// </summary>
        public string CustomInfos { get;}

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName { get; }

        /// <summary>
        /// 是否自定义
        /// </summary>
        public bool UseCustom { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Infos"></param>
        /// <param name="UseCustom"></param>
        public ParamAttribute(string Infos, bool UseCustom)
        {
            if (UseCustom)
            {
                CustomInfos = Infos;
            }
            else
            {
                ParamName = Infos;
            }
        }
    }
}
