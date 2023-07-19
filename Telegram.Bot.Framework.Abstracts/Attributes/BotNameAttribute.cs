using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    /// <summary>
    /// 设定哪些Bot可以访问
    /// </summary>
    /// <remarks>
    /// 加上这个标签之后，需要Bot的名称符合设定的Bot名称之后，才可以正常进行访问
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class BotNameAttribute : Attribute
    {
        /// <summary>
        /// 设定的Bot名称，可以有多条数据
        /// </summary>
        public HashSet<string> BotNames { get; }

        /// <summary>
        /// 对名称进行设定
        /// </summary>
        /// <param name="BotNames">要设定的Bot名称</param>
        public BotNameAttribute(params string[] BotNames)
        {
            this.BotNames = new HashSet<string>(BotNames);
        }
    }
}
