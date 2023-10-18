using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Bots
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITelegramBotBuilder
    {
        /// <summary>
        /// 添加内容设置
        /// </summary>
        /// <param name="telegramPartCreator">可扩展的内容设置 <see cref="ITelegramPartCreator"/> </param>
        /// <returns></returns>
        public ITelegramBotBuilder AddTelegramPartCreator(ITelegramPartCreator telegramPartCreator);

        /// <summary>
        /// 设置完成后，进行 <see cref="ITelegramBot"/> 对象的构建
        /// </summary>
        /// <returns> 构建 <see cref="ITelegramBot"/> 对象 </returns>
        public ITelegramBot Build();
    }
}
